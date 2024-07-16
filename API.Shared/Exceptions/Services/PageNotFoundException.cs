using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PageNotFoundException : BaseException
    {
        public PageNotFoundException() : base(HttpStatusCode.NotFound, "Page_Not_Found")
        {
        }
    }
}
