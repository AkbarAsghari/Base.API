using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class OnlyLetterAndDashAcceptedInURLException : BaseException
    {
        public OnlyLetterAndDashAcceptedInURLException() : base(HttpStatusCode.BadRequest, "Only_Letter_And_Dash_Accepted_In_URL")
        {
        }
    }
}
