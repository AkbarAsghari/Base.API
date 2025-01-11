using API.Shared.Enums;
using System.Net;

namespace API.Shared.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(string exception) : base(
            HttpStatusCode.BadRequest,
            "ValidationException",
            new Dictionary<ExceptionLanguageEnum, object?>
            {
                { ExceptionLanguageEnum.Fa , exception }
            })
        {
        }
    }
}
