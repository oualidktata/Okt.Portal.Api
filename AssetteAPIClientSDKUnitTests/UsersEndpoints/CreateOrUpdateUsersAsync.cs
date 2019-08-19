using Assette.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Portal.SDK.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Users
{
    [TestCategory("CreateOrUpdateUsersAsync")]
    [TestClass]
    public class CreateOrUpdateUsersAsync : BaseTest, IDisposable
    {
        public CreateOrUpdateUsersAsync():base()
        {
           
        }
        void IDisposable.Dispose()
        {
            //Clean up
        }

        #region User Creation

        [TestMethod]
        public async Task CreateOrUpdateUsersAsync_ThreeValidUsers_ShouldReturnArrayOfSuccessfullResults()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            var user2 = _generators.GenerateNewUser();
            var user3 = _generators.GenerateNewUser();
            //Act
           var result = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1, user2, user3 });
            //Assert
            result.ShouldBeOfType<UserSimpleDtoListResult>();
            result.Data.Count.ShouldBe(3);
            result.Data.Count(x => x.Success).ShouldBe(3);//All success
        }

        [TestMethod]
        public async Task CreateOrUpdateUsersAsync_InvalidUsers_ShouldReturn400BadRequest()
        {
            //TODO Assette: ModelState validation to be implemented on API (constraints should be defined on DTO).
            //Should return Bad request(Invalid field lengths etc)
            //Arrange
            var user1 = _generators.GenerateNewUser();
            user1.Email = "InvalidEmailAdress";
            user1.UserCode = "More than 11 characters is not allowed!";
            var user2 = _generators.GenerateNewUser();
            var user3 = _generators.GenerateNewUser();
            //Act
            try
            {
                var result = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1, user2, user3 });

            }
            catch (Exception ex)
            {
                ex.ShouldNotBeOfType<ApiException>();
            }
            //var ex=await Assert.ThrowsAsync<ApiException>(()=> _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto>{ user1, user2 }));
            //Assert
            //Assert.IsInstanceOfType(result, typeof(ICollection<UserDtoIResult>));
            //Assert.AreEqual(3, result.Count);
            //Assert.IsTrue((result).Where(x => x.Success).Count() == 2);//All success
        }
        [TestMethod]
        public async Task CreateOrUpdateUsersAsync_DuplicateUsers_ShouldResultOnSimpleUpdate()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            var user2 = user1;
            var user3 = _generators.GenerateNewUser();
            //Act
           
                var result = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1, user2, user3 });
            //Assert
                result.ShouldBeOfType<UserSimpleDtoListResult>();
                result.Data.Count.ShouldBe(3);//two new and One Update
                result.Data.Count(x => x.Success).ShouldBe(3);//2 successes 
           }
        [TestMethod]
        public async Task CreateOrUpdateUsersAsync_UpdateIsActive_ShouldReturnArrayOfSuccessfullResults()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            var resultCreation = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1});
            var resultGetAfterCreation = await _client.GetUserAsync(user1.UserCode);
            //Act
            user1.IsActive = !user1.IsActive;
            var resultUpdate = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1 });
            var resultGetAfterUpdate = await _client.GetUserAsync(user1.UserCode);
            //Assert
            resultUpdate.ShouldBeOfType<UserSimpleDtoListResult>();
            resultCreation.ShouldBeOfType<UserSimpleDtoListResult>();
            resultGetAfterUpdate.Data.UserCode.ShouldBe(resultGetAfterCreation.Data.UserCode);
        }
        [TestMethod]
        public async Task CreateOrUpdateUsersAsync_UpdateMultipleUsers_ShouldReturnArrayOfSuccessfullResults()
        {
            //Arrange
            var user1 = _generators.GenerateNewUser();
            var user2 = _generators.GenerateNewUser();
            var resultCreation = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1,user2 });
            var resultGetAfterCreation1 = await _client.GetUserAsync(user1.UserCode);
            var resultGetAfterCreation2 = await _client.GetUserAsync(user2.UserCode);
            //Act
            user1.IsActive = !user1.IsActive;
            user2.IsActive = !user2.IsActive;
            var resultUpdate1 = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1 });
            var resultUpdate2 = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user2 });
            var resultGetAfterUpdateuser1 = await _client.GetUserAsync(user1.UserCode);
            var resultGetAfterUpdateuser2 = await _client.GetUserAsync(user2.UserCode);
            //Assert
            resultUpdate1.ShouldBeOfType<UserSimpleDtoListResult>();
            resultCreation.ShouldBeOfType<UserSimpleDtoListResult>();
            resultGetAfterUpdateuser1.Data.UserCode.ShouldBe(resultGetAfterCreation1.Data.UserCode);
            resultGetAfterUpdateuser2.Data.UserCode.ShouldBe(resultGetAfterCreation2.Data.UserCode);
        }


        #endregion

    }
}
