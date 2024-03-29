﻿using NewsWebSite_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL.Interfaces
{
    public interface IArticleThemeRepository : IBaseRepository<ArticleThemeDB>
    {
        IEnumerable<ArticleThemeDB> GetAllWithArticles();
    }
}
