using AutoMapper;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_BLL.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDB, User>().ReverseMap();
        }
    }
}

