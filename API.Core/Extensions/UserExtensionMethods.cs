using API.Shared.Enums;
using API.Infrastructure.Entities;

namespace API.Core.Extensions
{
    public static class UserExtensionMethods
    {
        public static IEnumerable<Users> WithoutPasswords(this IEnumerable<Users> users)
        {
            if (users == null) return null;

            return users.Select(x => x.WithoutPassword());
        }

        public static Users WithoutPassword(this Users user)
        {
            if (user == null) return null;

            user.Password = null;
            user.PasswordSalt = null;
            return user;
        }

        public static string GetFullName(this Users user) => user.FirstName != null ? user.FirstName : (user.LastName == null ? "ناشناس" : String.Empty);
    }
}
