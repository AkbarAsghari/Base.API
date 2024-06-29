namespace API.Shared.Exceptions
{
    public class DataBaseUnhandledException : BaseException
    {
        public DataBaseUnhandledException(string message) : base(System.Net.HttpStatusCode.InternalServerError, $"DataBase_Unhandled_Exception: {message}" , "خطایی از سمت پایگاه داده رخ داده است")
        {
        }
    }
}
