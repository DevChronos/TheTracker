using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace TheTracker
{
    public class StrongboxTracker(GameController gameController, Graphics graphics, TheTrackerSettings settings)
    {
        private readonly GameController _gameController = gameController;
        private readonly Graphics _graphics = graphics;
        private readonly TheTrackerSettings _settings = settings;

        private Camera Camera => _gameController.Game.IngameState.Camera;
        private string[] StrongboxMods => _settings.StrongboxMods.Split('\n');
        private IEnumerable<LabelOnGround> LabelsOnGround => _gameController.IngameState.IngameUi.ItemsOnGroundLabelsVisible
            .Where(element => element?.ItemOnGround?.Metadata?.StartsWith("Metadata/Chests/StrongBox") ?? false);

        public void DrawStrongboxMods()
        {
            var strongboxes = LabelsOnGround.Where(labelOnGround => _getStrongboxMods(labelOnGround).ToList().Exists(_shouldDraw));

            foreach (var strongbox in strongboxes)
            {
                var positionedComponent = strongbox.ItemOnGround.GetComponent<Positioned>();
                if (positionedComponent is null) continue;

                var worldPosition = _gameController.IngameState.Data.ToWorldWithTerrainHeight(positionedComponent.GridPosition);
                var screenPosition = Camera.WorldToScreen(worldPosition);
                var mod = _getStrongboxMods(strongbox).ToList().Find(_shouldDraw);

                _graphics.DrawCircleInWorld(worldPosition, 700, StrongboxMods.GetColor(mod).ToDx(), 2);
                _graphics.DrawText(mod, screenPosition, FontAlign.Center);
            }

            bool _shouldDraw(string mod) => StrongboxMods.Contains(mod);
            string[] _getStrongboxMods(LabelOnGround labelOnGround) => labelOnGround?.Label?.GetChildAtIndex(0)?.GetChildAtIndex(1)?.GetChildAtIndex(3)?.GetText(512)?.Split('\n') ?? [];
        }
    }
}
