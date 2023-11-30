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
        IEnumerable<Comment> GetAllComments();
        Comment AddNewComment(Comment comment);
        void DeleteComment(Guid id);
        Comment UpdateComment(Comment updatedComment);
        Comment GetCommentById(Guid id);
        IEnumerable<Comment> GetCommentsByArticleId(Guid articleId);
    }
}
