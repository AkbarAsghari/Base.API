using API.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class ZibalExceptionException : BaseException
    {
        public ZibalExceptionException(ZibalResultEnum zibalResult) :
            base(HttpStatusCode.ServiceUnavailable, $"Paymanet_Exception", zibalResult.ToString())
        {
        }
    }
}
