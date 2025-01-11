using API.Shared.Enums;
using System.Net;

namespace API.Shared.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string exception) : base(
            HttpStatusCode.NotFound,
            "NotFoundException",
            new Dictionary<ExceptionLanguageEnum, object?>
            {
                { ExceptionLanguageEnum.Fa , exception }
            })
        {
        }
    }
}
