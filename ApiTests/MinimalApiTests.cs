using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using SPA_app_comments.Core.Domain.Entities;
using SPA_app_comments.Core.Domain.Entities.Base;
using SPA_app_comments.Core.Domain.RepositoryContracts;
using SPA_app_comments.Core.Domain.Requests.Comments;
using SPA_app_comments.Core.Dto;
using SPA_app_comments.Infrastructure;
using SPA_app_comments.MinimalAPI;
using System.Linq.Expressions;

namespace Tests;
public class MinimalApiTests
{
    private readonly IUnitOfWork<ApplicationDbContext> _mockUnitOfWork;
    private readonly IRepository<Comment> _mockCommentRepository;
    private readonly IRepository<User> _mockUserRepository;

    public MinimalApiTests()
    {
        _mockUnitOfWork = Substitute.For<IUnitOfWork<ApplicationDbContext>>();
        _mockCommentRepository = Substitute.For<IRepository<Comment>>();
        _mockUserRepository = Substitute.For<IRepository<User>>();

        _mockUnitOfWork.GetRepository<Comment>().Returns(_mockCommentRepository);
        _mockUnitOfWork.GetRepository<User>().Returns(_mockUserRepository);
    }

    [Fact]
    public async Task CreateComment_InvalidModel_ReturnsBadRequestWithErrors()
    {
        var request = new CreateCommentRequest
        {
            UserName = "",
            Email = "invalid-email", 
            Text = "", 
            Url = "invalid-url"
        };

        var result = await MinimalAPI.CreateComment(request, _mockUnitOfWork);

        Assert.IsType<BadRequest<IEnumerable<ErrorResponse>?>>(result);
    }

    [Fact]
    public async Task CreateComment_ValidModelWithoutUserName_ReturnsBadRequest()
    {
        var request = new CreateCommentRequest
        {
            UserName = "",
            Email = "test@test.com",
            Text = "Test comment",
            Url = "https://valid.url"
        };

        var result = await MinimalAPI.CreateComment(request, _mockUnitOfWork);

        Assert.IsType<BadRequest<IEnumerable<ErrorResponse>?>>(result);

    }

    [Fact]
    public async Task CreateComment_InvalidEmail_ReturnsBadRequest()
    {
        var request = new CreateCommentRequest
        {
            UserName = "TestUser",
            Email = "invalid-email", 
            Text = "Test comment",
            Url = "https://valid.url"
        };

        var result = await MinimalAPI.CreateComment(request, _mockUnitOfWork);

        Assert.IsType<BadRequest<IEnumerable<ErrorResponse>?>>(result);

    }

    [Fact]
    public async Task CreateComment_InvalidUrl_ReturnsBadRequest()
    {
        var request = new CreateCommentRequest
        {
            UserName = "TestUser",
            Email = "test@test.com",
            Text = "Test comment",
            Url = "invalid-url"
        };

        var result = await MinimalAPI.CreateComment(request, _mockUnitOfWork);

        Assert.IsType<BadRequest<IEnumerable<ErrorResponse>?>>(result);

    }
}
