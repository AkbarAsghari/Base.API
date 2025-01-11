using API.Shared.Enums;
using System.Net;

namespace API.Shared.Exceptions
{
    public class DuplicateException : BaseException
    {
        public DuplicateException(string exception) : base(
            HttpStatusCode.Conflict,
            "DuplicateException",
            new Dictionary<ExceptionLanguageEnum, object?>
            {
                { ExceptionLanguageEnum.Fa , exception }
            })
        {
        }
    }
}
