using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin.Services;

namespace SWMGPlugin
{
    internal class Services
    {

        [PluginService]
        public static ISigScanner SigScanner { get; set; }

        [PluginService]
        public static IDataManager DataManager { get; set; }

        [PluginService]
        public static IGameInteropProvider GameInteropProvider { get; set; }        
    }
}
