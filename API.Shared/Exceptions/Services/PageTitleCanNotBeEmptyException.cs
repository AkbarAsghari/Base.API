using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PageTitleCanNotBeEmptyException : BaseException
    {
        public PageTitleCanNotBeEmptyException() : base(HttpStatusCode.BadRequest, "Page_Title_Can_Not_Be_Empty")
        {
        }
    }
}
