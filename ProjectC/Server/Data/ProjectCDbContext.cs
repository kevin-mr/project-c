using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Data
{
    public class ProjectCDbContext : DbContext
    {
        public DbSet<RequestRule> RequestRule { get; set; }
        public DbSet<WebhookRule> WebhookRule { get; set; }
        public DbSet<Workflow> Workflow { get; set; }
        public DbSet<WorkflowAction> WorkflowAction { get; set; }
        public DbSet<RequestEvent> RequestEvent { get; set; }

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
                .HasMany(x => x.WorkflowActions)
                .WithOne(x => x.RequestRule)
                .HasForeignKey(x => x.RequestRuleId);
            modelBuilder
                .Entity<RequestRule>()
                .HasMany(x => x.RequestEvents)
                .WithOne()
                .HasForeignKey(x => x.RequestRuleId);

            modelBuilder.Entity<WebhookRule>().ToTable("WebhookRule");
            modelBuilder
                .Entity<WebhookRule>()
                .Property(x => x.Method)
                .HasConversion(
                    x => x.ToString(),
                    x => (WebhookRuleMethod)Enum.Parse(typeof(WebhookRuleMethod), x)
                );
            modelBuilder
                .Entity<WebhookRule>()
                .HasMany(x => x.RequestEvents)
                .WithOne()
                .HasForeignKey(x => x.WebhookRuleId);

            modelBuilder.Entity<Workflow>().ToTable("Workflow");
            modelBuilder
                .Entity<Workflow>()
                .ToTable("Workflow")
                .HasMany(x => x.WorkflowActions)
                .WithOne(x => x.WorkFlow)
                .HasForeignKey(x => x.WorkFlowId);

            modelBuilder.Entity<WorkflowAction>().ToTable("WorkflowAction");
            modelBuilder
                .Entity<WorkflowAction>()
                .HasOne(x => x.WorkFlow)
                .WithMany(x => x.WorkflowActions)
                .HasForeignKey(x => x.WorkFlowId);
            modelBuilder
                .Entity<WorkflowAction>()
                .HasOne(x => x.RequestRule)
                .WithMany(x => x.WorkflowActions)
                .HasForeignKey(x => x.RequestRuleId);

            modelBuilder.Entity<RequestEvent>().ToTable("RequestEvent");
            modelBuilder.Entity<RequestEvent>().Ignore(x => x.Headers);
            modelBuilder.Entity<RequestEvent>().Ignore(x => x.Body);
            modelBuilder
                .Entity<RequestEvent>()
                .HasOne<RequestRule>()
                .WithMany(x => x.RequestEvents)
                .HasForeignKey(x => x.RequestRuleId);
            modelBuilder
                .Entity<RequestEvent>()
                .HasOne<WebhookRule>()
                .WithMany(x => x.RequestEvents)
                .HasForeignKey(x => x.WebhookRuleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
