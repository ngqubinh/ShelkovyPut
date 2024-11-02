using Application.ViewModels.Auth;

namespace Application.Interfaces
{
    public interface IAuthService 
    {
        Task ForgotPassword(ForgotPasswordVM model);
    }
}
