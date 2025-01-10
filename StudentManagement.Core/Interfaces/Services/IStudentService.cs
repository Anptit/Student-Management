using StudentManagement.Core.ViewModel;

namespace StudentManagement.Core.Interfaces.Services
{
    public interface IStudentService
    {
        Task<bool> Create(StudentViewModel viewModel);
        Task<bool> Update(int studentId, StudentViewModel viewModel);
        Task<bool> Delete(int studentId);
        Task<StudentViewModel> Get(int studentId);
        Task<IEnumerable<StudentInfoViewModel>> GetAll(string search, string classSearch, int pageSize, int pageIndex);
    }
}
