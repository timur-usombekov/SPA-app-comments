using Microsoft.EntityFrameworkCore;
using SPA_app_comments.Core.Domain.RepositoryContracts;
using SPA_app_comments.Infrastructure;
using SPA_app_comments.Infrastructure.Repositories;
using SPA_app_comments.MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString(nameof(ApplicationDbContext))));
builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/comment", MinimalAPI.GetMainComments);
app.MapGet("/comment/{commentId}", MinimalAPI.GetRepliesForComment);
app.MapPost("/comment", MinimalAPI.CreateComment).DisableAntiforgery();

app.UseCors("AllowAll");

app.Run();