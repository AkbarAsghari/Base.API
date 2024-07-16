using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PageDescriptionCanNotBeEmptyException : BaseException
    {
        public PageDescriptionCanNotBeEmptyException() : base(HttpStatusCode.BadRequest, "Page_Description_Can_Not_Be_Empty")
        {
        }
    }
}
