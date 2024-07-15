using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class MobileIsNotValidException : BaseException
    {
        public MobileIsNotValidException() : base(System.Net.HttpStatusCode.BadRequest, "Mobile_Is_Not_Valid")
        {
        }
    }
}
