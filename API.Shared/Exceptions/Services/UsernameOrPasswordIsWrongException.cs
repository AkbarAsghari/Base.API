using API.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class UsernameOrPasswordIsWrongException : BaseException
    {
        public UsernameOrPasswordIsWrongException() : base(System.Net.HttpStatusCode.NotFound,
            "Username_Or_Password_Is_Wrong",
                 new Dictionary<ExceptionLanguageEnum, object?> {
                     { ExceptionLanguageEnum.Fa, "نام کاربری یا رمز عبور اشتباه است" },
                     { ExceptionLanguageEnum.En, "Username or password is wrong" }}
                 )
        { }
    }
}
