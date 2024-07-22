using API.Core.Extensions;
using API.Core.Interfaces.Services;
using API.Infrastructure.DataAcess;
using API.Infrastructure.Entities;
using API.Infrastructure.Interfaces;
using API.Shared;
using API.Shared.DTOs.Users;
using API.Shared.Enums;
using API.Shared.Exceptions;
using API.Shared.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;
        private readonly IEmailNotificationService _EmailNotificationService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailNotificationService emailNotificationService)
        {
            _UnitOfWork = unitOfWork;
            _Mapper = mapper;
            _EmailNotificationService = emailNotificationService;
        }

        public async Task<AuthUserDTO> AuthenticateAsync(string usernameOrEmail, string password)
        {
            if (string.IsNullOrEmpty(usernameOrEmail.Trim()))
                throw new UsernameIsNotValidException();

            User? user;
            if (RegexTool.IsValidEmail(usernameOrEmail.Trim()))
                user = await _UnitOfWork.Repository<User>().FirstOrDefault(x => x.Email.ToLower() == usernameOrEmail.ToLower() && x.Active == true);
            else
                user = await _UnitOfWork.Repository<User>().FirstOrDefault(x => x.Username!.ToLower() == usernameOrEmail.ToLower() && x.Active == true);

            if (user == null)
                throw new UsernameOrPasswordIsWrongException();


            if (!HashTool.VerifyPassword(password, user.Password, user.PasswordSalt))
                throw new UsernameOrPasswordIsWrongException();

            string token = GenerateJWTToken(user);

            return new AuthUserDTO(token);
        }

        private string GenerateJWTToken(User user)
        {
            // generate token that is valid for 365 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWTSettings.Secret);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Username == null ? user.Email : user.Username));
            claims.Add(new Claim("FullName", string.IsNullOrEmpty((user.FirstName + user.LastName).Trim()) ? user.Email : $"{user.FirstName} {user.LastName}"));
            claims.Add(new Claim(ClaimTypes.Role, ((RolesEnum)user.Role).ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<UserDTO> GetByIDAsync(Guid ID)
        {
            var user = await _UnitOfWork.Repository<User>()
                .FirstOrDefault(x => x.ID == ID);

            var nonePasswordUser = user.WithoutPassword();

            return _Mapper.Map<UserDTO>(nonePasswordUser);
        }

        public async Task<AuthUserDTO> RegisterAsync(RegisterUserDTO registerUser)
        {
            using (var transaction = await _UnitOfWork.GetDBTransaction)
            {
                if (registerUser.Password.Length < 8)
                    throw new PasswordLessThan8CharacterException();
                if (!RegexTool.IsValidEmail(registerUser.Email))
                    throw new EmailIsNotValidException();

                if (await _UnitOfWork.Repository<User>().AnyAsync(x => x.Email.ToLower() == registerUser.Email.ToLower()))
                    throw new EmailIsDuplicateException();

                User user = new User();

                user.Email = registerUser.Email;
                user.Active = true;
                user.IsEmailApproved = false;
                user.Role = (int)RolesEnum.User;

                string passwordHash, passwordSalt;
                HashTool.CreatePasswordHash(registerUser.Password, out passwordHash, out passwordSalt);

                user.Password = passwordHash;
                user.PasswordSalt = passwordSalt;

                _UnitOfWork.Repository<User>().Insert(user);

                await _UnitOfWork.Save();

                await SendConfirmEmailToken(user.ID, EmailTypeEnum.ConfirmEmail);

                transaction.Commit();
                return new AuthUserDTO(GenerateJWTToken(user));
            }
        }

        public async Task<bool> ConfirmEmailWithTokenAsync(string token)
        {
            var confirmUserTicket = await _UnitOfWork.Repository<ResetPasswordTickets>().IgnoreQueryFilters()
                .SingleOrDefault(x => x.Token == token);

            if (confirmUserTicket == null)
                throw new TokenNotFoundException();
            if (DateTime.UtcNow > confirmUserTicket.ExpirationDate)
                throw new TokenExpiredException();
            if (confirmUserTicket.IsTokenUsed)
                throw new TokenUsedBeforeException();

            var existUser = await _UnitOfWork.Repository<User>().SingleOrDefault(x => x.ID == confirmUserTicket.UserId);

            existUser.IsEmailApproved = true;

            _UnitOfWork.Repository<User>().Update(existUser);

            confirmUserTicket.IsTokenUsed = true;
            _UnitOfWork.Repository<ResetPasswordTickets>().Update(confirmUserTicket);

            return await _UnitOfWork.Save();
        }

        public async Task<bool> UpdateAsync(Guid userId, UpdateUserPersonalInfoDTO updateUser)
        {
            using (var transaction = await _UnitOfWork.GetDBTransaction)
            {
                var existUser = await _UnitOfWork.Repository<User>()
                    .FirstOrDefault(x => x.ID == userId);

                if (existUser == null)
                    throw new UserNotFoundException();

                existUser.FirstName = updateUser.FirstName;
                existUser.LastName = updateUser.LastName;
                existUser.Company = updateUser.Company;

                _UnitOfWork.Repository<User>().Update(existUser);
                await _UnitOfWork.Save();
                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            return _Mapper.Map<IEnumerable<UserDTO>>(await _UnitOfWork.Repository<User>().GetEntities()
            .OrderByDescending(x => x.CreateDate).ToListAsync());
        }

        public async Task<int> UsersCountAsync()
        {
            return await _UnitOfWork.Repository<User>().GetEntities().CountAsync();
        }
        public async Task<bool> ForgetPasswordAsync(ForgetPasswordDTO forgetPassword)
        {
            if (!RegexTool.IsValidEmail(forgetPassword.Email))
                throw new EmailIsNotValidException();

            var existUser = await _UnitOfWork.Repository<User>().FirstOrDefault(x => x.Email.ToLower() == forgetPassword.Email.ToLower());
            if (existUser == null)
                throw new UserNotFoundException();

            string token = HashTool.GenerateToken();

            var link = $"https://dnslab.link/User/ResetPassword/{token}";

            if (await _EmailNotificationService.SendForgetPasswordEmail(
                userId: existUser.ID,
                fullName: $"{existUser.FirstName} {existUser.LastName}",
                link: link))
            {
                _UnitOfWork.Repository<ResetPasswordTickets>().Insert(new ResetPasswordTickets
                {
                    UserId = existUser.ID,
                    Token = token,
                    ExpirationDate = DateTime.UtcNow.AddDays(1),
                    IsTokenUsed = false
                });
                return await _UnitOfWork.Save();
            }

            return false;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPassword)
        {
            var resetPasswordTicket = await _UnitOfWork.Repository<ResetPasswordTickets>().IgnoreQueryFilters()
                .SingleOrDefault(x => x.Token == resetPassword.Token);

            if (resetPasswordTicket == null)
                throw new TokenNotFoundException();

            if (DateTime.UtcNow > resetPasswordTicket.ExpirationDate)
                throw new TokenExpiredException();
            if (resetPasswordTicket.IsTokenUsed)
                throw new TokenUsedBeforeException();

            var existUser = await _UnitOfWork.Repository<User>().SingleOrDefault(x => x.ID == resetPasswordTicket.UserId);

            string passwordHash, passwordSalt;
            HashTool.CreatePasswordHash(resetPassword.Password, out passwordHash, out passwordSalt);

            existUser.Password = passwordHash;
            existUser.PasswordSalt = passwordSalt;

            _UnitOfWork.Repository<User>().Update(existUser);

            resetPasswordTicket.IsTokenUsed = true;
            _UnitOfWork.Repository<ResetPasswordTickets>().Update(resetPasswordTicket);

            return await _UnitOfWork.Save();
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDTO changePassword)
        {
            if (changePassword.NewPassword.Length < 8)
                throw new PasswordLessThan8CharacterException();

            var existUser = await _UnitOfWork.Repository<User>().SingleOrDefault(x => x.ID == userId);
            if (existUser == null)
                throw new UserNotFoundException();

            if (!HashTool.VerifyPassword(changePassword.CurrentPassword, existUser.Password, existUser.PasswordSalt))
                throw new OldPasswordIsWrongException();

            string passwordHash, passwordSalt;
            HashTool.CreatePasswordHash(changePassword.NewPassword, out passwordHash, out passwordSalt);

            existUser.Password = passwordHash;
            existUser.PasswordSalt = passwordSalt;

            _UnitOfWork.Repository<User>().Update(existUser);
            return await _UnitOfWork.Save();
        }

        public async Task<bool> DeactivateAccountAsync(Guid ID)
        {
            return false;

            var user = await _UnitOfWork.Repository<User>().SingleOrDefault(x => x.ID == ID);
            if (user == null)
                throw new UserNotFoundException();

            user.Active = false;
            _UnitOfWork.Repository<User>().Update(user);

            return await _UnitOfWork.Save();
        }

        public async Task<bool> ChangeEmailAsync(Guid userId, string email)
        {
            using (var transaction = await _UnitOfWork.GetDBTransaction)
            {
                if (!RegexTool.IsValidEmail(email))
                    throw new EmailIsNotValidException();

                if (await _UnitOfWork.Repository<User>().IgnoreQueryFilters().AnyAsync(x => x.Email.ToLower() == email.ToLower() && x.ID != userId))
                    throw new EmailIsDuplicateException();

                var existUser = await _UnitOfWork.Repository<User>().FirstOrDefault(x => x.ID == userId);

                if (existUser == null)
                    throw new UserNotFoundException();
                if (existUser.Email == email.Trim())
                    return true;

                existUser.Email = email.Trim();
                existUser.IsEmailApproved = false;

                _UnitOfWork.Repository<User>().Update(existUser);

                await _UnitOfWork.Save();

                await SendConfirmEmailToken(userId, EmailTypeEnum.ConfirmEmail);
                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> ChangeMobileAsync(Guid userId, string mobile)
        {
            using (var transaction = await _UnitOfWork.GetDBTransaction)
            {
                if (!RegexTool.IsValidMobile(mobile))
                    throw new MobileIsNotValidException();

                if (await _UnitOfWork.Repository<User>().IgnoreQueryFilters().AnyAsync(x => x.Mobile == mobile.Trim() && x.ID != userId))
                    throw new MobileIsDuplicateException();

                var existUser = await _UnitOfWork.Repository<User>().FirstOrDefault(x => x.ID == userId);

                if (existUser == null)
                    throw new UserNotFoundException();
                if (existUser.Mobile == mobile.Trim())
                    return true;

                existUser.Mobile = mobile.Trim();

                _UnitOfWork.Repository<User>().Update(existUser);

                await _UnitOfWork.Save();

                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> ResendConfirmEmailTokenAsync(Guid userId)
        {
            var existUser = await _UnitOfWork.Repository<User>().FirstOrDefault(x => x.ID == userId);

            if (existUser == null)
                throw new UserNotFoundException();

            return await SendConfirmEmailToken(userId, EmailTypeEnum.ResendActivationLink);
        }

        public async Task<bool> SendConfirmEmailToken(Guid userId, EmailTypeEnum emailType)
        {
            string token = HashTool.GenerateToken();

            var link = $"https://dnslab.link/User/ConfirmEmail/{token}";

            if (await _EmailNotificationService.SendConfirmEmail(
            userId: userId,
                link: link,
                resend: emailType == EmailTypeEnum.ResendActivationLink))
            {
                _UnitOfWork.Repository<ResetPasswordTickets>().Insert(new ResetPasswordTickets
                {
                    UserId = userId,
                    Token = token,
                    ExpirationDate = DateTime.UtcNow.AddDays(1),
                    IsTokenUsed = false
                });
                return await _UnitOfWork.Save();
            }
            return false;
        }

        public async Task<bool> ChangeUserRoleAsync(Guid userId, RolesEnum role)
        {
            var existUser = await _UnitOfWork.Repository<User>().FirstOrDefault(x => x.ID == userId);

            if (existUser == null)
                throw new UserNotFoundException();

            existUser.Role = (int)role;

            _UnitOfWork.Repository<User>().Update(existUser);

            return await _UnitOfWork.Save();
        }

        public async Task<bool> UpdateUsernameAsync(Guid userId, string? Username)
        {
            using (var transaction = await _UnitOfWork.GetDBTransaction)
            {
                if (string.IsNullOrWhiteSpace(Username))
                    Username = null;
                else
                {
                    if (!RegexTool.IsValidUsername(Username))
                        throw new UsernameIsNotValidException();

                    if (await _UnitOfWork.Repository<User>().AnyAsync(x => x.Username!.ToLower() == Username.ToLower() && x.ID != userId))
                        throw new UsernameIsDuplicateException();
                }

                var existUser = await _UnitOfWork.Repository<User>()
                        .FirstOrDefault(x => x.ID == userId);

                if (existUser == null)
                    throw new UserNotFoundException();

                existUser.Username = Username;

                await _UnitOfWork.Save();

                await transaction.CommitAsync();
                return true;
            }
        }
    }
}
