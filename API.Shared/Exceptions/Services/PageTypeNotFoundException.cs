using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PageTypeNotFoundException : BaseException
    {
        public PageTypeNotFoundException() : base(HttpStatusCode.BadRequest, "Page_Type_Not_Found")
        {
        }
    }
}
