using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class OldPasswordIsWrongException : BaseException
    {
        public OldPasswordIsWrongException() : base(System.Net.HttpStatusCode.Forbidden, "Old_Password_Is_Wrong") { }
    }
}
