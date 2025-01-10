namespace StudentManagement.Core.ViewModel
{
    public class ClassViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = default!;
        public string? Description { get; set; }
        public int TotalStudent { get; set; }
        public List<StudentViewModel>? Students { get; set; }
    }
}
