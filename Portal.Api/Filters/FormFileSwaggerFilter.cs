using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Api.Filters
{
    /// <summary>
    /// Filter to enable handling file upload in swagger
    /// Graciosité : https://alexdunn.org/2018/07/12/adding-a-file-upload-field-to-your-swagger-ui-with-swashbuckle/
    /// </summary>
    public class FileUploadOperation : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo == null) return;



            if (operation.OperationId?.ToLower() == "UploadDocument".ToLower())
            {
                var props = new Dictionary<string, OpenApiSchema>();
                var schema = new OpenApiSchema() { Type = "string", Format = "binary" };
                props.Add("fileName",
                    new OpenApiSchema
                    {
                        Type = "file",
                        Items = schema
                    });

                #region sample
                //var sample = new OpenApiExample()
                //{
                //    //Reference = new OpenApiReference
                //    //{
                //    //    Type = ReferenceType.Example,
                //    //    Id = "example1",
                //    //},
                //    Value = new OpenApiObject
                //    {
                //        ["versions"] = new OpenApiArray
                //        {
                //            new OpenApiObject
                //            {
                //                ["status"] = new OpenApiString("Status1"),
                //                ["id"] = new OpenApiString("v1"),
                //                //["links"] = new OpenApiArray
                //                //{
                //                //    new OpenApiObject
                //                //    {
                //                //        ["href"] = new OpenApiString("http://example.com/1"),
                //                //        ["rel"] = new OpenApiString("sampleRel1")
                //                //    }
                //                //}
                //            },

                //            new OpenApiObject
                //            {
                //                ["status"] = new OpenApiString("Status2"),
                //                ["id"] = new OpenApiString("v2"),
                //                //["links"] = new OpenApiArray
                //                //{
                //                //    new OpenApiObject
                //                //    {
                //                //        ["href"] = new OpenApiString("http://example.com/2"),
                //                //        ["rel"] = new OpenApiString("sampleRel2")
                //                //    }
                //                //}
                //            }
                //        }
                //    }
                //};
                //var samples = new Dictionary<string, OpenApiExample>();
                //samples.Add("sample1", sample);
                #endregion

                //var uploadPro= new KeyValuePair<string,>
                operation.RequestBody.Content.Clear();
                operation.RequestBody.Content.Add("application/octet-stream", new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format="binary",
                        Properties=props
                        
                        //Example= new Open() { Summary= @"Something as example for application/octet-stream (Type:String and Format:Binary)" }
                        //Properties = props
                        //Type = "object",
                        //Properties = props
                    },
                    //Examples = samples
                });

                //var actionAttributes = context.MethodInfo.GetCustomAttributes(true);
                //var controllerAttributes = context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true);
                //var actionAndControllerAttributes = actionAttributes.Union(controllerAttributes);
                //ApplySwaggerOperationAttribute(operation, actionAttributes);
                //ApplySwaggerOperationFilterAttributes(operation, context, actionAndControllerAttributes);
                //ApplySwaggerResponseAttributes(operation, actionAndControllerAttributes, context);

                //operation.Parameters.Add(new OpenApiParameter
                //{
                //    Name = "uploadedFile",
                //   // Content = new Dictionary<string, OpenApiMediaType>() { }
                //    In = ParameterLocation.Query,
                //    Description = "Upload File",
                //    Required = true,
                //    Ty
                //    Type = "file"
                //});

            }
          



            //if (operation.OperationId.ToLower() == "apivaluesuploadpost")
            //{
            //    operation.Parameters.Clear();
            //    operation.Parameters.Add(new NonBodyParameter
            //    {
            //        Name = "uploadedFile",
            //        In = "formData",
            //        Description = "Upload File",
            //        Required = true,
            //        Type = "file"
            //    });
            //    operation.Consumes.Add("multipart/form-data");
            //}


        }
        private static void ApplySwaggerOperationAttribute(
           OpenApiOperation operation,
           IEnumerable<object> actionAttributes)
        {
            var swaggerOperationAttribute = actionAttributes
                .OfType<SwaggerOperationAttribute>()
                .FirstOrDefault();

            if (swaggerOperationAttribute == null) return;

            if (swaggerOperationAttribute.Summary != null)
                operation.Summary = swaggerOperationAttribute.Summary;

            if (swaggerOperationAttribute.Description != null)
                operation.Description = swaggerOperationAttribute.Description;

            if (swaggerOperationAttribute.OperationId != null)
                operation.OperationId = swaggerOperationAttribute.OperationId;

            if (swaggerOperationAttribute.Tags != null)
            {
                operation.Tags = swaggerOperationAttribute.Tags
                    .Select(tagName => new OpenApiTag { Name = tagName })
                    .ToList();
            }
        }

        public static void ApplySwaggerOperationFilterAttributes(
            OpenApiOperation operation,
            OperationFilterContext context,
            IEnumerable<object> actionAndControllerAttributes)
        {
            var swaggerOperationFilterAttributes = actionAndControllerAttributes
                .OfType<SwaggerOperationFilterAttribute>();

            foreach (var swaggerOperationFilterAttribute in swaggerOperationFilterAttributes)
            {
                var filter = (Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter)Activator.CreateInstance(swaggerOperationFilterAttribute.FilterType);
                filter.Apply(operation, context);
            }
        }

        private void ApplySwaggerResponseAttributes(
            OpenApiOperation operation,
            IEnumerable<object> actionAndControllerAttributes,
            OperationFilterContext context)
        {
            var swaggerResponseAttributes = actionAndControllerAttributes
                .OfType<SwaggerResponseAttribute>();

            foreach (var swaggerResponseAttribute in swaggerResponseAttributes)
            {
                var statusCode = swaggerResponseAttribute.StatusCode.ToString();
                if (!operation.Responses.TryGetValue(statusCode, out OpenApiResponse response))
                {
                    response = new OpenApiResponse();
                }

                if (swaggerResponseAttribute.Description != null)
                    response.Description = swaggerResponseAttribute.Description;

                operation.Responses[statusCode] = response;
            }
        }
    }

    //    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    //    {

    //        if (context.MethodInfo == null) return;



    //        if (operation.OperationId?.ToLower() == "UploadDocument".ToLower())
    //        {


    //            var props = new Dictionary<string, OpenApiSchema>();

    //            props.Add("fileName",
    //                new OpenApiSchema
    //                {
    //                    Type = "object",
    //                    Items = { Type = "string", Format = "binary", }

    //                });

    //            //var uploadPro= new KeyValuePair<string,>
    //            operation.RequestBody.Content.Add("formData", new OpenApiMediaType()
    //            {
    //                Schema = new OpenApiSchema
    //                {
    //                    Type = "object",
    //                    Properties = props
    //                }
    //            });
    //        }
    //}
}
