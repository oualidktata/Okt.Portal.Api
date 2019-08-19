using Assette.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Portal.SDK.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Accounts
{
    [TestCategory("CreateOrUpdateAccountsAsync")]
    [TestClass]
    public class CreateOrUpdateAccountsAsync : BaseTest, IDisposable
    {
        public CreateOrUpdateAccountsAsync():base()
        {
           
        }
        void IDisposable.Dispose()
        {
            //Clean up
        }

        #region Account Creation

        [TestMethod]
        public async Task CreateOrUpdateAccountsAsync_ThreeValidAccounts_ShouldReturnArrayOfSuccessfullResults()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            var account2 = _generators.GenerateNewAccount();
            var account3 = _generators.GenerateNewAccount();
            //Act
           var result = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1, account2, account3 });
            //Assert
            result.ShouldBeOfType<AccountSimpleDtoListResult>();
            result.Data.Count.ShouldBe(3);
            result.Data.Count(x => x.Success).ShouldBe(3);//All success
        }

        [TestMethod]
        public async Task CreateOrUpdateAccountsAsync_InvalidAccounts_ShouldReturn400BadRequest()
        {
            //TODO Assette: ModelState validation to be implemented on API (constraints should be defined on DTO).
            //Should return Bad request(Invalid field lengths etc)
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            account1.Code = "InvalidAccountWithMoreThan10Caracters";//Validation rules for all 
            
            var account2 = _generators.GenerateNewAccount();
            var account3 = _generators.GenerateNewAccount();
            //Act
            //var ex=await Assert.ThrowsAsync<ApiException>(()=> _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto>{ account1, account2 }));
            var result = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1, account2,account3 });
            //Assert
            //Assert.IsInstanceOfType(result, typeof(ICollection<AccountDtoIResult>));
            //Assert.AreEqual(3, result.Count);
            //Assert.IsTrue((result).Where(x => x.Success).Count() == 2);//All success
        }
        [TestMethod]
        public async Task CreateOrUpdateAccountsAsync_DuplicateAccounts_ShouldResultOnSimpleUpdate()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            var account2 = account1;
            var account3 = _generators.GenerateNewAccount();
            //Act
           
                var result = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1, account2, account3 });
            //Assert
                result.ShouldBeOfType<AccountSimpleDtoListResult>();
                result.Data.Count.ShouldBe(3);//two new and One Update
                result.Data.Count(x => x.Success).ShouldBe(3);//2 successes 
           }
        [TestMethod]
        public async Task CreateOrUpdateAccountsAsync_UpdateIsActive_ShouldReturnArrayOfSuccessfullResults()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1});
            var resultGetAfterCreation = await _client.GetAccountAsync(account1.Code);
            //Act
            account1.IsActive = !account1.IsActive;
            var resultUpdate = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1 });
            var resultGetAfterUpdate = await _client.GetAccountAsync(account1.Code);
            //Assert
            resultUpdate.ShouldBeOfType<AccountSimpleDtoListResult>();
            resultCreation.ShouldBeOfType<AccountSimpleDtoListResult>();
            resultGetAfterUpdate.Data.IsActive.ShouldBe(!resultGetAfterCreation.Data.IsActive);
            resultGetAfterUpdate.Data.Code.ShouldBe(resultGetAfterCreation.Data.Code);
        }
        [TestMethod]
        public async Task CreateOrUpdateAccountsAsync_UpdateMultipleAccounts_ShouldReturnArrayOfSuccessfullResults()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            var account2 = _generators.GenerateNewAccount();
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1,account2 });
            var resultGetAfterCreation1 = await _client.GetAccountAsync(account1.Code);
            var resultGetAfterCreation2 = await _client.GetAccountAsync(account2.Code);
            //Act
            account1.IsActive = !account1.IsActive;
            account2.IsActive = !account2.IsActive;
            var resultUpdate1 = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1 });
            var resultUpdate2 = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account2 });
            var resultGetAfterUpdateAccount1 = await _client.GetAccountAsync(account1.Code);
            var resultGetAfterUpdateAccount2 = await _client.GetAccountAsync(account2.Code);
            //Assert
            resultUpdate1.ShouldBeOfType<AccountSimpleDtoListResult>();
            resultCreation.ShouldBeOfType<AccountSimpleDtoListResult>();
            resultGetAfterUpdateAccount1.Data.IsActive.ShouldBe(!resultGetAfterCreation1.Data.IsActive);
            resultGetAfterUpdateAccount1.Data.Code.ShouldBe(resultGetAfterCreation1.Data.Code);
            resultGetAfterUpdateAccount2.Data.IsActive.ShouldBe(!resultGetAfterCreation2.Data.IsActive);
            resultGetAfterUpdateAccount2.Data.Code.ShouldBe(resultGetAfterCreation2.Data.Code);
        }


        #endregion

    }
}
