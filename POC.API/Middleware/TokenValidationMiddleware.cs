using POC.Application.Redis;
using System.IdentityModel.Tokens.Jwt;

namespace POC.API.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenBlocklistService tokenBlocklistService)
        {
            var user = context.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var jti = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrWhiteSpace(jti))
                {
                    var isBlocked = await tokenBlocklistService.IsBlockedAsync(jti);
                    if (isBlocked)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Token is revoked."
                        });
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
