using Microsoft.AspNetCore.Identity;
using StudentManagement.Core.Interfaces.Services;
using StudentManagement.Core.ViewModel;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Register(RegisterViewModel viewModel)
        {
            var user = new AppUser()
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email
            };

            var res = await _userManager.CreateAsync(user, viewModel.Password);
            if (!res.Succeeded)
            {
                throw new Exception("Register fail");
            }

            return true;
        }

        public async Task<AccountViewModel> Login(LoginViewModel viewModel)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null) 
            {
                throw new Exception("Your email doesn't exists");
            }

            var res = await _signInManager.PasswordSignInAsync(user, viewModel.Password, isPersistent: false, lockoutOnFailure: false);
            if (!res.Succeeded)
            {
                throw new Exception("Your email or password is incorrect");
            }

            return new AccountViewModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.PhoneNumber
            };
        }

        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
