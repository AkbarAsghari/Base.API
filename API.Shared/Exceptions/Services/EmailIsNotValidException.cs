using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class EmailIsNotValidException : BaseException
    {
        public EmailIsNotValidException() : base(System.Net.HttpStatusCode.BadRequest, "Email_Is_Not_Valid")
        {
        }
    }
}
