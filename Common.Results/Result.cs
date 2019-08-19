using Common.Results;
using System.Collections.Generic;

namespace Common.Result
{
    public class Result : IResult
    {
        public Result(bool success,int errorCode=0)
        {
            Success = success;
            ErrorCode = errorCode;
            Messages = new List<string>();
        }

        public Result(bool success, string message, int errorCode = 0)
        {
            Success = success;
            ErrorCode = errorCode;
            Messages = new List<string>();
            AddMessage(message);
        }

        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public int ErrorCode { get; set; }

        public IResult AddMessage(string message)
        {
            (Messages as List<string>).Add(message);
            return this;
        }
        public IResult AddMultipleMessage(IList<string> messages)
        {
            Messages.AddRange(messages);
            return this;
        }
    }
}