using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services;
using ProjectC.Server.Services.Interfaces;
using System.Reflection;
using FluentValidation;

namespace ProjectC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
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

            app.Use(
                async (context, next) =>
                {
                    if (
                        context.Request.Path.StartsWithSegments(
                            MockServerService.MOCK_SERVER_PREFIX
                        )
                    )
                    {
                        var mockServerService =
                            context.RequestServices.GetRequiredService<IMockServerService>();
                        if (mockServerService is not null)
                        {
                            var requestRule = await mockServerService.FindRequestRule(
                                context.Request
                            );
                            if (requestRule is not null)
                            {
                                await mockServerService.BuildRequestRuleResponse(
                                    context,
                                    requestRule
                                );
                                return;
                            }
                        }
                    }
                    else if (
                        context.Request.Path.StartsWithSegments(MockServerService.WEBHOOK_PREFIX)
                    )
                    {
                        var mockServerService =
                            context.RequestServices.GetRequiredService<IMockServerService>();
                        if (mockServerService is not null)
                        {
                            var webhookRule = await mockServerService.FindWebhookRule(
                                context.Request
                            );
                            if (webhookRule is not null)
                            {
                                await mockServerService.BuildWebhookRuleResponse(
                                    context,
                                    webhookRule
                                );
                                return;
                            }
                        }
                    }

                    await next(context);
                }
            );

            app.UseRouting();

            app.MapRazorPages();
            app.MapControllers();

            app.MapHub<RequestsHub>("/request-rule-events");
            app.MapHub<WebhookHub>("/webhook-rule-events");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
