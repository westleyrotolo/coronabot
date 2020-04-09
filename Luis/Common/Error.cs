using System;
namespace CovidBot.Luis.Common
{
    public class Error
    {
        public static string[] ERROR_LIST = new string[] {
            "Unspecified", "Unauthorized", "BadRequest", "TooManyRequests", "Forbidden"
        };

        public const string UNEXPECTED_ERROR_CODE = "Unexpected";

        public string Code { get; set; }
        public string Message { get; set; }
    }
}
