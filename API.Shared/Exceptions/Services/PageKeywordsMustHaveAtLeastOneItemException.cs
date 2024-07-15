using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PageKeywordsMustHaveAtLeastOneItemException : BaseException
    {
        public PageKeywordsMustHaveAtLeastOneItemException() : base(HttpStatusCode.BadRequest, "Page_Keywords_Must_Have_At_Least_One_Item")
        {
        }
    }
}
