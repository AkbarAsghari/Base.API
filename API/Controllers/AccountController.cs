using API.Shared.DTOs.Users;
using API.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Shared.Extensions;
using API.Shared;
using API.Shared.Enums;
using API.Attributes;

namespace API.Controllers
{

    /// <summary>
    /// All user side activities are done in this section
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AccountController(IUserService userService, IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Authenticate user with user and password 
        /// </summary>
        /// <remarks>Rate limit 23 times per day</remarks>
        /// <response code="200">If the authentication is successful, you will receive a token.</response>
        [ProducesResponseType(typeof(AuthUserDTO), 200)]
        [AllowAnonymous]
        [HttpPost("authenticateAsync")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateDTO model)
        {
            return Ok(await _userService.AuthenticateAsync(model.UsernameOrEmail, model.Password));
        }

        [AllowAnonymous]
        [HttpPost("GenerateTokenWithRefreshTokenAsync")]
        public async Task<IActionResult> GenerateTokenWithRefreshTokenAsync([FromBody] RefreshTokenDTO model)
        {
            return Ok(await _userService.GenerateTokenWithRefreshTokenAsync(model.RefreshToken));
        }

        /// <summary>
        /// Send a link to change your password if you forget it
        /// </summary>
        /// <remarks>Rate limit 3 times per day</remarks>
        /// <response code="200">If the email is successful, you will receive Ture</response>
        [ProducesResponseType(typeof(bool), 200)]
        [AllowAnonymous]
        [HttpPost("ForgetPasswordAsync")]
        public async Task<IActionResult> ForgetPasswordAsync([FromBody] ForgetPasswordDTO model)
        {
            return Ok(await _userService.ForgetPasswordAsync(model));
        }

        /// <summary>
        /// Password reset using the emailed token in Forgot Password
        /// </summary>
        /// <remarks>Rate limit 7 times per day</remarks>
        /// <response code="200">If the Reset Password is successful, you will receive Ture</response>
        [ProducesResponseType(typeof(bool), 200)]
        [AllowAnonymous]
        [HttpPost("ResetPasswordAsync")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO model)
        {
            return Ok(await _userService.ResetPasswordAsync(model));
        }

        /// <summary>
        /// Change password (required old password)
        /// </summary>
        /// <response code="200">If the Change Password is successful, you will receive Ture</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("ChangePasswordAsync")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO model)
        {
            return Ok(await _userService.ChangePasswordAsync(_contextAccessor!.HttpContext!.GetClaimsUserID(), model));
        }

        /// <summary>
        /// Change User Role *Admin*
        /// </summary>
        /// <response code="200">If the Change Role is successful, you will receive Ture</response>
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(Roles = AuthorizeRoles.Admin)]
        [HttpPut("ChangeUserRoleAsync")]
        public async Task<IActionResult> ChangeUserRoleAsync([FromQuery] Guid userId, RolesEnum role)
        {
            return Ok(await _userService.ChangeUserRoleAsync(userId, role));
        }

        /// <summary>
        /// Register user in api database
        /// </summary>
        /// <remarks>Rate limit 0 times (Register user is avilable only from dnslab website)</remarks>
        /// <response code="200">If the register is successful, you will receive a token.</response>
        [ProducesResponseType(typeof(AuthUserDTO), 200)]
        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        [AllowAnonymous]
        [HttpPost("RegisterAsync")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDTO model)
        {
            return Ok(await _userService.RegisterAsync(model));
        }

        /// <summary>
        /// Confirm email with Token that Emailed
        /// </summary>
        /// <remarks>Rate limit 3 times Per hour</remarks>
        /// <response code="200">If the confirmation is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [AllowAnonymous]
        [HttpPost("ConfirmEmailWithTokenAsync")]
        public async Task<IActionResult> ConfirmEmailWithTokenAsync([FromQuery] string token)
        {
            return Ok(await _userService.ConfirmEmailWithTokenAsync(token));
        }

