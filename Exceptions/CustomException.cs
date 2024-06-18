using System.Net;

namespace HomeBanking.Exceptions
{
    public class CustomException : Exception
    {
        public string message;
        public int statusCode;

        public CustomException(string message, int statusCode) : base(message)
        {
            {
                this.message = message;
                this.statusCode = statusCode;
            }
        }
    }
}
