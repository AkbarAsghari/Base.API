using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class UsernameOrPasswordIsWrongException : BaseException
    {
        public UsernameOrPasswordIsWrongException() : base(System.Net.HttpStatusCode.Unauthorized, "Username_Or_Password_Is_Wrong") { }
    }
}
