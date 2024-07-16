using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class UsernameIsDuplicateException : BaseException
    {
        public UsernameIsDuplicateException() : base(System.Net.HttpStatusCode.Conflict, "Username_Is_Duplicate")
        {
        }
    }
}
