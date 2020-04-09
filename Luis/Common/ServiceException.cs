using System;
namespace CovidBot.Luis.Common
{
    public class ServiceException
    {
        public Error Error { get; set; }
        public string Message { get; set; }

        public Exception ToException()
        {
            var errorMessage = ToString();
            return new Exception(errorMessage);
        }

        public override string ToString() =>
            $"{Error?.Code ?? Error.UNEXPECTED_ERROR_CODE} - {Error?.Message ?? Message}";
    }
}
