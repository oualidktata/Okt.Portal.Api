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
    public class GetUserAsync : BaseTest
    {
        public GetUserAsync() : base()
        {

        }
        [TestMethod]
        public async Task GetUserAsync_AnExistantUserCode_ShouldReturnUserResult()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            //Act
            var resultCreation = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1 });
            var resultGet = await _client.GetUserAsync(user1.UserCode);
            //Assert
            resultGet.Data.ShouldNotBeNull();
            resultGet.Data.UserCode.ShouldBe(user1.UserCode);
        }
        [TestMethod]
        public async Task GetUserAsync_NotExistantUserCode_ShouldReturnException404NotFound()
        {
            try
            {
                await _client.GetUserAsync("FictionalUserCode");
            }
            catch (System.Exception ex)
            {
                ex.ShouldBeOfType<ApiException>();
                ex.Message.ShouldContain("404");
            }
        }
        [TestMethod]
        public async Task GetUserAsync_NullUserCode_ShouldReturnException404NotFound()
        {
            try
            {
                await _client.GetUserAsync(null);
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<ArgumentNullException>();
            }
        }
    }
}