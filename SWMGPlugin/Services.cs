using Dalamud.Game.ClientState;
using Dalamud.Game.Gui;
using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Data;

namespace SWMGPlugin
{
    internal class Services
    {

        [PluginService]
        public static SigScanner SigScanner { get; set; }

        [PluginService]
        public static DataManager DataManager { get; set; }
    }
}
