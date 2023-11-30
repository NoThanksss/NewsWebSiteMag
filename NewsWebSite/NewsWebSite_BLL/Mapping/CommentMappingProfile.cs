using AutoMapper;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_BLL.Mapping
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<CommentDB, Comment>().ReverseMap();
        }
    }
}

