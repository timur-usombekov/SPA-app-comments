using Microsoft.EntityFrameworkCore;
using SPA_app_comments.Core.Domain.RepositoryContracts;
using SPA_app_comments.Infrastructure;
using SPA_app_comments.Infrastructure.Repositories;
using SPA_app_comments.MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/comment", MinimalAPI.GetComments);
app.MapPost("/comment", MinimalAPI.CreateComment);

app.Run();