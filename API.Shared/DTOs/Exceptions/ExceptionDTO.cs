using System.Net;

namespace API.Shared.DTOs.BasicData
{
    public class ExceptionDTO
    {
        public string Key { get; set; }
        public string PersianMessage { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
