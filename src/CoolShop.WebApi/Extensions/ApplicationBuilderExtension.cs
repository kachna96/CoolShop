using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace CoolShop.WebApi.Extensions;

/// <summary>
/// Application builder extensions for registrations during startup
/// </summary>
public static class ApplicationBuilderExtension
{
    /// <summary>
    /// Add custom message when exception occurs and return InternalServerError
    /// </summary>
    /// <param name="applicationBuilder"></param>
    public static void UseApplicationHandlingMiddleware(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseExceptionHandler(error =>
        {
            error.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                if (feature is not null)
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "Something went wrong, please try again later.",
                        StatusCode = StatusCodes.Status500InternalServerError
                    });
                }
            });
        });
    }
}
