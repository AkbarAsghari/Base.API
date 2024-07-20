using API.Shared.Enums;
using API.Infrastructure.Entities;

namespace API.Core.Extensions
{
    public static class UserExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            if (users == null) return null;

            return users.Select(x => x.WithoutPassword());
        }

        public static User WithoutPassword(this User user)
        {
            if (user == null) return null;

            user.Password = null;
            user.PasswordSalt = null;
            return user;
        }

        public static string GetFullName(this User user) => user.FirstName != null ? user.FirstName : (user.LastName == null ? "ناشناس" : String.Empty);
    }
}
