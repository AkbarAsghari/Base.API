using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class TransactionNotFoundException : BaseException
    {
        public TransactionNotFoundException()
            : base(HttpStatusCode.NotFound, "Transaction_Not_Found")
        {
        }
    }
}
