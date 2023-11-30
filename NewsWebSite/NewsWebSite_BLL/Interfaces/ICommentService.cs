using NewsWebSite_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_BLL.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> AddNewCommentAsync(Comment comment);
        Task DeleteCommentAsync(Guid id);
        Task<Comment> UpdateCommentAsync(Comment updatedComment);
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(Guid articleId);
    }
}
