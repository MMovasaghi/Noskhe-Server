using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace NoskheAPI_Beta.Middleware
{
    public class ApiKeyHandler
    {
        private const string DesktopApiParameter = "desktop-api-key";
        private const string DesktopAPIKeyToCheck = @"k3bKN^u9o(sW;qH8zKXp^:.=P[}`gQ'V!wJ*8CK_da%?KB~w!?V{[YxnaY*6!rs";
        private const string AndroidApiParameter = "mobile-api-key";
        private const string AndroidAPIKeyToCheck = @"5NKr(b!ki^0.2okbH_|7]KP,*Jt=m=^;m6K~Q`F7lKB'?]qy*{6?;ylyyTti{'i";
        private RequestDelegate next;
        public ApiKeyHandler(RequestDelegate next)
        {
            this.next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            bool ValidKey = false;
            var CheckDesktopApiKeyExists = context.Request.Headers.ContainsKey(DesktopApiParameter);
            var CheckAndroidApiKeyExists = context.Request.Headers.ContainsKey(AndroidApiParameter);
            if (context.Request.Path.Value.Contains("/desktop-api"))
            {
                if(CheckDesktopApiKeyExists)
                    if (context.Request.Headers[DesktopApiParameter].Equals(DesktopAPIKeyToCheck))
                    {
                        ValidKey = true;
                    }
            }
            else if (context.Request.Path.Value.Contains("/mobile-api"))
            {
                if(CheckAndroidApiKeyExists)
                    if (context.Request.Headers[AndroidApiParameter].Equals(AndroidAPIKeyToCheck))
                    {
                        ValidKey = true;
                    }
            }
            else if (context.Request.Path.Value.Contains("/Transaction/Report")) ValidKey = true;
            else if (context.Request.Path.Value.Contains("/NotificationHub")) ValidKey = true;
            else if (context.Request.Path.Value.Contains("/api/db/init")) ValidKey = true; // felan bekhatere por kardane db

            if (!ValidKey)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Invalid API Key");
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
    public static class MyHandlerExtenstions
    {
        public static IApplicationBuilder UseApiKeyHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyHandler>();
        }
    }
}