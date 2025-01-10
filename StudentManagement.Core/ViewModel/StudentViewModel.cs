using StudentManagement.Domain.Enum;

namespace StudentManagement.Core.ViewModel
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Phone {  get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? Address { get; set; }
    }
}
