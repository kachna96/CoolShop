using System;
using System.IO;
using System.Reflection;
using Ardalis.GuardClauses;
using CoolShop.WebApi.Behaviors;
using CoolShop.WebApi.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CoolShop.WebApi;

/// <summary>
/// Startup
/// </summary>
public class Startup
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration">Configuration</param>
    public Startup(IConfiguration configuration)
    {
        Guard.Against.Null(configuration, nameof(configuration));

        Configuration = configuration;
    }

    /// <summary>
    /// Loaded configuration
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Configure available services to DI
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
            options.HttpsPort = 5001;
        });

        services.AddDbContext<CoolShopContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddControllers();
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoolShop v1", Version = "v1" });
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "CoolShop v2", Version = "v2" });
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        });

        services.AddAutoMapper(typeof(Startup));
        services.AddMediatR(typeof(Startup));
        services.AddValidatorsFromAssemblyContaining(typeof(Startup));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
    }

    /// <summary>
    /// Configure request pipeline
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <param name="env">Host environment</param>
    /// <param name="provider">Api version provider</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
