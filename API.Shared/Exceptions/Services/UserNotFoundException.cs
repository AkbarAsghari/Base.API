using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class UserNotFoundException : BaseException
    {
        public UserNotFoundException() : base(System.Net.HttpStatusCode.NotFound, "User_Not_Found") { }
    }
}
