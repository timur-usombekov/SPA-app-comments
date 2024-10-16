﻿using Microsoft.AspNetCore.Mvc;
using SPA_app_comments.Core.Domain.Entities;
using SPA_app_comments.Core.Domain.RepositoryContracts;
using SPA_app_comments.Core.Domain.Requests.Comments;
using SPA_app_comments.Core.Dto;
using SPA_app_comments.Core.Helpers.Exeptions;
using SPA_app_comments.Core.Helpers.Extensions;
using SPA_app_comments.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace SPA_app_comments.MinimalAPI
{
    public class MinimalAPI
    {
        public static IResult GetMainComments(IUnitOfWork<ApplicationDbContext> db, [FromQuery]string? sortBy = null)
        {
            var repo = db.GetRepository<Comment>();
            var comments = repo.GetAll();

            comments = sortBy switch
            {
                "username" => comments.OrderBy(c => c.User.Name), 
                "email" => comments.OrderBy(c => c.User.Email),
                "date" => comments.OrderBy(c => c.CreatedAt), 
                _ => comments.OrderByDescending(c => c.CreatedAt)
            };

            var commentResponse = comments
                .Where(c => c.ParentCommentId is null)
                .Select(c => c.ToCommentResponse()).ToList();


            return TypedResults.Ok(commentResponse);
        }
        public static IResult GetRepliesForComment([FromRoute] Guid commentId, IUnitOfWork<ApplicationDbContext> db, [FromQuery] string? sortBy = null)
        {
            var repo = db.GetRepository<Comment>();
            var comments = repo.GetAll();

            comments = sortBy switch
            {
                "username" => comments.OrderBy(c => c.User.Name),
                "email" => comments.OrderBy(c => c.User.Email),
                "date" => comments.OrderBy(c => c.CreatedAt),
                _ => comments.OrderByDescending(c => c.CreatedAt)
            };
            var commentResponse = comments
                .Where(c => c.ParentCommentId == commentId)
                .Select(c => c.ToCommentResponse()).ToList();

            return TypedResults.Ok(commentResponse);
        }

        public async static Task<IResult> CreateComment([FromForm] CreateCommentRequest request, IUnitOfWork<ApplicationDbContext> db)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request);

            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            {
                var errors = validationResults.Select(vr => new ErrorResponse
                {
                    Error = vr.ErrorMessage
                });

                return Results.BadRequest(errors);
            }

            var commentRepo = db.GetRepository<Comment>();
            var userRepo = db.GetRepository<User>();
            var existUser = userRepo.GetAll((user) => user.Name == request.UserName && user.Email == request.Email).FirstOrDefault();
            if (existUser is not null)
            {
                try
                {
                    return await SaveCommentWithoutUser(commentRepo, db, existUser, request);
                }
                catch (FileSizeException e)
                {
                    return TypedResults.BadRequest(new ErrorResponse() {Error = e.Message });
                }
            }
            else
            {
                try
                {
                    return await SaveCommentWithUser(commentRepo, userRepo, db, request);
                }
                catch (FileSizeException e)
                {
                    return TypedResults.BadRequest(new ErrorResponse() { Error = e.Message });
                }

            }
        }
        private static async Task<IResult> SaveCommentWithoutUser(IRepository<Comment> commentRepository, 
            IUnitOfWork<ApplicationDbContext> db,
            User user, CreateCommentRequest request)
        {
            var comment = commentRepository.Insert(new Comment()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                Text = request.Text,
                Url = request.Url,
                File = await CheckFile(request.File),
                FileExtension = Path.GetExtension(request.File?.FileName),
                ParentCommentId = request.ParentCommentId == Guid.Empty ? null : request.ParentCommentId,
            });

            await db.SaveChangesAsync();
            return TypedResults.Created("/comment", comment.ToCommentResponse());
        }

        private static async Task<IResult> SaveCommentWithUser(
            IRepository<Comment> commentRepository,
            IRepository<User> userRepository,
            IUnitOfWork<ApplicationDbContext> db,
            CreateCommentRequest request)
        {
            var user = userRepository.Insert(new User()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Name = request.UserName,
            });

            await db.SaveChangesAsync();

            var comment = commentRepository.Insert(new Comment()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                Url = request.Url,
                Text = request.Text,
                File = await CheckFile(request.File),
                FileExtension = Path.GetExtension(request.File?.FileName),
                ParentCommentId = request.ParentCommentId == Guid.Empty ? null : request.ParentCommentId
            });

            await db.SaveChangesAsync();
            return TypedResults.Created("/comment", comment.ToCommentResponse());
        }

        private static async Task<byte[]?> CheckFile(IFormFile? file)
        {
            if (file is null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                if (Path.GetExtension(file.FileName) == ".txt" && file.Length > 1024 * 100)
                    throw new FileSizeException("Text file size exceeds the limit of 100 KB.");
                var fileData = memoryStream.ToArray(); 

                return fileData;
            }
        }
    }
}
