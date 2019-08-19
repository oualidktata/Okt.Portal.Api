using Assette.Client;
using Framework.Common;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Portal.Api.Filters
{
    public class SchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            foreach (var item in context.SchemaRepository.Schemas)
            {
                if (item.Key=="DocumentTypeDtoIEnumerableIResult")
                {
                    item.Value.Title = "ListOfDocumentTypeResults";
                    item.Value.Description = "List of Results of Type DocumentTypeDto. Every result may contain messages.";

                    var res1 =new ResultBuilder<DocumentTypeDto>().Success().SetData(new DocumentTypeDto { DocTypeCode = "Type1", Category = new CategoryDto() }).Build();
                    var res2 = new ResultBuilder<DocumentTypeDto>().Success().SetData(new DocumentTypeDto { DocTypeCode = "Type2", Category = new CategoryDto() }).Build();

                    var list = new List<ResultObj<DocumentTypeDto>>() { res1, res2 };
                    //schemaToUpdate.Example = new OpenApiExample() { ExternalValue = "" } as IOpenApiAny;
                    //new List<ResultObj<DocumentTypeDto>>() { new Result<DocumentTypeDto>(true, new DocumentTypeDto { DocTypeCode = "Type1", Category = new CategoryDto() }), new Result<DocumentTypeDto>(true, new DocumentTypeDto { DocTypeCode = "Type2", Category = new CategoryDto() }) } as IOpenApiAny;
                }
                if (item.Key == "AccountDtoIEnumerableIResult")
                {
                    item.Value.Title = "ListOfAccountResults";
                    item.Value.Description = "List of Results of AccountDtos. Every result may contain messages.";
                }
                if (item.Key == "PermissionDtoIEnumerableIResult")
                {
                    item.Value.Title = "ListOfPermissionResults";
                    item.Value.Description = "List of Results of Permissions. Every result may contain messages.";
                }
                if (item.Key == "CategoryDtoIEnumerableIResult")
                {
                    item.Value.Title = "ListOfCategoryResults";
                    item.Value.Description = "List of Results of Categories. Every result may contain messages.";
                }
                if (item.Key == "DocumentFileDtoIEnumerableIResult")
                {
                    item.Value.Title = "ListOfCDocumentFileResults";
                    item.Value.Description = "List of Results of Document Files. Every result may contain messages.";
                }
                    if (item.Key == "UserSimpleDtoIResult")
                {
                    item.Value.Title = "ListOfSimpleUserDtoResults";
                    item.Value.Description = "List of Results of SimpleUserDto Files. Every result may contain messages.";
                }
            }
        }
    }
}
