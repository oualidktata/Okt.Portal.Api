using System.Collections.Generic;

namespace Common.Results
{
    public interface IResult
    {
        bool Success { get; set; }
        int ErrorCode { get; set; }
        List<string> Messages { get; set; }
        IResult AddMessage(string message);
        IResult AddMultipleMessage(IList<string> messages);
    }
}