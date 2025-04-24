using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MediatR;
using System.Reflection;
using BlogAPI.Models;
using BlogAPI.Data;
using BlogAPI.Middleware;
using BlogAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using NLog;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    logger.Debug("Starting Blog API...");
    // Add services to the container.
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders(); // Remove default logging
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.Host.UseNLog(); // Setup NLog for Dependency injection

    // Add services
    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    builder.Services.AddScoped<IBlogService, BlogService>();
    builder.Services.AddScoped<IAuditService, AuditService>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    });
    builder.Services.AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV"; // e.g., v1
        options.SubstituteApiVersionInUrl = true;
    });
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog API", Version = "v1" });
    });

    var app = builder.Build();

    // Middleware
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseMiddleware<AuditMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1"));
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application stopped because of an exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
