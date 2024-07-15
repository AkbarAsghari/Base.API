using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PageURLCanNotBeEmptyException : BaseException
    {
        public PageURLCanNotBeEmptyException() : base(HttpStatusCode.BadRequest, "Page_URL_Can_Not_Be_Empty")
        {
        }
    }
}
