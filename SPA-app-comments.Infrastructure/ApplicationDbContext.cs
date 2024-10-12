using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SPA_app_comments.Core.Domain.Entities;

namespace SPA_app_comments.Infrastructure
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions options): base(options) 
        {
            try 
            {
                var creator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (creator != null)
                {
                    if (!creator.CanConnect()) { creator.Create(); }
                    if (!creator.HasTables()) { creator.CreateTables(); }
                }
            } 
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .Navigation(c => c.User)
                .AutoInclude();
        }

    }
}
