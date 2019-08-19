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
    public class SearchAccountsAsync : BaseTest
    {
        public SearchAccountsAsync() : base()
        {

        }
        [TestMethod]
        public async Task SearchAccountsAsync_AnExistantAccountCode_ShouldReturnListWithOneAccountResult()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            var account2 = _generators.GenerateNewAccount();
            //Act
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1,account2 });
            var sieveModel = new SieveModel
            {
                //Filters = "IsActive==false",
                Filters = $"Code@={account2.Code}",
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };
            var resultGet = await _client.SearchAccountsAsync(sieveModel);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Success.ShouldBeTrue();
            resultGet.Data.Count().ShouldBe(1);
        }
        [TestMethod]
        public async Task SearchAccountsAsync_ListOfValidAccountCodes_ShouldReturnListOfAccounts()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            var account2 = _generators.GenerateNewAccount();
            //Act
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1, account2 });
            var sieveModel = new SieveModel
            {
                //Filters = "IsActive==false",
                Filters = $"Code=={account1.Code}|{account2.Code}",
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };
            var resultGet = await _client.SearchAccountsAsync(sieveModel);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Success.ShouldBeTrue();
            resultGet.Data.Count().ShouldBe(2);
        }
        [TestMethod]
        public async Task SearchAccountsAsync_InactiveAccount_ShouldReturnOneInactiveAccount()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            account1.IsActive = false;
            var account2 = _generators.GenerateNewAccount();
            //Act
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1, account2 });
            var sieveModel = new SieveModel
            {
                //Filters = "IsActive==false",
                Filters = $"Code=={account1.Code},IsActive==false",
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };
            var resultGet = await _client.SearchAccountsAsync(sieveModel);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Success.ShouldBeTrue();
            resultGet.Data.Count().ShouldBe(1);
        }
        [TestMethod]
        [DataRow("Code==Unknown",true,0,DisplayName ="Unkown should return no data")]
        
        public async Task SearchAccountsAsync_UnexistantAccounts_ShouldReturnEmptyList(string filter,bool success,int count)
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            account1.IsActive = false;
            var account2 = _generators.GenerateNewAccount();
            //Act
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1, account2 });
            var sieveModel = new SieveModel
            {
                Filters = filter,
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };
            var resultGet = await _client.SearchAccountsAsync(sieveModel);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Success.ShouldBe(success);
            resultGet.Data.Count().ShouldBe(count);
        }
        [TestMethod]
        public async Task SearchAccountsAsync_NoFilter_ShouldReturnAllItems()
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            account1.IsActive = false;
            var account2 = _generators.GenerateNewAccount();
            //Act
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1, account2 });
            var sieveModel = new SieveModel
            {
                Filters = "",
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };
            var resultGet = await _client.SearchAccountsAsync(sieveModel);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            //resultGet.Success.ShouldBe(success);
            //resultGet.Data.Count().ShouldBe(count);
        }
        [TestMethod]
        [DataRow("OpenDate<2000-01-01", "2000-01-01")]
        [DataRow("OpenDate<2001-01-01", "2001-01-01")]
        [DataRow("OpenDate<2002-01-01", "2002-01-01")]

        public async Task SearchAccountsAsync_ValidOpenDate_ShouldReturnCorrectList(string filter,string beforeDate)
        {
            //Arrange
            var account1 = _generators.GenerateNewAccount();
            account1.OpenDate = Convert.ToDateTime(beforeDate);
            //Act
            var resultCreation = await _client.CreateOrUpdateAccountsAsync(new List<AccountToCreateDto> { account1});
            var sieveModel = new SieveModel
            {
                Filters = $"{filter}",
                Sorts = "Code",
                Page = 1,
                PageSize = 10000

            };
            var resultGet = await _client.SearchAccountsAsync(sieveModel);
            //Search to validate against getAll and validate locally
            var all = GetAllAccounts().Result.Data;
            var count= all.Count(x => x.OpenDate < Convert.ToDateTime(beforeDate));
            
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Success.ShouldBeTrue();
            resultGet.Data.Count().ShouldBe(count,$"Expected Count:{count},Actual:{resultGet.Data.Count()}");
        }

        private async Task<AccountDtoListResult> GetAllAccounts()
        {
            var sieveModel = new SieveModel
            {
                Filters = "",
                Sorts = "Code",
                Page = 1,
                PageSize = 10000

            };
            return await _client.SearchAccountsAsync(sieveModel);
        }
    }
}