using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class EmailIsDuplicateException : BaseException
    {
        public EmailIsDuplicateException() : base(System.Net.HttpStatusCode.Conflict, "Email_Is_Duplicate")
        {
        }
    }
}
