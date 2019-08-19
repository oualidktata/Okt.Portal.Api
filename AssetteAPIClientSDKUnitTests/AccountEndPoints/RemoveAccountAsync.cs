using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
//using AccountToCreateDto = API.SDK.Contracts.AccountToCreateDto;
//using AccountDtoIResult = API.SDK.Contracts.AccountDtoIResult;
using Portal.SDK.Test;
using System;
using Assette.Client;
using System.Collections.Generic;
using Shouldly;
using System.Linq;

namespace Accounts
{
    [TestCategory("RemoveAccountAsync")]
    [TestClass]
    public class RemoveAccountAsync : BaseTest
    {
        public RemoveAccountAsync() : base()
        {

        }
        [TestMethod]
        public async Task RemoveAccountAsync_SoftDelete_ShouldSetActiveToFalse()
        {
            var account1 = _generators.GenerateNewAccount();
            var account2 = _generators.GenerateNewAccount();
            var account3 = _generators.GenerateNewAccount();//to not add but to Delete
            //Act
            await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1, account2 });

            var result = await _client.RemoveAccountsAsync(new List<string>{ account1.Code,account2.Code,account3.Code});
            
            result.Success.ShouldBeTrue();
            result.ShouldBeOfType<AccountSimpleDtoListResult>();
            result.Data.ToList().Count(x => x.Success).ShouldBe(2);
            result.Data.ToList().Count(x => !x.Success).ShouldBe(1);
        }

        [TestMethod]
        public async Task RemoveAccountAsync_UnknownAccount_ShouldNotFoundAPIException()
        {
           // var resultCreation = await Assert.ThrowsExceptionAsync<ApiException>(() => _client.RemoveAccountAsync("ABC111eww3344222121111"));
        }

        [TestMethod]
        [TestCategory("Exception")]
        public async Task RemoveAccountAsync_EmptyAccountCode_ShouldReturnArgumentNullException()
        {
          //  var resultCreation = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _client.RemoveAccountAsync(null));
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        [DataTestMethod]
        [TestCategory("Exception")]
        [DataRow("")]
        public async Task RemoveAccountAsync_EmptyAccountCode_ShouldReturnUnsupportedApiVersionException(string invalidAccountCode)
        {
            //var resultCreation = await Assert.ThrowsExceptionAsync<ApiException>(() => _client.RemoveAccountAsync(invalidAccountCode));
           // Assert.AreEqual(resultCreation.StatusCode,405);
        }
    }
}