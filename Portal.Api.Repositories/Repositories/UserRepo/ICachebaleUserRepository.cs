using Assette.Client;
using Framework.Common;

namespace Portal.Api.Repositories.Contracts
{
    public interface ICachebaleUserRepository : ICachebaleRepository<UserDto, UserToCreateDto, UserSimpleDto>
    {
        ResultObj<UserDto> Activate(string userCode, bool activate);
        ResultObj ForgotPassword(ForgotPasswordDto forgotPasswordModel);
        ResultObj<UserDto> Lock(string userCode, bool lockValue);
        ResultObj ResetPassword(ResetPasswordDto resetPasswordModel);
    }
}