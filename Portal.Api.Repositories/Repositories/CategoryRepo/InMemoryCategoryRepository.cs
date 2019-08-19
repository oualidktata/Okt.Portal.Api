﻿using System;
using System.Collections.Generic;
using Assette.Client;
using AutoMapper;
using Framework.Common;
using Microsoft.Extensions.Caching.Memory;
using Portal.Api.Repositories.Contracts;
using Sieve.Services;
using SieveModel = Sieve.Models.SieveModel;

namespace Portal.Api.Repositories.Repos
{


    public class InMemoryCategoryRepository : CachebaleRepository<CategoryDto, CategoryToCreateDto, CategoryDto>, ICachebaleCategoryRepository
    {
        public InMemoryCategoryRepository(IMapper mapper, IMemoryCache cache, ICategoryRepoOptions repoOptions, ISieveProcessor sieveProcessor) : base(mapper, cache, repoOptions,sieveProcessor)
        {
        }

        public ResultObj<IEnumerable<CategoryDto>> FindByFilter(SieveModel searchModel)
        {
            throw new NotImplementedException();
        }
    }
}
