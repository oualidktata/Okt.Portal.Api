using api.sdk;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;

namespace Portal.SDK.Test
{
    public class DocumentCreateBulkTests:IDisposable
    {
        private Client _client;
        public DocumentCreateBulkTests()
        {
            var util = new Common();
            _client = util.CreateSDKClient("http://localhost:5000");
        }
        void IDisposable.Dispose()
        {
            //Clean up
        }

        #region CreateBulkDocumentsAsync

        [Fact]
        [Trait("Category", "Bulk")]
        public async Task CreateBulkDocumentsAsync_TwoValidDocument_ShouldReturnArrayOfSuccessfullResults()
        {
            //Arrange
            var Document1 = Generators.GenerateNewDocument();
            var Document2 = Generators.GenerateNewDocument();
            var Document3 = Generators.GenerateNewDocument();
            //Act
            var result = await _client.CreateBulkDocumentsAsync(new[] { Document1, Document2,Document3 });
            //Assert
            Assert.IsAssignableFrom<ICollection<DocumentSimpleDtoIResult>>(result);
            Assert.Equal(3,result.Count);
            Assert.True((result).Where(x=>x.Success).Count()==3);//All success
        }

        [Fact]
        [Trait("Category", "Bulk")]
        public async Task CreateBulkDocumentsAsync_InvalidDocuments_ShouldReturn400BadRequest()
        {
            //Arrange
            var Document1 = Generators.GenerateNewDocument();
            Document1.Email = "InvalidEmailAdress";
            Document1.DocumentKey = "More than 11 characters is not allowed!";
            var Document2 = Generators.GenerateNewDocument();
            //Act
            var ex = await Assert.ThrowsAsync<ApiException>(() => _client.CreateBulkDocumentsAsync(new[] { Document1, Document2 }));
            //Assert

            Assert.Equal(400, ex.StatusCode);
            Assert.NotEmpty(ex.Response);
        }
        [Fact]
        [Trait("Category", "Bulk")]
        public async Task CreateBulkDocumentsAsync_DuplicateDocuments_ShouldReturnArrayOfSuccessfulAndFaildedResults()
        {
            //Arrange
            var Document1 = Generators.GenerateNewDocument();
            var Document2 = Document1;
            //Act
            var ex = await Assert.ThrowsAsync<ApiException<ICollection<DocumentSimpleDtoIResult>>>(() => _client.CreateBulkDocumentsAsync(new[] { Document1, Document2 }));
            //Assert

            //Assert.IsType<ApiException<List<DocumentSimpleDtoIResult>>>>(ex.Result);
            Assert.Equal(409,ex.StatusCode);
            //Assert.IsAssignableFrom<IList<DocumentSimpleDtoIResult>>(result);
            //Assert.Equal(2, result.Count);
          //  Assert.True((ex.Result<DocumentSimpleDtoIResult>.Where(x => x.Success).Count() == 1);//One success
          //  Assert.True(((List<DocumentSimpleDtoIResult>)result).Where(x => !x.Success).Count() == 1);//One failure
        }
        #endregion

    }
}
