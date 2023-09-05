using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services;
using ProjectC.Server.Services.Interfaces;
using System.Reflection;
using FluentValidation;
using ProjectC.Server.Middlewares;
using System.Text.Json.Serialization;
using ProjectC.Server.BackgroudServices;
using Microsoft.OpenApi.Models;

namespace ProjectC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddControllersWithViews()
                .AddJsonOptions(
                    options =>
                        options.JsonSerializerOptions.ReferenceHandler =
                            ReferenceHandler.IgnoreCycles
                );
            ;
            builder.Services.AddCors(
                o =>
                    o.AddPolicy(
                        "AllowAnyOrigin",
                        builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                        }
                    )
            );
            builder.Services.AddRazorPages();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Project C",
                        Description = "Tool for Manual Integration Testing RESTful APIs"
                    }
                );
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            //SignalR
            builder.Services.AddSignalR();
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" }
                );
            });

            //Database
            builder.Services.AddDbContext<ProjectCDbContext>(
                options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectC-DB"))
            );

            //External Libraries
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //Services
            builder.Services.AddHostedService<WebhookEventBackgroudService>();
            builder.Services.AddSingleton<WebhookEventQueue>();
            builder.Services.AddTransient<IRequestRuleService, RequestRuleService>();
            builder.Services.AddTransient<IMockServerService, MockServerService>();
            builder.Services.AddTransient<IWebhookRuleService, WebhookRuleService>();
            builder.Services.AddTransient<IWorkflowService, WorkflowService>();
            builder.Services.AddTransient<IWorkflowActionService, WorkflowActionService>();
            builder.Services.AddTransient<IRequestEventService, RequestEventService>();
            builder.Services.AddTransient<IWorkflowStorageService, WorkflowStorageService>();
            builder.Services.AddTransient<IWebhookEventService, WebhookEventService>();
            builder.Services.AddTransient<IWorkflowTriggerService, WorkflowTriggerService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseCors("AllowAnyOrigin");

            app.UseMockServerHandler();
            app.UseWebhookHandler();

            app.UseRouting();

            app.MapRazorPages();
            app.MapControllers();

            app.MapHub<RequestHub>("/request-events");
            app.MapHub<RequestRuleHub>("/request-rule-events");
            app.MapHub<WebhookRuleHub>("/webhook-rule-events");
            app.MapHub<WorkflowActionHub>("/workflow-action-events");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
