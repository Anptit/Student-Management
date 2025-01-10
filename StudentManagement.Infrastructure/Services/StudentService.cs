
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Core.Interfaces.Services;
using StudentManagement.Core.Utitlities;
using StudentManagement.Core.ViewModel;
using StudentManagement.Domain.Entities;
using System.Linq.Expressions;
using System.Net;

namespace StudentManagement.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentInClassRepository _studentInClassRepo;
        private readonly IClassRepository _classRepo;
        public StudentService(
            IStudentRepository studentRepo,
            IStudentInClassRepository studentInClassRepo,
            IClassRepository classRepo
        ) 
        {
            _studentRepo = studentRepo;
            _studentInClassRepo = studentInClassRepo;
            _classRepo = classRepo;
        }

        public async Task<bool> Create(StudentViewModel viewModel)
        {
            var student = new Student()
            {
                StudentName = viewModel.StudentName,
                DOB = viewModel.DOB,
                Phone = viewModel.Phone,
                Address = viewModel.Address
            };

            await _studentRepo.Add(student);
            return true;
        }

        public async Task<bool> Delete(int studentId)
        {
            var student = await _studentRepo.Get(studentId);
            if (student == null)
            {
                throw new Exception("Student not found");
            }

            var studentInClass = await _studentInClassRepo.QueryAsync(sc => sc.StudentId == student.Id);
            foreach (var sc in studentInClass)
            {
                await _studentInClassRepo.Delete(sc);
            }
            await _studentRepo.Delete(student);

            return true;
        }

        public async Task<StudentViewModel> Get(int studentId)
        {
            var student = await _studentRepo.Get(studentId);
            if (student == null)
            {
                throw new Exception("Student not found");
            }

            return new StudentViewModel()
            {
                StudentId = studentId,
                StudentName = student.StudentName,
                Phone = student.Phone,
                DOB = student.DOB,
                Address = student.Address
            };
        }

        public async Task<IEnumerable<StudentInfoViewModel>> GetAll(string search, string classSearch, int pageSize, int pageIndex)
        {
            Expression<Func<Student, bool>> filter = i => !i.IsDeleted;

            if (!string.IsNullOrEmpty(search))
            {
                filter = PredicateBuilder
                    .AndAlso(filter, i => i.StudentName != null && i.StudentName.Contains(search));
            }

            if (!string.IsNullOrEmpty(classSearch))
            {
                filter = PredicateBuilder
                    .AndAlso(filter, i => i.StudentInClasses.Any(sc => sc.Class != null && sc.Class.Name.Contains(classSearch)));
            }

            var students = await _studentRepo.QueryAsync(filter, null, "StudentInClasses", pageSize, pageIndex);
            List<StudentInfoViewModel> viewModels = new List<StudentInfoViewModel>();
            foreach (var student in students)
            {
                var classOfStudent = new List<ClassViewModel>();
                if (student.StudentInClasses != null && student.StudentInClasses.Count() > 0) {
                    var classIds = student.StudentInClasses.Select(s => s.ClassId).ToList();
                    classOfStudent = (await _classRepo.QueryAndSelectAsync(x => new ClassViewModel()
                    {
                        ClassId = x.Id,
                        ClassName = x.Name

                    }, x => classIds.Contains(x.Id))).ToList();
                }

                viewModels.Add(new StudentInfoViewModel()
                {
                    StudentId = student.Id,
                    StudentName = student.StudentName,
                    Phone = student.Phone,
                    DOB = student.DOB,
                    Address = student.Address,
                    BelongClass = classOfStudent
                });
            };
            
            return viewModels;
    }

        public async Task<bool> Update(int studentId, StudentViewModel viewModel)
        {
            var student =  _studentRepo.FirstOrDefault(x => x.Id == studentId);
            if (student == null) 
            {
                throw new Exception("Student not found");
            }

            student.StudentName = viewModel.StudentName;
            student.DOB = viewModel.DOB;
            student.Address = viewModel.Address;
            student.Phone = viewModel.Phone;
            await _studentRepo.Update(student);

            return true;
        }
    }
}
