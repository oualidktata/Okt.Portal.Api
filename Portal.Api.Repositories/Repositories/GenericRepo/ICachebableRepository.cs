using Framework.Common;
using System;
using System.Collections.Generic;
using SieveModel = Sieve.Models.SieveModel;

namespace Portal.Api.Repositories.Contracts
{
    public partial interface ICachebaleRepository<TEntity,TCreateDto,TOutputDto> 
                                    where TEntity: class,new()
                                    where TCreateDto: class
                                    where TOutputDto : class
    {
        IEnumerable<ResultObj<TOutputDto>> CreateOrUpdateEntities(TCreateDto[] itemsToCreate, string itemToCreateKeyPropertyName=null, string entityKeyPropertyName=null);
        //IEnumerable<IResult<TEntity>> BulkUpdate(TEntity[] itemsToUpdate);
        //ResultObj<TSimpleDto> CreateOneEntity(TCreateDto itemToCreate, string itemToCreateKeyPropertyName=null, string entityKeyPropertyName=null);
        /// <summary>
        /// Finds a specific Item by its key field
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>One Unique Item</returns>
        ResultObj<TEntity> FindByKey(Func<TEntity,bool> predicate);
        /// <summary>
        /// Fetch more than one item by filter predicate. If the predicate is null all elements are returned
        /// </summary>
        /// <param name="predicate">the predicate to filter by, null means get All</param>
        /// <returns>A collection of Items</returns>
        ResultObj<IEnumerable<TEntity>> FindMany(Func<TEntity, bool> predicate=null);

        /// <summary>
        /// Removes an Entity from the storage
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>A result</returns>
        ResultObj Remove(Func<TEntity, bool> predicate, string softPropToUpdate);

        /// <summary>
        /// Removes a list of entities
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>A result</returns>


        /// <summary>
        /// Removes a list of entities
        /// </summary>
        /// <typeparam name="TKeyProperty">Type of the key property selector</typeparam>
        /// <typeparam name="TSoftProperty">Type of the soft key property selector</typeparam>
        /// <param name="listOfCodes">List of item codes to softly delete</param>
        /// <param name="keySelector">Key Property Selector</param>
        /// <param name="softProperty">Soft Key Property Selector</param>
        /// <param name="softPropertyName">The name of the soft property</param>
        /// <returns></returns>
        IEnumerable<ResultObj<TOutputDto>> Remove<TKeyProperty,TSoftProperty>(string[] listOfCodes,Func<TEntity, TKeyProperty> keySelector, Func<TEntity, TSoftProperty> softProperty, string softPropertyName, TOutputDto defaultOutputObject);

        /// <summary>
        /// Removes an Entity from the storage
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>A result</returns>
        ResultObj Remove(Func<TEntity, bool> predicate);
        /// <summary>
        /// Search items based on filter
        /// </summary>
        /// <param name="searchModel">the Sieve Model</param>
        /// <returns>List of entities</returns>
        ResultObj<IEnumerable<TEntity>> SearchFor(SieveModel searchModel);
    }
}