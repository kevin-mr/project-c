using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Hubs;
using ProjectC.Server.Services;

namespace ProjectC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" }
                );
            });
            builder.Services.AddDbContext<ProjectCDbContext>(
                options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectC-DB"))
            );
            builder.Services.AddSingleton<IRequestInspectorService, RequestInspectorService>();
            builder.Services.AddTransient<IMockServerService, MockServerService>();

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
                    if (context.Request.Path.StartsWithSegments("/custom"))
                    {
                        var mockServerService =
                            context.RequestServices.GetRequiredService<IMockServerService>();
                        if (mockServerService is not null)
                        {
                            var request = await mockServerService.FindRequest(context.Request);
                            if (request is not null)
                            {
                                await context.Response.WriteAsync(request.Body);
                                return;
                            }
                        }
                    }

                    await next(context);
                }
            );

            app.UseRouting();

            app.Use(
                async (context, next) =>
                {
                    Console.WriteLine(
                        $"After. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}"
                    );
                    await next(context);
                }
            );

            app.MapRazorPages();
            app.MapControllers();

            app.MapHub<RequestHistoryHub>("/request-history");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
