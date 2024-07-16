using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class MobileIsDuplicateException : BaseException
    {
        public MobileIsDuplicateException() : base(System.Net.HttpStatusCode.Conflict, "Mobile_Is_Duplicate")
        {
        }
    }
}
