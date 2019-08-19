using System;
using System.Linq;
using Assette.Client;
using AutoMapper;
using Framework.Common;
using Microsoft.Extensions.Caching.Memory;
using Portal.Api.Repositories.Contracts;
using Sieve.Services;

namespace Portal.Api.Repositories.Repos
{
    public class InMemoryUserRepository : CachebaleRepository<UserDto, UserToCreateDto, UserSimpleDto>, ICachebaleUserRepository
    {
        public InMemoryUserRepository(IMapper mapper, IMemoryCache cache, IUserRepoOptions repoOptions, ISieveProcessor sieveProcessor) : base(mapper, cache, repoOptions,sieveProcessor)
        {
        }
        public T GetPropertyValue<T>(object obj, string propName) {
            return (T)obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
        #region IUserRepository methods
        public ResultObj ForgotPassword(ForgotPasswordDto forgotPasswordModel)
        {
            throw new NotImplementedException();
        }

        public ResultObj<UserDto> Lock(string userCode, bool lockValue)
        {
            throw new NotImplementedException();
        }

        public ResultObj ResetPassword(ResetPasswordDto resetPasswordModel)
        {
            throw new NotImplementedException();
        }

        public ResultObj<UserDto> Activate(string userCode, bool activate)
        {
            //activate or deactivate by updating IsActive flag to true or false
            var result = FindByKey(u => u.UserCode == userCode);
            if (!result.Success)
            {
                return result;
            }

            result.Data.IsActive = activate;
            var user = ListOfItems.FirstOrDefault(u => u.UserCode == userCode);
            user.IsActive = activate;
            return new ResultBuilder<UserDto>().Success(user).Build();
        }
        #endregion
    }
}
