using Sieve.Services;
using System.Linq;

namespace Portal.Api.Dtos.Sieve
{
    public class SieveCustomFilterMethods : ISieveCustomFilterMethods
    {
        public IQueryable<AssociationDto> IsNew(IQueryable<AssociationDto> source, string op, string[] values)
            => source.Where(p => p.AccountCode ==values[0] && p.DocumentTypeCode == values[1]);
    }
}
