using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PageBodyCanNotBeEmptyException : BaseException
    {
        public PageBodyCanNotBeEmptyException() : base(HttpStatusCode.BadRequest, "Page_Body_Can_Not_Be_Empty")
        {
        }
    }
}
