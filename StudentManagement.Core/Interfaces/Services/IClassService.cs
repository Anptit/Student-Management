using StudentManagement.Core.ViewModel;

namespace StudentManagement.Core.Interfaces.Services
{
    public interface IClassService
    {
        Task<bool> Create(ClassViewModel viewModel);
        Task<bool> Update(ClassViewModel viewModel);
        Task<bool> Delete(int classId);
        Task<ClassViewModel> Get(int classId);
        Task<IEnumerable<ClassViewModel>> GetAll(string search, int pageSize, int pageIndex);
    }
}
