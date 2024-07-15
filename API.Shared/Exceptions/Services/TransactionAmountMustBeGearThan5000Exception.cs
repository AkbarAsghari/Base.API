using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class TransactionAmountMustBeGearThan5000Exception : BaseException
    {
        public TransactionAmountMustBeGearThan5000Exception() :
            base(HttpStatusCode.BadRequest, "Transaction_Amount_Must_Be_Gear_Than_5000")
        {
        }
    }
}
