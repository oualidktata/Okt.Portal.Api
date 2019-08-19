using Assette.Client;
using AutoMapper;

namespace Portal.Api.Repositories.Profiles
{
    public class MappingProfiles:Profile
    {

        public MappingProfiles()
        {
            //User mappings
            CreateMap<UserToCreateDto,UserDto>();
            CreateMap<UserToCreateDto, UserSimpleDto>();
            CreateMap<UserDto, UserSimpleDto>();
            //Account mappings
            CreateMap<AccountToCreateDto, AccountDto>();
            CreateMap<AccountDto, AccountDto>();
            CreateMap<AccountDto, AccountSimpleDto>().ForMember(destination=>destination.AccountCode, member=>member.MapFrom(x=>x.Code));
        }
    }
}
