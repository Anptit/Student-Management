using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Domain.Entities;
using StudentManagement.Infrastructure.EntityFramework;

namespace StudentManagement.Infrastructure.Repositories
{
    public class StudentInClassRepository : RepositoryBase<StudentInClass>, IStudentInClassRepository
    {
        public StudentInClassRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
