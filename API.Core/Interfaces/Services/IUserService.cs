using API.Shared.DTOs.Users;
using API.Shared.Enums;

namespace DevMark.ApplicationCore.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthUserDTO> Authenticate(string usernameOrEmail, string password);
        Task<AuthUserDTO> RegisterAsync(RegisterUserDTO registerUser);
        Task<bool> Update(Guid userId, UpdateUserPersonalInfoDTO updateUser);
        Task<bool> UpdateUsername(Guid userId, string? Username);
        Task<bool> ChangeUserRole(Guid userId, RolesEnum role);

        Task<UserDTO> GetByIDAsync(Guid ID);
        Task<IEnumerable<UserDTO>> GetAll();
        Task<int> UsersCount();

        Task<bool> ForgetPassword(ForgetPasswordDTO forgetPassword);
        Task<bool> ResetPassword(ResetPasswordDTO resetPassword);
        Task<bool> ChangePassword(Guid userId, ChangePasswordDTO changePassword);

        Task<bool> ChangeEmail(Guid userId, string email);

        Task<bool> ResendConfirmEmailToken(Guid userId);
        Task<bool> ConfirmEmailWithToken(string token);

        Task<bool> DeactivateAccount(Guid ID);
    }
}
