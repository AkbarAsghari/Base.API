using API.Shared.Enums;
using System.Net;

namespace API.Shared.Exceptions
{
    public class NotImplementedException : BaseException
    {
        public NotImplementedException(string exception) : base(
            HttpStatusCode.NotImplemented,
            "DuplicateException",
            new Dictionary<ExceptionLanguageEnum, object?>
            {
                { ExceptionLanguageEnum.Fa , exception }
            })
        {
        }
    }
}
