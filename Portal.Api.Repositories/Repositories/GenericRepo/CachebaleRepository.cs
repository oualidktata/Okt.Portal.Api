using Assette.Client;
using AutoMapper;
using Framework.Common;
using Microsoft.Extensions.Caching.Memory;
using Portal.Api.Repositories.Repos;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using SieveModel = Sieve.Models.SieveModel;

namespace Portal.Api.Repositories.Contracts
{
    public class CachebaleRepository<TEntity, TInputDto, TOutputDto> : ICachebaleRepository<TEntity, TInputDto, TOutputDto>
                                    where TEntity : class, new()
                                    where TInputDto : class
                                    where TOutputDto : class
    {
        protected IMapper _mapper { get; set; }
        protected IMemoryCache _cache { get; set; }
        protected ISieveProcessor _sieveProcessor { get; set; }

        protected string _cacheItemName { get; set; }
        public CachebaleRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        protected List<TEntity> ListOfItems
        {
            get
            {
                if (_cache.Get<List<TEntity>>(_cacheItemName) == null)
                {
                    _cache.Set(_cacheItemName, new List<TEntity>());
                }
                return _cache.Get<List<TEntity>>(_cacheItemName);
            }
            set
            {
                if (_cache.Get<List<TEntity>>(_cacheItemName) == null)
                {
                    _cache.Set(_cacheItemName, new List<TEntity>());
                }
                _cache.Set(_cacheItemName, value);
            }
        }

        public CachebaleRepository(IMapper mapper, IMemoryCache cache, IRepoOptions repoOptions, ISieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _cache = cache;
            _cacheItemName = repoOptions.CacheItemName;
            _sieveProcessor = sieveProcessor;
        }

        public IEnumerable<ResultObj<TOutputDto>> CreateOrUpdateEntities(TInputDto[] itemsToCreate, string itemToCreatePropName, string entityPropName)
        {
            var results = new List<ResultObj<TOutputDto>>();
            foreach (var item in itemsToCreate)
            {
                //Create a single user and return the result
                results.Add(CreateOrUpdateOneEntity(item, itemToCreatePropName, entityPropName));
            }
            return results;
        }

        //public IResult<TOutputDto> CreateEntity(TInputDto itemToCreate, Func<TEntity, bool> keyFilter)
        //{
        //    if (FindByKey(keyFilter).Success)
        //    {
        //        return new Result<TOutputDto>(false, _mapper.Map<TOutputDto>(itemToCreate), $"User already exists for the key"
        //            //{itemToCreate.userCode}"
        //            );
        //    }
        //    var entity = _mapper.Map<TEntity>(itemToCreate);
        //    ListOfItems.Add(entity);
        //    return new Result<TOutputDto>(true, _mapper.Map<TOutputDto>(entity));
        //}

        public ResultObj<TEntity> FindByKey(Func<TEntity, bool> predicate)
        {
            try
            {
                if (!ListOfItems.Any())
                {
                    return new ResultBuilder<TEntity>().Failure("No Item found or list is Empty").Build();
                }
                var listOfItems = ListOfItems.Where(predicate).TryGetUniqueItemFromList(_cacheItemName);
                return listOfItems;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        private ResultObj<TOutputDto> CreateOrUpdateOneEntity(TInputDto itemToCreate, string itemToCreateKeyName, string entityKeyName)
        {

            //TO DO: implement mapping
            var entity = _mapper.Map<TInputDto, TEntity>(itemToCreate);
            var outputDto = _mapper.Map<TEntity, TOutputDto>(entity);
            var itemToCreateValue = itemToCreate.GetType().GetProperty(itemToCreateKeyName).GetValue(itemToCreate, null) as string;
           
            var isAlreadyThere = ListOfItems.Any(x => x.GetType().GetProperty(itemToCreateKeyName).GetValue(x, null) as string == itemToCreateValue);

            if (isAlreadyThere)//then update
            {
                //return new ResultBuilder<TOutputDto>()
                //    .Failure("Entity already Exists")
                //    .SetData(outputDto)
                //    .Build();
                var entityFound=ListOfItems.FirstOrDefault(x=> x.GetType().GetProperty(itemToCreateKeyName).GetValue(x, null) as string == itemToCreateValue);
                ListOfItems.Remove(entityFound);
                ListOfItems.Add(entity);
                return new ResultBuilder<TOutputDto>().Success(outputDto).AddMessage("Modified").Build();
            }
       
            ListOfItems.Add(entity);
            return new ResultBuilder<TOutputDto>().Success(outputDto).Build();
        }


        public ResultObj<IEnumerable<TEntity>> FindMany(Func<TEntity, bool> predicate)
        {
            if (predicate == null)
            {
                return new ResultBuilder<IEnumerable<TEntity>>().Success(ListOfItems.ToList()).Build();
            }
            else
            {
                return new ResultBuilder<IEnumerable<TEntity>>().Success(ListOfItems.Where(predicate).ToList()).Build();
            }
        }

        public ResultObj Remove(Func<TEntity, bool> keyPredicate, string softPropertyToFlag)
        {
            var result=this.FindByKey(keyPredicate);
            if (!result.Success) //not found
            {
                return new ResultBuilder()
                        .Failure("Not found")
                        .Build();
            }
            ListOfItems.Remove(result.Data);
            result.Data.GetType().GetProperty(softPropertyToFlag).SetValue(result.Data,false);
            ListOfItems.Add(result.Data);
            return new ResultBuilder()
                    .Success()
                    .Build();
        }

        public ResultObj Remove(Func<TEntity, bool> keyPredicate)
        {
            var result = this.FindByKey(keyPredicate);
            if (!result.Success) //not found
            {
                return new ResultBuilder()
                        .Failure("Not found")
                        .Build();
            }
            ListOfItems.Remove(result.Data);

            return new ResultBuilder()
                    .Success()
                    .Build();
        }

        public ResultObj<IEnumerable<TEntity>> SearchFor(SieveModel searchModel)
        {

            var filteredList = _sieveProcessor.Apply<TEntity>(searchModel, ListOfItems.AsQueryable());
            return new ResultBuilder<IEnumerable<TEntity>>().Success(filteredList.ToList()).Build();
        }

       
        IEnumerable<ResultObj<TOutputDto>> ICachebaleRepository<TEntity, TInputDto, TOutputDto>.Remove<TKeyProperty, TSoftProperty>
            (string[] listOfCodes, Func<TEntity, TKeyProperty> keySelector, Func<TEntity, TSoftProperty> softPropertySelector,string softPropertyName, TOutputDto defaultEmptyOutput)
        {
            var listOfResults = new List<ResultObj<TOutputDto>>();
            listOfCodes.ToList().ForEach((item) => {
                var result = this.FindByKey(x => keySelector(x).Equals(item));
                if (!result.Success)
                {
                    //var output = _mapper.Map<TEntity, TOutputDto>(result.Data);
                    //output.GetType().GetProperty(keyPropertyName).SetValue(output, string.Empty);
                    listOfResults.Add(new ResultBuilder<TOutputDto>().Failure(result.Messages.FirstOrDefault()).SetData(defaultEmptyOutput).Build());
                }
                else
                {
                    var itemToSoftDelete = ListOfItems.Find(x => keySelector(x).Equals(item));
                    ListOfItems.Remove(itemToSoftDelete);
                    itemToSoftDelete.GetType().GetProperty(softPropertyName).SetValue(itemToSoftDelete, false);
                    
                    ListOfItems.Add(itemToSoftDelete);
                    var itemToReturn=_mapper.Map<TEntity, TOutputDto>(itemToSoftDelete);
                    listOfResults.Add(new ResultBuilder<TOutputDto>().Success(itemToReturn).Build());
                }
            });
            return listOfResults;
        }

        //IEnumerable<TEntity> ICachebableRepository<TEntity, TInputDto, TOutputDto>.GetItems(Func<TEntity, bool> predicate)
        //{
        //    throw new NotImplementedException();
        //}
    }
}