        /// <summary>
        /// Resend confirm email token
        /// </summary>
        /// <remarks>Rate limit 3 times Per Day</remarks>
        /// <response code="200">If the resend confirmation email is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPost("ResendConfirmEmailTokenAsync")]
        public async Task<IActionResult> ResendConfirmEmailTokenAsync()
        {
            return Ok(await _userService.ResendConfirmEmailTokenAsync(_contextAccessor!.HttpContext!.GetClaimsUserID()));
        }

        /// <summary>
        /// Update user basic information
        /// </summary>
        /// <response code="200">If the update is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("updateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserPersonalInfoDTO model)
        {
            return Ok(await _userService.UpdateAsync(_contextAccessor!.HttpContext!.GetClaimsUserID(), model));
        }


        /// <summary>
        /// Update user Username
        /// </summary>
        /// <remarks>Rate limit 20 times per day</remarks>
        /// <response code="200">If the update is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("UpdateUsernameAsync")]
        public async Task<IActionResult> UpdateUsernameAsync([FromQuery] string? username)
        {
            return Ok(await _userService.UpdateUsernameAsync(_contextAccessor!.HttpContext!.GetClaimsUserID(), username));
        }

        /// <summary>
        /// Change email address
        /// </summary>
        /// <remarks>Rate limit 3 times per day</remarks>
        /// <response code="200">If the change is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("ChangeEmailAsync")]
        public async Task<IActionResult> ChangeEmailAsync([FromQuery] string email)
        {
            return Ok(await _userService.ChangeEmailAsync(_contextAccessor!.HttpContext!.GetClaimsUserID(), email));
        }

        /// <summary>
        /// Change mobile
        /// </summary>
        /// <response code="200">If the change is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("ChangeMobileAsync")]
        public async Task<IActionResult> ChangeMobileAsync([FromQuery] string mobile)
        {
            return Ok(await _userService.ChangeMobileAsync(_contextAccessor!.HttpContext!.GetClaimsUserID(), mobile));
        }

        /// <summary>
        /// Get user information by header authorize
        /// </summary>
        /// <response code="200">If successful, you will receive user information.</response>
        [ProducesResponseType(typeof(UserDTO), 200)]
        [HttpGet("GetCurrentUserAsync")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            return Ok(await _userService.GetByIDAsync(_contextAccessor!.HttpContext!.GetClaimsUserID()));
        }

        /// <summary>
        /// Get active users count
        /// </summary>
        /// <response code="200">If successful, you will receive active users count.</response>
        [ProducesResponseType(typeof(int), 200)]
        [AllowAnonymous]
        [HttpGet("UsersCountAsync")]
        public async Task<IActionResult> UsersCountAsync()
        {
            return Ok(await _userService.UsersCountAsync());
        }

        /// <summary>
        /// Get user information by user Id *Admin*
        /// </summary>
        /// <response code="200">If successful, you will receive active user information.</response>
        [ProducesResponseType(typeof(UserDTO), 200)]
        [Authorize(Roles = AuthorizeRoles.Admin)]
        [HttpGet("GetUserAsync")]
        public async Task<IActionResult> GetUserAsync([FromQuery] Guid Id)
        {
            return Ok(await _userService.GetByIDAsync(Id));
        }

        /// <summary>
        /// Get all users information *Admin*
        /// </summary>
        /// <response code="200">If successful, you will receive all active users information.</response>
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        [Authorize(Roles = AuthorizeRoles.Admin)]
        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _userService.GetAllAsync());
        }

        /// <summary>
        /// Deactivate user by authorize header *Admin* (NA)
        /// </summary>
        /// <response code="200">If successful, you will receive True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpDelete("DeactivateAccountAsync")]
        public async Task<IActionResult> DeactivateAccountAsync()
        {
            return Ok(await _userService.DeactivateAccountAsync(_contextAccessor!.HttpContext!.GetClaimsUserID()));
        }

    }
}
