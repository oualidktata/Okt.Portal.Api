using Assette.Client;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Portal.Api.Repositories.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options,
            ISieveCustomSortMethods customSortMethods,
            ISieveCustomFilterMethods customFilterMethods)
            : base(options, customSortMethods, customFilterMethods)
        {
            
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.AddAccountSieveMappings()
                .AddUserSieveMappings();
            return mapper;
        }
    }

    public static class SieveMapperExtensions
    {
        public static SievePropertyMapper AddAccountSieveMappings(this SievePropertyMapper mapper)
        {
            mapper.Property<AccountDto>(p => p.Code)
                            .CanFilter()
                            .CanSort();

            mapper.Property<AccountDto>(p => p.IsActive)
                .CanFilter()
                .CanSort();

            mapper.Property<AccountDto>(p => p.OpenDate)
               .CanFilter()
               .CanSort();
            return mapper;
        }

        public static SievePropertyMapper AddUserSieveMappings(this SievePropertyMapper mapper)
        {
            mapper.Property<UserDto>(p => p.UserCode)
                            .CanFilter()
                            .CanSort();

            mapper.Property<UserDto>(p => p.IsActive)
                .CanFilter()
                .CanSort();

            mapper.Property<UserDto>(p => p.FirstName)
               .CanFilter()
               .CanSort();
            mapper.Property<UserDto>(p => p.LastName)
               .CanFilter()
               .CanSort();
            return mapper;
        }

    }
}
