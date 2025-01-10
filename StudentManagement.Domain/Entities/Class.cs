using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Entities
{
    public class Class : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int TotalStudent { get; set; }
        public IEnumerable<StudentInClass>? StudentInClasses { get; set; }
    }
}
