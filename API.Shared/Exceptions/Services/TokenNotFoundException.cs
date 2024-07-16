using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class TokenNotFoundException : BaseException
    {
        public TokenNotFoundException() : base(System.Net.HttpStatusCode.NotFound, "Token_Not_Found")
        {
        }
    }
}
