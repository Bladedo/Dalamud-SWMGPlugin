using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace SWMGPlugin
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public float Volume { get; set; } = 0.1f;

        public void Save()
        {
            Plugin.PluginInterface.SavePluginConfig(this);
        }
    }
}
