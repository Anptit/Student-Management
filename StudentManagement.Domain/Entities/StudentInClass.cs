namespace StudentManagement.Domain.Entities
{
    public class StudentInClass : BaseEntity
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public Class? Class { get; set; }
        public Student? Student { get; set; }
    }
}
