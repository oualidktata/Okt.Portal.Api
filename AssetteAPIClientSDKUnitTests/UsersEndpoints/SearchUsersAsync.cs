using Assette.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
//using UserToCreateDto = API.SDK.Contracts.UserToCreateDto;
//using ApiException = API.SDK.Contracts.ApiException;
//using UserDtoIResult = API.SDK.Contracts.UserDtoIResult;
using Portal.SDK.Test;
using System.Linq;
using Shouldly;
using System;

namespace Users
{
    [TestCategory("GetUserAsync")]
    [TestClass]
    public class SearchUsersAsync : BaseTest
    {
        public SearchUsersAsync() : base()
        {

        }
        [TestMethod]
        public async Task SearchUsersAsync_AnExistantUserCode_ShouldReturnListWithOneUserResult()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            var user2 = _generators.GenerateNewUser();
            //Act
            var resultCreation = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1,user2 });
            var sieveModel = new SieveModel
            {
                //Filters = "IsActive==false",
                Filters = $"UserCode@={user2.UserCode}",
                Sorts = "UserCode",
                Page = 1,
                PageSize = 10

            };
            var resultGet = await _client.SearchUsersAsync(sieveModel);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Success.ShouldBeTrue();
            resultGet.Data.Count().ShouldBe(1);
        }
        [TestMethod]
        public async Task SearchUsersAsync_ListOfValidUserCodes_ShouldReturnListOfUsers()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            var user2 = _generators.GenerateNewUser();
            //Act
            var task = _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1, user2 });
            var sieveModel = new SieveModel
            {
                //Filters = "IsActive==false",
                Filters = $"UserCode=={user1.UserCode}|{user2.UserCode}",
                Sorts = "UserCode",
                Page = 1,
                PageSize = 10

            };

            var resultGet = new UserDtoListResult();
            var continuation = task.ContinueWith(async x =>
            {
                resultGet = await _client.SearchUsersAsync(sieveModel);
                //Assert
                resultGet.Data.ShouldNotBeNull();
                resultGet.Success.ShouldBeTrue();
                resultGet.Data.Count().ShouldBe(2);
            });
        }
        [TestMethod]
        public async Task SearchUsersAsync_InactiveUser_ShouldReturnOneInactiveUser()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            user1.IsActive = false;
            var user2 = _generators.GenerateNewUser();
            //Act
            var task = _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1, user2 });
            var sieveModel = new SieveModel
            {
                //Filters = "IsActive==false",
                Filters = $"UserCode=={user1.UserCode},IsActive==false",
                Sorts = "UserCode",
                Page = 1,
                PageSize = 10

            };

            var resultGet = new UserDtoListResult();
            var continuation=task.ContinueWith(async x =>
            {
                resultGet = await _client.SearchUsersAsync(sieveModel);
                resultGet.Data.ShouldNotBeNull();
                resultGet.Success.ShouldBeTrue();
                resultGet.Data.Count().ShouldBe(1);
            });
            //continuation.Wait();
            

            //Assert


        }
        [TestMethod]
        [DataRow("UserCode==Unknown",true,0,DisplayName ="Unkown should return no data")]
        
        public async Task SearchUsersAsync_UnexistantUsers_ShouldReturnEmptyList(string filter,bool success,int count)
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            user1.IsActive = false;
            var user2 = _generators.GenerateNewUser();
            //Act
            var resultCreation = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1, user2 });
            var sieveModel = new SieveModel
            {
                Filters = filter,
                Sorts = "UserCode",
                Page = 1,
                PageSize = 10

            };
            var resultGet = await _client.SearchUsersAsync(sieveModel);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Success.ShouldBe(success);
            resultGet.Data.Count().ShouldBe(count);
        }
        [TestMethod]
        public async Task SearchUsersAsync_NoFilter_ShouldReturnAllItems()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            user1.IsActive = false;
            var user2 = _generators.GenerateNewUser();
            //Act
            var resultCreation = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1, user2 });
            var sieveModel = new SieveModel
            {
                Filters = "",
                Sorts = "UserCode",
                Page = 1,
                PageSize = 10

            };
            var resultGet = await _client.SearchUsersAsync(sieveModel);
            //Assert
            resultGet.Data.ShouldNotBeNull();
        }
    }
}