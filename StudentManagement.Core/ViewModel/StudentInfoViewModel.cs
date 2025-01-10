using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enum;

namespace StudentManagement.Core.ViewModel
{
    public class StudentInfoViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = default!;
        public string? Phone {  get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? Address { get; set; }
        public List<ClassViewModel>? BelongClass { get; set; }
    }
}
