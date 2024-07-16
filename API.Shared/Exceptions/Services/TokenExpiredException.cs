using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class TokenExpiredException : BaseException
    {
        public TokenExpiredException() : base(System.Net.HttpStatusCode.BadRequest, "Token_Expired")
        {
        }
    }
}
