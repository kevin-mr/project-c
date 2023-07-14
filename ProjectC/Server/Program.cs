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
            builder.Services.AddRazorPages();

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
            builder.Services.AddTransient<IRequestRuleService, RequestRuleService>();
            builder.Services.AddSingleton<IRequestInspectorService, RequestInspectorService>();
            builder.Services.AddTransient<IMockServerService, MockServerService>();
            builder.Services.AddTransient<IWebookRuleService, WebookRuleService>();
            builder.Services.AddTransient<IWorkflowService, WorkflowService>();
            builder.Services.AddTransient<IWorkflowActionService, WorkflowActionService>();
            builder.Services.AddTransient<IRequestEventService, RequestEventService>();
            builder.Services.AddTransient<IWorkflowStorageService, WorkflowStorageService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

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
