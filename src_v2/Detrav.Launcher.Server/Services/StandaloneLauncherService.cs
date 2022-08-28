using Microsoft.Extensions.Primitives;

namespace Detrav.Launcher.Server.Services
{
    public interface IStandaloneLauncherService
    {
        bool IsEnabled { get; set; }
        string Version { get; set; }
    }

    public class StandaloneLauncherService : IStandaloneLauncherService
    {
        public bool IsEnabled { get; set; }
        public string Version { get; set; }
    }
}
