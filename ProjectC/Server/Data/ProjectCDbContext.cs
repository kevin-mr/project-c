using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Data
{
    public class ProjectCDbContext : DbContext
    {
        public DbSet<RequestRule> RequestRule { get; set; }

        public ProjectCDbContext(DbContextOptions<ProjectCDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestRule>().ToTable("RequestRule");

            modelBuilder
                .Entity<RequestRule>()
                .Property(x => x.Method)
                .HasConversion(
                    x => x.ToString(),
                    x => (RequestRuleMethod)Enum.Parse(typeof(RequestRuleMethod), x)
                );

            modelBuilder
                .Entity<RequestRule>()
                .Property(x => x.ResponseMethod)
                .HasConversion(
                    x => x.ToString(),
                    x => (RequestRuleMethod)Enum.Parse(typeof(RequestRuleMethod), x)
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
