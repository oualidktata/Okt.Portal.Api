using Assette.Client;
using Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Api.Repositories
{
    public static class ConverterExtensions
    {

        //public static TRootOutModel ConvertTo<TInput,TRootOutModel,TInnerOutModel>(this List<ResultObj<TInput>> internalModel) where TRootOutModel : class,new()
        //                                                                                    where TInput:class,new()
        //                                                                                    where TInnerOutModel:class, new ()

        //{
        //    var outputRootModel = new TRootOutModel();
        //    var innerCollection = new List<TInnerOutModel>();

        //    internalModel.ForEach(x => innerCollection.Add(new TInnerOutModel()
        //    {
        //        Data = x.Data,
        //        Success = x.Success,
        //        ErrorCode = x.ErrorCode,
        //        Messages = x.Messages
        //    }));

        //    outputRootModel.Success = true;
        //    outputRootModel.Data = innerCollection;


        //    return outputRootModel;
        //}

        public static AccountSimpleDtoListResult ConvertTo(this List<ResultObj<AccountSimpleDto>> internalModel) 
        {
            try
            {
                var outputRootModel = new AccountSimpleDtoListResult();
                var innerCollection = new List<AccountSimpleDtoResult>();

                internalModel.ForEach(x =>

                innerCollection.Add(new AccountSimpleDtoResult()
                {
                    Data = x.Data,
                    Success = x.Success,
                    ErrorCode = x.ErrorCode,
                    Messages = x.Messages
                }));
                outputRootModel.Success = true;
                outputRootModel.Data = innerCollection;
                outputRootModel.AdditionalProperties = new Dictionary<string, object>();
                return outputRootModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static UserSimpleDtoListResult ConvertTo(this List<ResultObj<UserSimpleDto>> internalModel)
        {
            var outputRootModel = new UserSimpleDtoListResult();
            var innerCollection = new List<UserSimpleDtoResult>();

            internalModel.ForEach(x =>

            //User Automapper
            innerCollection.Add(new UserSimpleDtoResult()
            {
                Data = x.Data,
                Success = x.Success,
                ErrorCode = x.ErrorCode,
                Messages = x.Messages
            }));
            outputRootModel.Success = true;
            outputRootModel.Data = innerCollection;
            return outputRootModel;
        }

        public static UserSimpleDtoResult ConvertToSimpleDto(this ResultObj<UserDto> internalModel)
        {
            var outputRootModel = new UserSimpleDtoResult();
            outputRootModel.Success = internalModel.Success;
            outputRootModel.Data = new UserSimpleDto() {
                UserCode =internalModel.Data.UserCode,
                UserId =internalModel.Data.UserId
            };
            outputRootModel.Messages = internalModel.Messages;
            return outputRootModel;
        }
        public static UserDtoResult ConvertTo(this ResultObj<UserDto> internalModel)
        {
            var outputRootModel = new UserDtoResult();
            outputRootModel.Success = internalModel.Success;
            outputRootModel.Data = internalModel.Data;
            outputRootModel.Messages = internalModel.Messages;
            return outputRootModel;
        }
        public static TRootOutput ConvertTo<TRootOutput, TInnerOutput, TInnerInput>(this List<ResultObj<TInnerInput>> internalModel)    where TRootOutput : class,new()
                                                                                                                                        where TInnerOutput : class, new()
                                                                                                                                        where TInnerInput : class, new ()
        {
            var outputRootModel = new TRootOutput();
            var innerCollection = new List<TInnerOutput>();

            //internalModel.ForEach(x =>

            //innerCollection.Add(new TInnerOutput()where TInnerOutput : class,new
            //{
            //    Data = x.Data,
            //    Success = x.Success,
            //    ErrorCode = x.ErrorCode,
            //    Messages = x.Messages
            //}));
            //outputRootModel.Success = true;
            //outputRootModel.Data = innerCollection;

            return new TRootOutput();// outputRootModel;
        }

        public static AccountDtoResult ConvertTo(this ResultObj<AccountDto> internalModel)
        {
            var outputRootModel = new AccountDtoResult()
            {
                Data = internalModel.Data,
                Success = internalModel.Success,
                ErrorCode = internalModel.ErrorCode,
                Messages = internalModel.Messages
            };
            return outputRootModel;
        }

        public static AccountDtoListResult ConvertTo(this ResultObj<IEnumerable<AccountDto>> internalModel)
        {
           var outputRootModel = new AccountDtoListResult();
            outputRootModel.Success = internalModel.Success;
            outputRootModel.ErrorCode = internalModel.ErrorCode;
            outputRootModel.Data = internalModel.Data.ToList();
            outputRootModel.Messages = internalModel.Messages;
            return outputRootModel;
        }

        public static UserDtoListResult ConvertTo(this ResultObj<IEnumerable<UserDto>> internalModel)
        {
            var outputRootModel = new UserDtoListResult();
            outputRootModel.Success = internalModel.Success;
            outputRootModel.ErrorCode = internalModel.ErrorCode;
            outputRootModel.Data = internalModel.Data.ToList();
            outputRootModel.Messages = internalModel.Messages;
            return outputRootModel;
        }

    }
}
