using Application.Interfaces;
using Application.ViewModels.Auth;
using Domain.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Infrastructure.Repositories.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task ForgotPassword(ForgotPasswordVM model)
        {
            var user = await _userManager.FindByNameAsync(model.Email!);
            if(user == null || !(await _userManager.IsPhoneNumberConfirmedAsync(user)))
            {
                throw new Exception("User not found or email not cofirmed");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"https://yourwebsite.com/Account/ResetPassword?token={token}&email={model.Email}";
            await _emailSender.SendEmailAsync(
            model.Email,
            "Reset Password",
            $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");
        }
    }
}
