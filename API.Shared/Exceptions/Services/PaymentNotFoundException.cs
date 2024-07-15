using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PaymentNotFoundException : BaseException
    {
        public PaymentNotFoundException()
            : base(HttpStatusCode.NotFound, "Payment_Not_Found")
        {
        }
    }
}
