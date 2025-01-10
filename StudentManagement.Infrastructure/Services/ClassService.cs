using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Core.Interfaces.Services;
using StudentManagement.Core.Utitlities;
using StudentManagement.Core.ViewModel;
using StudentManagement.Domain.Entities;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StudentManagement.Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classService;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentInClassRepository _studentInClassRepository;

        public ClassService
        (
           IClassRepository classService, 
           IStudentInClassRepository studentInClassRepository,
           IStudentRepository studentRepository
        ) 
        {
            _classService = classService;
            _studentInClassRepository = studentInClassRepository;
            _studentRepository = studentRepository;
        }
        public async Task<bool> Create(ClassViewModel viewModel)
        {
            var newClass = new Class()
            {
                Name = viewModel.ClassName,
                CreateAt = DateTime.Now
            };

            await _classService.Add(newClass);

            if (viewModel.Students != null && viewModel.Students.Count > 0)
            {
                foreach (var student in viewModel.Students) 
                {
                    var studentInClass = new StudentInClass()
                    {
                        ClassId = newClass.Id,
                        StudentId = student.StudentId,
                        DeleteAt = DateTime.MinValue, 
                        CreateAt = DateTime.Now,
                        UppdateAt = DateTime.Now
                    };

                    await _studentInClassRepository.Add(studentInClass);
                }
            }

            return true;
        }

        public async Task<bool> Delete(int classId)
        {
            var getClass = _classService.FirstOrDefault(x=>x.Id == classId);

            if (getClass == null)
            {
                throw new Exception("Class not found");
            }

            var studentInClass = await _studentInClassRepository.QueryAsync(x => x.ClassId == classId);

            if (studentInClass != null)
            {
                foreach (var student in studentInClass)
                {
                    await _studentInClassRepository.Delete(student);
                }
            }

            await _classService.Delete(getClass);

            return true;
        }

        public async Task<ClassViewModel> Get(int classId)
        {
            var classGet = await _classService.Get(classId, "StudentInClasses");

            if (classGet == null)
            {
                throw new Exception("Class not found");
            }

            var studentInClass = new List<StudentViewModel>();

            if (classGet.StudentInClasses != null && classGet.StudentInClasses.Count() > 0)
            {
                var studentIds = classGet.StudentInClasses
                                         .Select(x => x.StudentId)
                                         .ToList();

                studentInClass = (await _studentRepository.QueryAndSelectAsync(x => new StudentViewModel()
                                                                                    {
                                                                                        StudentId = x.Id,
                                                                                        StudentName = x.StudentName
                                                                                     },x => studentIds.Contains(x.Id))).ToList();
            }

        
            return new ClassViewModel() 
            {
                ClassId = classId,
                ClassName = classGet.Name,
                Description = classGet.Description,
                Students = studentInClass,
            };
        }

        public async Task<IEnumerable<ClassViewModel>> GetAll(string search, int pageSize, int pageIndex)
        {
            Expression<Func<Class, bool>> filter = i => !i.IsDeleted;

            if (!string.IsNullOrEmpty(search))
            {
                filter = PredicateBuilder
                    .AndAlso(filter, i => i.Name.Contains(search));
            }

            var classes = await _classService.QueryAsync(filter,null,"StudentInClasses", pageSize, pageIndex);

            List<ClassViewModel> viewModels = new List<ClassViewModel>();
            foreach (var c in classes)
            {
                var studentInClass = new List<StudentViewModel>();

                if (c.StudentInClasses != null && c.StudentInClasses.Count() > 0)
                {
                    var studentIds = c.StudentInClasses
                                             .Select(x => x.StudentId)
                                             .ToList();

                    studentInClass = (await _studentRepository.QueryAndSelectAsync(x => new StudentViewModel()
                    {
                        StudentId = x.Id,
                        StudentName = x.StudentName

                    }, x => studentIds.Contains(x.Id))).ToList();
                }
                viewModels.Add(new ClassViewModel()
                {
                    ClassId = c.Id,
                    ClassName = c.Name,
                    Students = studentInClass
                });
            }

            return viewModels;
        }

        public async Task<bool> Update(ClassViewModel viewModel)
        {
            var existedClass = _classService.FirstOrDefault(x=>x.Id == viewModel.ClassId,null, "StudentInClasses");

            if (existedClass == null)
            {
                throw new Exception("Class not found");
            }

            existedClass.Name = viewModel.ClassName;
            existedClass.Description = viewModel.Description;
            existedClass.TotalStudent = viewModel.TotalStudent;
            existedClass.UppdateAt = DateTime.Now;

            await _classService.Update(existedClass);

            if (viewModel.Students.Any()) 
            {

                if (existedClass.StudentInClasses != null && existedClass.StudentInClasses.Count() > 0)
                {
                    var studentIds = existedClass.StudentInClasses
                                                 .Select(x => x.StudentId)
                                                 .ToList();

                    // logic delete student not exists in model
                    var studentOlds = studentIds.Except(viewModel.Students.Select(x => x.StudentId).ToList());

                    foreach (var student in studentOlds) 
                    {
                        var deleteStudent = _studentInClassRepository.FirstOrDefault(x => x.ClassId == viewModel.ClassId && x.StudentId == student);

                        if(deleteStudent != null)
                        {
                            await _studentInClassRepository.Delete(deleteStudent); 
                        }
                    }

                    // logic add new student in class
                    var studentNews = viewModel.Students.Select(x => x.StudentId).ToList().Except(studentIds);

                    foreach (var student in studentNews)
                    {
                        var newStudent = new StudentInClass()
                        {
                            ClassId = viewModel.ClassId,
                            StudentId = student
                        };

                        await _studentInClassRepository.Add(newStudent);
                    }

                    return true;
                }

                foreach (var student in viewModel.Students)
                {
                    var newStudent = new StudentInClass()
                    {
                        ClassId = viewModel.ClassId,
                        StudentId = student.StudentId
                    };

                    await _studentInClassRepository.Add(newStudent);
                }
            }

            return true;
        }
    }
}
