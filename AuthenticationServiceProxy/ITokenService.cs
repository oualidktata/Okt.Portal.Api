using System.Threading.Tasks;

namespace AuthenticationServiceProxy
{
    public interface ITokenService
    {
        Task<string> GetToken();
    }
}