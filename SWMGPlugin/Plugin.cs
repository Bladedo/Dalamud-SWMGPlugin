using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using SWMGPlugin.Windows;
using SWMGPlugin.Helpers.AudioHelper;
using Dalamud.Plugin.Services;

namespace SWMGPlugin
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "SWMG Plugin";

        [PluginService] 
        public static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

        [PluginService] 
        internal static ICommandManager CommandManager { get; private set; } = null!;

        [PluginService] 
        internal static ITextureProvider TextureProvider { get; private set; } = null!;

        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SWMGPlugin");

        private ConfigWindow ConfigWindow { get; init; }

        private MainWindow MainWindow { get; init; }

    private SWMG SWMG { get; set; }

        public Plugin()
        {
            PluginInterface.Create<Services>();
            Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();            

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "swmg.png");

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this, imagePath);

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            SWMG = new SWMG();
            SWMG.OnEnable();
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            AudioHelper.Teardown();
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            SWMG.OnDisable();
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
