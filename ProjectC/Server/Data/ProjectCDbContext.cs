using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Data
{
    public class ProjectCDbContext : DbContext
    {
        public DbSet<RequestEvent> RequestEvent { get; set; }
        public DbSet<RequestRule> RequestRule { get; set; }
        public DbSet<WebhookRule> WebhookRule { get; set; }
        public DbSet<WebhookEvent> WebhookEvents { get; set; }
        public DbSet<Workflow> Workflow { get; set; }
        public DbSet<WorkflowAction> WorkflowAction { get; set; }
        public DbSet<WorkflowStorage> WorkflowStorage { get; set; }
        public DbSet<WorkflowTrigger> WorkflowTrigger { get; set; }

        public ProjectCDbContext(DbContextOptions<ProjectCDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestEvent>().ToTable("RequestEvent");
            modelBuilder.Entity<RequestEvent>().Ignore(x => x.Headers);
            modelBuilder.Entity<RequestEvent>().Ignore(x => x.Body);
            modelBuilder
                .Entity<RequestEvent>()
                .HasOne(x => x.RequestRule)
                .WithMany(x => x.RequestRuleEvents)
                .HasForeignKey(x => x.RequestRuleId);
            modelBuilder
                .Entity<RequestEvent>()
                .HasOne(x => x.WebhookRule)
                .WithMany(x => x.WebhookRuleEvents)
                .HasForeignKey(x => x.WebhookRuleId);
            modelBuilder
                .Entity<RequestEvent>()
                .HasOne(x => x.WorkflowAction)
                .WithMany(x => x.WorkflowActionEvents)
                .HasForeignKey(x => x.WorkflowActionId);

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

            modelBuilder.Entity<WebhookRule>().ToTable("WebhookRule");
            modelBuilder
                .Entity<WebhookRule>()
                .Property(x => x.Method)
                .HasConversion(
                    x => x.ToString(),
                    x => (WebhookRuleMethod)Enum.Parse(typeof(WebhookRuleMethod), x)
                );

            modelBuilder.Entity<WebhookEvent>().ToTable("WebhookEvent");
            modelBuilder
                .Entity<WebhookEvent>()
                .HasOne(x => x.WebhookRule)
                .WithMany(x => x.WebhookEvents)
                .HasForeignKey(x => x.WebhookRuleId);

            modelBuilder.Entity<Workflow>().ToTable("Workflow");
            modelBuilder
                .Entity<Workflow>()
                .HasMany(x => x.WorkflowActions)
                .WithOne(x => x.Workflow)
                .HasForeignKey(x => x.WorkflowId);
            modelBuilder
                .Entity<Workflow>()
                .HasOne(x => x.WorkflowStorage)
                .WithOne(x => x.Workflow)
                .HasForeignKey<WorkflowStorage>(x => x.WorkflowId);

            modelBuilder.Entity<WorkflowAction>().ToTable("WorkflowAction");
            _ = modelBuilder
                .Entity<WorkflowAction>()
                .Property(x => x.Method)
                .HasConversion(
                    x => x.ToString(),
                    x =>
                        !string.IsNullOrEmpty(x)
                            ? (RequestRuleMethod)Enum.Parse(typeof(RequestRuleMethod), x)
                            : null
                );
            modelBuilder
                .Entity<WorkflowAction>()
                .HasOne(x => x.Workflow)
                .WithMany(x => x.WorkflowActions)
                .HasForeignKey(x => x.WorkflowId);
            modelBuilder
                .Entity<WorkflowAction>()
                .HasOne(x => x.RequestRule)
                .WithMany(x => x.WorkflowActions)
                .HasForeignKey(x => x.RequestRuleId);

            modelBuilder.Entity<WorkflowStorage>().ToTable("WorkflowStorage");
            modelBuilder
                .Entity<WorkflowStorage>()
                .HasOne(x => x.Workflow)
                .WithOne(x => x.WorkflowStorage)
                .HasForeignKey<WorkflowStorage>(x => x.WorkflowId);

            modelBuilder.Entity<WorkflowTrigger>().ToTable("WorkflowTrigger");
            modelBuilder
                .Entity<WorkflowTrigger>()
                .HasOne(x => x.WorkflowAction)
                .WithMany(x => x.WorkflowTriggers)
                .HasForeignKey(x => x.WorkflowActionId);
            modelBuilder
                .Entity<WorkflowTrigger>()
                .HasOne(x => x.WebhookEvent)
                .WithMany(x => x.WorkflowTriggers)
                .HasForeignKey(x => x.WebhookEventId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
