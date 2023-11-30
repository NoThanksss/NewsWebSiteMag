using AutoMapper;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_BLL.Mapping
{
    public class ArticleMappingProfile : Profile
    {
        public ArticleMappingProfile()
        {
            CreateMap<ArticleDB, Article>().ReverseMap();
        }
    }
}

