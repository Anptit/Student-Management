using StudentManagement.Core.ViewModel;

namespace StudentManagement.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterViewModel viewModel);
        Task<AccountViewModel> Login(LoginViewModel viewModel);
        Task<bool> Logout(); 
    }
}
