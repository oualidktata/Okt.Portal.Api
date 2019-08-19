using Assette.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
//using AccountToCreateDto = API.SDK.Contracts.AccountToCreateDto;
//using ApiException = API.SDK.Contracts.ApiException;
//using AccountDtoIResult = API.SDK.Contracts.AccountDtoIResult;
using Portal.SDK.Test;
using System.Linq;
using Shouldly;
using System;

namespace Accounts
{
    [TestCategory("GetAccountAsync")]
    [TestClass]
    public class GetAccountAsync : BaseTest
    {
        public GetAccountAsync() : base()
        {

        }
        [TestMethod]
        public async Task GetAccountAsync_AnExistantAccountCode_ShouldReturnAccountResult()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            //Act
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1 });
            var resultGet = await _client.GetAccountAsync(account1.Code);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Data.Code.ShouldBe(account1.Code);
        }
        [TestMethod]
        public async Task GetAccountAsync_NotExistantAccountCode_ShouldReturnException404NotFound()
        {
            try
            {
               await _client.GetAccountAsync("FictionalAccountCode");
            }
            catch (System.Exception ex)
            {
                ex.ShouldBeOfType<ApiException>();
                ex.Message.ShouldContain("404");
            }
        }
        [TestMethod]
        public async Task GetAccountAsync_NullAccountCode_ShouldReturnException404NotFound()
        {
            try
            {
                await _client.GetAccountAsync(null);
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<ArgumentNullException>();
            }
        }
    }
}