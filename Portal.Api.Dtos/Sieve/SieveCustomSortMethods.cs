using Sieve.Services;

namespace Portal.Api.Dtos.Sieve
{
    public class SieveCustomSortMethods : ISieveCustomSortMethods
    {
        //public IQueryable<AssociationDto> Popularity(IQueryable<AssociationDto> source, bool useThenBy) => useThenBy
        //    ? ((IOrderedQueryable<AssociationDto>)source).ThenBy(p => p.UserCode)
        //    : source.OrderBy(p => p.AccountCode)
        //        .ThenBy(p => p.DocumentTypeCode);
    }
}
