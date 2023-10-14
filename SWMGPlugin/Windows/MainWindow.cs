using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;

namespace SWMGPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private IDalamudTextureWrap swmgImage;
    private Plugin plugin;

    public MainWindow(Plugin plugin, IDalamudTextureWrap swmgImage) : base(
        "My Amazing Window", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.swmgImage = swmgImage;
        this.plugin = plugin;
    }

    public void Dispose()
    {
        this.swmgImage.Dispose();
    }

    public override void Draw()
    {
        ImGui.Text($"Volume is {this.plugin.Configuration.Volume}");

        if (ImGui.Button("Show Settings"))
        {
            this.plugin.DrawConfigUI();
        }

        ImGui.Spacing();

        ImGui.Text("Have a wizard:");
        ImGui.Indent(55);
        ImGui.Image(this.swmgImage.ImGuiHandle, new Vector2(this.swmgImage.Width, this.swmgImage.Height));
        ImGui.Unindent(55);
    }
}
