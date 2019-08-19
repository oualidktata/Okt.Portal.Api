using Assette.Client;
using AutoMapper;
using Framework.Common;
using Microsoft.Extensions.Caching.Memory;
using Portal.Api.Repositories.Contracts;
using Sieve.Services;
using System;
using System.Collections.Generic;

namespace Portal.Api.Repositories.Repositories.AssociationRepo
{
    public class InMemoryAssociationRepository : CachebaleRepository<AssociationDto, AssociationDto, AssociationSimpleDto>, ICachebaleAssociationRepository
    {
        //public bool IsValidPermission()
        //{
        //    return true;
        //}

        //public bool Exists(AssociationSimpleDto permission)
        //{
        //    return true;
        //}

        //public IResult<AssociationDto> AddPermission(AssociationDto permissionToCreate)
        //{
        //    return new Result<AssociationDto>(true, new AssociationDto());
        //}
        //public IEnumerable<IResult<AssociationDto>> BulkInsert(AssociationDto[] permissionsToCreate)
        //{
        //    var results = new List<IResult<AssociationDto>>();
        //    foreach (var permission in permissionsToCreate)
        //    {
        //        results.Add(this.AddPermission(permission));
        //    }
        //    return results;
        //}

        public InMemoryAssociationRepository(IMapper mapper, IMemoryCache cache, IAssociationRepoOptions repoOptions,ISieveProcessor sieveProcessor) : base(mapper, cache, repoOptions,sieveProcessor)
        {
        }
        public IEnumerable<ResultObj<AssociationSimpleDto>> RemoveAssociations(AssociationDto[] associations)
        {
            throw new NotImplementedException();
        }

        public ResultObj<AssociationSimpleDto> RemoveAssociation(string userCode, string accountCode, string documenttypeCode)
        {
            throw new NotImplementedException();
        }

        public ResultObj<IEnumerable<AssociationSimpleDto>> FindBy(string userCode, string accountCode, string documentTypeCode)
        {
            throw new NotImplementedException();
        }
    }
}
