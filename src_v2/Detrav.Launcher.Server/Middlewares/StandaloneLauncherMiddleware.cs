using Detrav.Launcher.Server.Services;

namespace Detrav.Launcher.Server.Middlewares
{
    public class StandaloneLauncherMiddleware
    {
        private readonly RequestDelegate next;

        public StandaloneLauncherMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext context, IStandaloneLauncherService service)
        {
            if (context.Request.Headers.TryGetValue("X-DetravLauncherVersion", out var version))
            {
                service.IsEnabled = true;
                service.Version = version;
            }
            return next.Invoke(context);
        }
    }
}
