using AutoMapper;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_BLL.Mapping
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<AccountDB, Account>()
                .ForMember(opt => opt.UserId,
                db => db.MapFrom(inst => inst.UserDBId))
                .ForMember(opt => opt.Subscibers,
                db => db.MapFrom(inst => inst.Subscibers))
                .ForMember(opt => opt.Subscribtions,
                db => db.MapFrom(inst => inst.Subscribtions)).ReverseMap();
        }
    }
}

