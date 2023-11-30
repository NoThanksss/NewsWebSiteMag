using AutoMapper;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_BLL.Mapping
{
    public class ArticleThemeMappingProfile : Profile
    {
        public ArticleThemeMappingProfile()
        {
            CreateMap<ArticleThemeDB, ArticleTheme>().ReverseMap();
        }
    }
}

