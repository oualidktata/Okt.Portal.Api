using Common.Results;

namespace Common.Result
{
    public interface IResult<T> : IResult where T : class
    {
        T Data { get; set; }
    }
}