using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class PaymentAmountMustBeGearThan5000Exception : BaseException
    {
        public PaymentAmountMustBeGearThan5000Exception() :
            base(HttpStatusCode.BadRequest, "Payment_Amount_Must_Be_Gear_Than_5000")
        {
        }
    }
}
