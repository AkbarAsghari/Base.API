using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class TokenNameIsNotValidException : BaseException
    {
        public TokenNameIsNotValidException() : base(System.Net.HttpStatusCode.BadRequest, "TokenName_Is_Not_Valid")
        {
        }
    }
}
