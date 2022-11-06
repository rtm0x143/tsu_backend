using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace MovieCatalogBackend.Services.Auth;

public class AuthorizationResultTransformer : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden && authorizeResult.AuthorizationFailure != null)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            var result = new ProblemDetails
            {
                Title = "Permission denied",
                Status = StatusCodes.Status403Forbidden,
                Detail = string.Join('\n', authorizeResult.AuthorizationFailure.FailureReasons)
            };

            context.Response.Headers.ContentType = MediaTypeNames.Application.Json;
            await context.Response.BodyWriter.WriteAsync(Encoding.ASCII.GetBytes(result.ToJson()));
            return;
        }

        await next.Invoke(context);
    }
}