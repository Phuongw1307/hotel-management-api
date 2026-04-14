using Hotel_Management_API.Data;
using Hotel_Management_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel_Management_API.Middlewares
{
    public class TokenVersionMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenVersionService tokenVersionService)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var tokenVersionClaim = context.User.FindFirst("tokenVersion")?.Value;

                if (!int.TryParse(userIdClaim, out var userId) ||
                    !int.TryParse(tokenVersionClaim, out var tokenVersion))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Invalid token claims" });
                    return;
                }

                var isValid = await tokenVersionService.IsTokenVersionValidAsync(userId, tokenVersion);

                if (!isValid)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Token has been revoked" });
                    return;
                }
            }

            await _next(context);
        }
    }
}
