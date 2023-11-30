

using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
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


        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            try
            {
                var comments = await _commentRepository.GetAllAsync().ToListAsync();
                return _mapper.Map<List<Comment>>(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllComments method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public async Task<Comment> AddNewCommentAsync(Comment comment)
        {
            try
            {
                var commentToAdd = _mapper.Map<CommentDB>(comment);
                var newComment = await _commentRepository.AddEntityAsync(commentToAdd);

                return _mapper.Map<Comment>(newComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewComment method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            try
            {
                await _commentRepository.DeleteEntityAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteUser method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public async Task<Comment> UpdateCommentAsync(Comment updatedComment)
        {
            try
            {
                var mappedComment = _mapper.Map<CommentDB>(updatedComment);
                var result = await _commentRepository.UpdateEntityAsync(mappedComment);

                return _mapper.Map<Comment>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateComment method");
                throw new CommentServiceException(ex.Message);
            }
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            try
            {
                var comment = await _commentRepository.GetByIdAsync(id);
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

        public async Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(Guid articleId)
        {
            try
            {
                var comments = await _commentRepository.GetAllAsync().Where(x => x.ArticleId == articleId).ToListAsync();

                return _mapper.Map<List<Comment>>(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetCommentsByArticleId method");
                throw new CommentServiceException(ex.Message);
            }
        }
    }
}
