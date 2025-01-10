
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Domain.Entities;
using StudentManagement.Infrastructure.EntityFramework;

namespace StudentManagement.Infrastructure.Repositories
{
    public class ClassRepository : RepositoryBase<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
