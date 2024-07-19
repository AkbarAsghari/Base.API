using API.Shared.Enums;
using System.Net;

namespace API.Shared.Exceptions
{
    public abstract class BaseException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; private set; }
        public Dictionary<string, object?> Extensions { get; private set; }

        protected BaseException(HttpStatusCode httpStatusCode, string key, Dictionary<ExceptionLanguageEnum, object?>? Extensions = null)
            : base(key, null)
        {
            HttpStatusCode = httpStatusCode;
            this.Extensions = new Dictionary<string, object?>
            {
                { "Messages" , Extensions }
            };
        }
    }
}
