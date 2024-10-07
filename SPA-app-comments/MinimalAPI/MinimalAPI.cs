using Microsoft.AspNetCore.Mvc;
using SPA_app_comments.Core.Domain.Entities;
using SPA_app_comments.Core.Domain.RepositoryContracts;
using SPA_app_comments.Core.Domain.Requests.Comments;
using SPA_app_comments.Core.Helpers.Extensions;
using SPA_app_comments.Infrastructure;

namespace SPA_app_comments.MinimalAPI
{
    public class MinimalAPI
    {
        public static IResult GetMainComments(IUnitOfWork<ApplicationDbContext> db)
        {
            var repo = db.GetRepository<Comment>();
            var comments = repo.GetAll();
            var commentResponse = comments
                .Where(c => c.ParentCommentId is null)
                .Select(c => c.ToCommentResponse()).ToList();

            return TypedResults.Ok(commentResponse);
        }
        public static IResult GetRepliesForComment([FromRoute] Guid commentId, IUnitOfWork<ApplicationDbContext> db)
        {
            var repo = db.GetRepository<Comment>();
            var comments = repo.GetAll();
            var commentResponse = comments
                .Where(c => c.ParentCommentId == commentId)
                .Select(c => c.ToCommentResponse()).ToList();

            return TypedResults.Ok(commentResponse);
        }
        public async static Task<IResult> CreateComment(CreateCommentRequest request, IUnitOfWork<ApplicationDbContext> db)
        {
            var commentRepo = db.GetRepository<Comment>();
            var userRepo = db.GetRepository<User>();
            var existUser = userRepo.GetAll((user) => user.Name == request.UserName && user.Email == request.Email).FirstOrDefault();
            if (existUser is not null)
            {
                var comment = commentRepo.Insert(new Comment() 
                {
                    Id = Guid.NewGuid(),
                    UserId = existUser.Id,
                    Text = request.Text,
                    ParentCommentId = request.ParentCommentId,
                });

                await db.SaveChangesAsync();
                return TypedResults.Created("/comment", comment.ToCommentResponse());
            }
            else
            {
                var user = userRepo.Insert(new User()
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    Name = request.UserName,
                });

                await db.SaveChangesAsync();

                var comment = commentRepo.Insert(new Comment()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Text = request.Text,
                    ParentCommentId = request.ParentCommentId
                });

                await db.SaveChangesAsync();
                return TypedResults.Created("/comment", comment.ToCommentResponse());

            }
        }
    }
}
