using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class TokenUsedBeforeException : BaseException
    {
        public TokenUsedBeforeException() : base(System.Net.HttpStatusCode.BadRequest, "Token_Used_Before")
        {
        }
    }
}
