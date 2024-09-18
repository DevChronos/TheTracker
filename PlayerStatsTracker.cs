using ExileCore;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

namespace TheTracker
{
    public class PlayerStatsTracker(GameController gameController, TheTrackerSettings settings)
    {
        private readonly GameController _gameController = gameController;
        private readonly TheTrackerSettings _settings = settings;

        private string[] Stats => _settings.Stats.Split('\n');

        public void DrawStats()
        {
            var buffs = GetBuffs();
            if (buffs.Count == 0) return;

            var flags = _settings.MoveWindow
                ? ImGuiWindowFlags.AlwaysAutoResize
                : ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs;

            ImGui.SetNextWindowBgAlpha(0.75f);
            ImGui.Begin($"Stats", flags);
            buffs.ForEach(buff => ImGui.TextColored(Stats.GetColor(buff.Name), buff.Description));
            ImGui.End();
        }

        private List<(string Name, string Description)> GetBuffs()
        {
            if (_gameController.Player?.Buffs is null) return [];

            return _gameController.Player.Buffs
                .Where(buff => Stats.Contains(buff.DisplayName))
                .Select(buff => (
                    buff.DisplayName,
                    $"{buff.DisplayName} {(float.IsInfinity(buff.Timer) ? string.Empty : buff.Timer.ToString("0.00"))}"
                ))
                .ToList();
        }
    }
}
