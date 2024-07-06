using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;

namespace SWMGPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private string swmgImagePath;
    private Plugin plugin;

    public MainWindow(Plugin plugin, string swmgImagePath) : base("My Amazing Window##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.swmgImagePath = swmgImagePath;
        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text($"Volume is {this.plugin.Configuration.Volume}");

        if (ImGui.Button("Show Settings"))
        {
            this.plugin.DrawConfigUI();
        }

        ImGui.Spacing();

        ImGui.Text("Have a wizard:");

        var swmgImage = Plugin.TextureProvider.GetFromFile(swmgImagePath).GetWrapOrDefault();

        if (swmgImage != null)
        {
            ImGui.Indent(55f);
            ImGui.Image(swmgImage.ImGuiHandle, new Vector2(swmgImage.Width, swmgImage.Height));
            ImGui.Unindent(55f);
        }
       
    }
}
