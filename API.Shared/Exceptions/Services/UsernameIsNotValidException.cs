using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class UsernameIsNotValidException : BaseException
    {
        public UsernameIsNotValidException() : base(System.Net.HttpStatusCode.BadRequest, "Username_Is_Not_Valid")
        {
        }
    }
}
