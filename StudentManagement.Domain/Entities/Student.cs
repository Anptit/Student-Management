using StudentManagement.Domain.Enum;

namespace StudentManagement.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string? StudentName { get; set; }
        public Gender? Gender { get; set; }
        public string? Phone { get; set; }
        public DateTime? DOB { get; set; }
        public string? Address {  get; set; }
        public IEnumerable<StudentInClass>? StudentInClasses { get; set; }
    }
}
