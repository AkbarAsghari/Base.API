namespace API.Shared.Exceptions
{
    public class DataBaseExceptionNormalizer
    {

        static Dictionary<string, string> DataBaseNormalExceptions = new Dictionary<string, string>
        {

        };

        public static string Check(Exception exception)
        {
            var existException = DataBaseNormalExceptions.FirstOrDefault(x => exception.Message.Contains(x.Key) || exception.Message.Contains(x.Key));
            return existException.Value ?? exception.Message;
        }


    }
}
