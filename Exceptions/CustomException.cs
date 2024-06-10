using System.Net;

namespace HomeBanking.Exceptions
{
    public class CustomException : Exception
    {
        public string message;
        public HttpStatusCode statusCode;

        public CustomException(string message, HttpStatusCode statusCode) : base(message)
        {
            {
                this.message = message;
                this.statusCode = statusCode;
            }
        }
    }
}
