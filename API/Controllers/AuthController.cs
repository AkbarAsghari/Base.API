using API.Shared.DTOs.Users;
using DevMark.ApplicationCore.Interfaces.Services;
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
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthController(IUserService userService, IHttpContextAccessor contextAccessor)
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
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateDTO model)
        {
            return Ok(await _userService.Authenticate(model.UsernameOrEmail, model.Password));
        }

        /// <summary>
        /// Send a link to change your password if you forget it
        /// </summary>
        /// <remarks>Rate limit 3 times per day</remarks>
        /// <response code="200">If the email is successful, you will receive Ture</response>
        [ProducesResponseType(typeof(bool), 200)]
        [AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordAsync([FromBody] ForgetPasswordDTO model)
        {
            return Ok(await _userService.ForgetPassword(model));
        }

        /// <summary>
        /// Password reset using the emailed token in Forgot Password
        /// </summary>
        /// <remarks>Rate limit 7 times per day</remarks>
        /// <response code="200">If the Reset Password is successful, you will receive Ture</response>
        [ProducesResponseType(typeof(bool), 200)]
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO model)
        {
            return Ok(await _userService.ResetPassword(model));
        }

        /// <summary>
        /// Change password (required old password)
        /// </summary>
        /// <response code="200">If the Change Password is successful, you will receive Ture</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            return Ok(await _userService.ChangePassword(_contextAccessor!.HttpContext!.GetClaimsUserID(), model));
        }

        /// <summary>
        /// Change User Role *Admin*
        /// </summary>
        /// <response code="200">If the Change Role is successful, you will receive Ture</response>
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(Roles = AuthorizeRoles.Admin)]
        [HttpPut("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole([FromQuery] Guid userId, RolesEnum role)
        {
            return Ok(await _userService.ChangeUserRole(userId, role));
        }

        /// <summary>
        /// Register user in api database
        /// </summary>
        /// <remarks>Rate limit 0 times (Register user is avilable only from dnslab website)</remarks>
        /// <response code="200">If the register is successful, you will receive a token.</response>
        [ProducesResponseType(typeof(AuthUserDTO), 200)]
        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO model)
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
        [HttpPost("ConfirmEmailWithToken")]
        public async Task<IActionResult> ConfirmEmailWithToken([FromQuery] string token)
        {
            return Ok(await _userService.ConfirmEmailWithToken(token));
        }

        /// <summary>
        /// Resend confirm email token
        /// </summary>
        /// <remarks>Rate limit 3 times Per Day</remarks>
        /// <response code="200">If the resend confirmation email is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPost("ResendConfirmEmailToken")]
        public async Task<IActionResult> ResendConfirmEmailToken()
        {
            return Ok(await _userService.ResendConfirmEmailToken(_contextAccessor!.HttpContext!.GetClaimsUserID()));
        }

        /// <summary>
        /// Update user basic information
        /// </summary>
        /// <response code="200">If the update is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserPersonalInfoDTO model)
        {
            return Ok(await _userService.Update(_contextAccessor!.HttpContext!.GetClaimsUserID(), model));
        }


        /// <summary>
        /// Update user Username
        /// </summary>
        /// <remarks>Rate limit 20 times per day</remarks>
        /// <response code="200">If the update is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("UpdateUsername")]
        public async Task<IActionResult> UpdateUsername([FromQuery] string? username)
        {
            return Ok(await _userService.UpdateUsername(_contextAccessor!.HttpContext!.GetClaimsUserID(), username));
        }

        /// <summary>
        /// Change email address
        /// </summary>
        /// <remarks>Rate limit 3 times per day</remarks>
        /// <response code="200">If the change is successful, you will receive a True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPut("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDTO model)
        {
            return Ok(await _userService.ChangeEmail(_contextAccessor!.HttpContext!.GetClaimsUserID(), model));
        }

        /// <summary>
        /// Get user information by header authorize
        /// </summary>
        /// <response code="200">If successful, you will receive user information.</response>
        [ProducesResponseType(typeof(UserDTO), 200)]
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetByIDAsync(_contextAccessor!.HttpContext!.GetClaimsUserID()));
        }

        /// <summary>
        /// Get active users count
        /// </summary>
        /// <response code="200">If successful, you will receive active users count.</response>
        [ProducesResponseType(typeof(int), 200)]
        [AllowAnonymous]
        [HttpGet("UsersCount")]
        public async Task<IActionResult> UsersCount()
        {
            return Ok(await _userService.UsersCount());
        }

        /// <summary>
        /// Get user information by user Id *Admin*
        /// </summary>
        /// <response code="200">If successful, you will receive active user information.</response>
        [ProducesResponseType(typeof(UserDTO), 200)]
        [Authorize(Roles = AuthorizeRoles.Admin)]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser([FromQuery] Guid Id)
        {
            return Ok(await _userService.GetByIDAsync(Id));
        }

        /// <summary>
        /// Get all users information *Admin*
        /// </summary>
        /// <response code="200">If successful, you will receive all active users information.</response>
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        [Authorize(Roles = AuthorizeRoles.Admin)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }

        /// <summary>
        /// Deactivate user by authorize header *Admin* (NA)
        /// </summary>
        /// <response code="200">If successful, you will receive True.</response>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpDelete("DeactivateAccount")]
        public async Task<IActionResult> DeactivateAccount()
        {
            return Ok(await _userService.DeactivateAccount(_contextAccessor!.HttpContext!.GetClaimsUserID()));
        }

    }
}
