using Microsoft.AspNetCore.Identity;
using ModelLayer.DTO.AuthDtos;
using ModelLayer.Models;

namespace DataLayer.Services.Interfaces
{
    public interface IAuth
    {
        public Task<CustomIdentityUser> FindUserAsync(string id);
        public Task<AuthModel> OnRegisterAsync(RegisterDTO regInfo);
        public Task<AuthModel> OnLoginAsync(LoginDTO logInfo);
        public Task<string> CreateJWTAsync(CustomIdentityUser user);
        public Task<string> OnSetRoleAsync(RoleSetModelDTO requestInfo);
        public void SendEmail(EmailDTO emailObj);
        public Task<bool> ForgotPasswordAsync(string email);
        public Task<bool> ResetPasswordAsync(ResetPasswordModelDTO modelInfo);
        public Task<string> GetUserNameById(string id);
        public Task<CustomIdentityUser> FindUserByEmailAsync(string email);
        public Task<IdentityResult> UpdataUserData(CustomIdentityUser user);
    }
}
