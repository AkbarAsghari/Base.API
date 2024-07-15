using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PasswordLessThan8CharacterException : BaseException
    {
        public PasswordLessThan8CharacterException() : base(HttpStatusCode.BadRequest, "Password_Less_Than_8_Character")
        {
        }
    }
}
