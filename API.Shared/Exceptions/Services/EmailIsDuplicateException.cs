using API.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class EmailIsDuplicateException : BaseException
    {
        public EmailIsDuplicateException() : base(System.Net.HttpStatusCode.Conflict,
                                                  "Email_Is_Duplicate",
                                                  new Dictionary<Enums.ExceptionLanguageEnum, object?>
                                                  { { ExceptionLanguageEnum.Fa, "آدرس ایمیل تکراری میباشد" } })
        {
        }
    }
}
