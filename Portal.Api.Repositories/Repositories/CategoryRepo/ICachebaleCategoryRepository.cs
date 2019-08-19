using Assette.Client;
using Framework.Common;
using Portal.Api.Repositories.Repos;
using System.Collections.Generic;
using SieveModel = Sieve.Models.SieveModel;

namespace Portal.Api.Repositories.Contracts
{
    public interface ICachebaleCategoryRepository : ICachebaleRepository<CategoryDto, CategoryToCreateDto, CategoryDto>
    {
        ResultObj<IEnumerable<CategoryDto>> FindByFilter(SieveModel searchModel);
    }
}