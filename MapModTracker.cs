using ExileCore;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

namespace TheTracker
{
    public class MapModTracker(GameController gameController, TheTrackerSettings settings)
    {
        private readonly GameController _gameController = gameController;
        private readonly TheTrackerSettings _settings = settings;

        private string[] MapMods => _settings.MapMods.Split('\n');

        public void DrawMapMods()
        {
            var mods = GetMapMods();
            if (mods.Count == 0) return;

            var flags = _settings.MoveWindow
                ? ImGuiWindowFlags.AlwaysAutoResize
                : ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs;

            ImGui.SetNextWindowBgAlpha(0.75f);
            ImGui.Begin($"Map Mods", flags);
            mods.ForEach(mod => ImGui.TextColored(MapMods.GetColor(mod.Split(':').FirstOrDefault()), mod));
            ImGui.End();
        }

        private List<string> GetMapMods()
        {
            if (_gameController.IngameState?.Data?.MapStats is null) return [];

            return _gameController.IngameState.Data.MapStats
                .Where(mod => MapMods.Contains(mod.Key.ToString()))
                .Select(mod => $"{mod.Key}: {mod.Value}").ToList();
        }
    }
}
