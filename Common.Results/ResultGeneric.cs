using System.Collections.Generic;

namespace Common.Result
{
    public class Result<T> : Result, IResult<T> where T : class
    {

        public T Data { get; set; }
        public Result(bool success,T data,int errorCode=0) : base(success,errorCode)
        {
            Data = data;
            ErrorCode = errorCode;
        }
        public Result(bool success, T data,string message,int errorCode=0) : this(success,data,errorCode)
        {
            Messages = new List<string>() { message };
        }

    }
        
   
}
