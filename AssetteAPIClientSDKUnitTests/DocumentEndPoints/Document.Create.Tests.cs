//using API.SDK.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace Portal.SDK.Test
//{
//    public class DocumentCreateTests:BaseTest, IDisposable
//    {


//        public DocumentCreateTests():base()
//        {
           
//        }

//        void IDisposable.Dispose()
//        {
//            //Clean up
//        }
//        #region CreateDocumentAsync
//        [Fact]
//        [Trait("Document","Create")]
//        public async Task CreateDocument_ValidDocument_ReturnResultWithCreatedDocument()
//        {
//            //Arrange
//            var document1 = _generators.GenerateNewDocument();
//            document1.AsOfDate = DateTime.Now;
//            var document2 = _generators.GenerateNewDocument();
//            document2.AsOfDate = DateTime.Now;
//            var documents = new DocumentDto[] {document1,document2 };
//            //Act
//            var result = await _client.CreateDocumentMetadataAsync(documents);

//            //Assert
//            Assert.IsAssignableFrom<ICollection<DocumentDto>>(result);
//            Assert.Equal(2, result.Count);
//            Assert.True((result).Where(x => x.Success).Count() == 2);//All success
//        }
  

//       [Fact]
//        [Trait("Document", "Create")]
//        public async Task CreateDocument_DuplicateDocument_ShouldReturn409Conflict()
//        {
//            //Arrange
//            var document1 = _generators.GenerateNewDocument();
//            document1.AsOfDate = DateTime.Now;
//            var documents = new DocumentDto[] { document1, document1 };
//            //Act

//            var ex = await Assert.ThrowsAsync<ApiException<ICollection<string>>>(() => _client.CreateDocumentMetadataAsync(documents));
//            //Assert
//             Assert.Equal(409,ex.StatusCode);
//            //Assert.False(ex.ResultSuccess);
//        }
       
//        [Fact]
//        [Trait("Document", "Create")]
//        public async Task CreateDocumentAsync_InvalidPayload_ShouldReturn400BadRequest()
//        {
//            //Arrange
//            var document1 = _generators.GenerateNewDocument();
//            document1.AsOfDate = DateTime.Now;
//            document1.DocumentTypeCode = "";//it's required
//            var document2 = _generators.GenerateNewDocument();
//            document2.AsOfDate = DateTime.Now;
//            var documents = new DocumentDto[] { document1, document2 };
//            //Act
//            var ex = await Assert.ThrowsAsync<ApiException>(() => _client.CreateDocumentMetadataAsync(documents));
//            //Assert
//            Assert.Equal(400, ex.StatusCode);
//            Assert.NotEmpty(ex.Response);
//        }
//        #endregion


//    }
//}
