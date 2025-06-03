using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyRecipeApp.Models;
using System.ComponentModel.Design;
using Yummly.DTO.Activities;
using Yummly.DTO.Like;
using Yummly.DTO.Post;
using Yummly.Helper;
using Yummly.Models.DbModels;

namespace Yummly.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ActivityService(ApplicationDbContext context, IMapper mapper = null)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<bool>> LikeAsync(LikeDto likeDto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == likeDto.UserId);
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == likeDto.PostId);

            if (!userExists || post == null)
            {
                return new Response<bool> { Success = false, Message = "User or Post Not Found" };
            }

            var existingLike = await _context.PostLikes
                              .FirstOrDefaultAsync(l => l.UserId == likeDto.UserId && l.PostId == likeDto.PostId);

            if (existingLike != null)
            {
                _context.PostLikes.Remove(existingLike);
                post.LikeCount--;
                // return new Response<bool> { Success = true, Message = "UnLiked Successfully" };

            }
            else
            {
                var newlike = _mapper.Map<PostLikeModel>(likeDto);
                _context.PostLikes.Add(newlike);
                post.LikeCount++;
            }
            
            await _context.SaveChangesAsync();
            return new Response<bool>
            {
                Success = true,
                Message = existingLike != null ? "Unliked Successfully" : "Liked Successfully"
            };
        }


        public async Task<Response<AddCommentDto>> AddCommentAsync(AddCommentDto commentDto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == commentDto.UserId);
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == commentDto.PostId);

            if (!userExists || post == null)
            {
                return new Response<AddCommentDto> { Success = false, Message = "User or Post Not Found" };
            }

            
            var newComment = _mapper.Map<PostCommentModel>(commentDto);

            _context.PostComments.Add(newComment);
            post.CommentCount++;
            await _context.SaveChangesAsync();
            commentDto.CommentId = newComment.CommentId;

            return new Response<AddCommentDto> { Success = true, Message = "Comment Added successfully" ,Data = commentDto  };

        }

        public async Task<Response<string>> EditCommentAsync(EditCommentDto commentDto)
        {
            var comment = await _context.PostComments.FindAsync(commentDto.CommentId);

            if (comment == null)
                return new Response<string> { Success = false, Message = "comment not found" };

            comment.Body = commentDto.Body;

            await _context.SaveChangesAsync();

            return new Response<string> { Success = true, Message = "Comment Updated successfully", Data = commentDto.Body };
        }

        public async Task<Response<bool>> DeleteCommentAsync(DeleteCommentDto commentDto)
        {
            var comment = await _context.PostComments.FindAsync(commentDto.CommentId);
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == commentDto.PostId);

            if (comment == null)
                return new Response<bool> { Success = false, Message = "comment not found", Data = false };

            _context.PostComments.Remove(comment);
            post.CommentCount--;
            await _context.SaveChangesAsync();

            return new Response<bool> { Success = true, Message = "Comment deleted successfully", Data = true };
        }

       


    }
}
