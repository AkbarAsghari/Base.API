namespace API.Shared.Exceptions
{
    public class DataBasehandledException : BaseException
    {
        public DataBasehandledException(string message) : base(System.Net.HttpStatusCode.BadRequest, message, String.Empty)
        {
        }
    }
}
