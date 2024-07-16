using API.Shared.DTOs.BasicData;
using System.Net;

namespace API.Shared.Exceptions
{
    public abstract class BaseException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; private set; }
        string _PersianMessage;

        protected BaseException(HttpStatusCode httpStatusCode, string key, string persianMessage = null)
            : base(key, null)
        {
            _PersianMessage = persianMessage;
            HttpStatusCode = httpStatusCode;
        }

        public ExceptionDTO GenerateResponse()
        {
            return new ExceptionDTO
            {
                PersianMessage = _PersianMessage,
                Key = Message,
                HttpStatusCode = HttpStatusCode
            };
        }
    }
}
