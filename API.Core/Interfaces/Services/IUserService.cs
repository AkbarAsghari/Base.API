using API.Shared.DTOs.Users;
using API.Shared.Enums;

namespace DevMark.ApplicationCore.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthUserDTO> AuthenticateAsync(string usernameOrEmail, string password);
        Task<AuthUserDTO> RegisterAsync(RegisterUserDTO registerUser);
        Task<bool> UpdateAsync(Guid userId, UpdateUserPersonalInfoDTO updateUser);
        Task<bool> UpdateUsernameAsync(Guid userId, string? Username);
        Task<bool> ChangeUserRoleAsync(Guid userId, RolesEnum role);

        Task<UserDTO> GetByIDAsync(Guid ID);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<int> UsersCountAsync();

        Task<bool> ForgetPasswordAsync(ForgetPasswordDTO forgetPassword);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPassword);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDTO changePassword);

        Task<bool> ChangeEmailAsync(Guid userId, string email);
        Task<bool> ChangeMobileAsync(Guid userId, string mobile);

        Task<bool> ResendConfirmEmailTokenAsync(Guid userId);
        Task<bool> ConfirmEmailWithTokenAsync(string token);

        Task<bool> DeactivateAccountAsync(Guid ID);
    }
}
