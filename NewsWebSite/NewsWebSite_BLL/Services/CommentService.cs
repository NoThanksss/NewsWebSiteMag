

using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<CommentService> _logger;

        public CommentService(IMapper mapper, ICommentRepository commentRepository, ILogger<CommentService> logger)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
            _logger = logger;

        }


        public IEnumerable<Comment> GetAllComments()
        {
            try
            {
                return _mapper.Map<List<Comment>>(_commentRepository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllComments method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public Comment AddNewComment(Comment comment)
        {
            try
            {
                var commentToAdd = _mapper.Map<CommentDB>(comment);
                var newComment = _commentRepository.AddEntity(commentToAdd);

                return _mapper.Map<Comment>(newComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewComment method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public void DeleteComment(Guid id)
        {
            try
            {
                _commentRepository.DeleteEntity(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteUser method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public Comment UpdateComment(Comment updatedComment)
        {
            try
            {
                var mappedComment = _mapper.Map<CommentDB>(updatedComment);

                return _mapper.Map<Comment>(_commentRepository.UpdateEntity(mappedComment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateComment method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public Comment GetCommentById(Guid id)
        {
            try
            {
                var comment = _commentRepository.GetById(id);
                if (comment == null)
                {
                    _logger.LogError($"Comment with id {id} doesn't exist.");
                    throw new CommentServiceException($"Comment with id {id} doesn't exist.");
                }

                return _mapper.Map<Comment>(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GeCommentById method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public IEnumerable<Comment> GetCommentsByArticleId(Guid articleId)
        {
            try
            {
                return _mapper.Map<List<Comment>>(_commentRepository.GetAll().Where(x => x.ArticleId == articleId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetCommentsByArticleId method");
                throw new CommentServiceException(ex.Message);
            }
        }
    }
}
