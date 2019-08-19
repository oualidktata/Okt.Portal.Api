using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
//using UserToCreateDto = API.SDK.Contracts.UserToCreateDto;
//using UserDtoIResult = API.SDK.Contracts.UserDtoIResult;
using Portal.SDK.Test;
using System;
using Assette.Client;
using System.Collections.Generic;
using Shouldly;

namespace Users
{
    [TestCategory("ActivateAsync")]
    [TestClass]
    public class ActivateAsync : BaseTest
    {
        public ActivateAsync() : base()
        {

        }
        [TestMethod]
        public async Task ActivateAsync_SoftDelete_ShouldSetActiveToFalse()
        {
            var user1 = _generators.GenerateNewUser();
            user1.IsActive = false;
            //Act
            var result = await _client.CreateOrUpdateUsersAsync(new List<UserToCreateDto> { user1 });

            var resultActivate = await _client.ActivateUserAsync(user1.UserCode);
            var resultGet = await _client.GetUserAsync(user1.UserCode);
            resultActivate.Success.ShouldBeTrue();
            resultGet.Data.IsActive.ShouldBeTrue();
        }
        [TestMethod]
        [TestCategory("Exception")]
        public async Task ActivateAsync_EmptyUserCode_ShouldReturnArgumentNullException()
        {
            var resultCreation = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _client.ActivateUserAsync(null));
        }

        [TestMethod]
        [DataTestMethod]
        [TestCategory("Exception")]
        [DataRow("Unknown", typeof(ApiException<string>), "409", DisplayName = "Unknown user code results on conflict: 409")]
        [DataRow("", typeof(ApiException), "400", DisplayName = "empty user code results on BadRequest: 400")]
        public async Task ActivateAsync_InvalidUserCodes_ShouldThrowApiVersionExceptions(string invalidUserCode, Type typeOfException, string errorStatusCode)
        {
            try
            {
                await _client.ActivateUserAsync(invalidUserCode);
            }
            catch (Exception ex)
            {

                ex.ShouldBeOfType(typeOfException);

                ex.Message.ShouldContain(errorStatusCode);
            }

        }
    }
}