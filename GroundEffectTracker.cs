using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System.Linq;

namespace TheTracker
{
    public class GroundEffectTracker(GameController gameController, Graphics graphics, TheTrackerSettings settings)
    {
        private readonly GameController _gameController = gameController;
        private readonly Graphics _graphics = graphics;
        private readonly TheTrackerSettings _settings = settings;

        private Camera Camera => _gameController.Game.IngameState.Camera;
        private string[] GroundEffects => _settings.GroundEffects.Split('\n');

        public void DrawGroundEffects()
        {
            var groundEffects = _gameController.EntityListWrapper.ValidEntitiesByType[EntityType.Effect].Where(_shouldDraw);

            foreach (var groundEffect in groundEffects)
            {
                var positionedComponent = groundEffect.GetComponent<Positioned>();
                if (positionedComponent is null) continue;

                var worldPosition = _gameController.IngameState.Data.ToWorldWithTerrainHeight(positionedComponent.GridPosition);
                var screenPosition = Camera.WorldToScreen(worldPosition);
                var buffName = groundEffect.Buffs.Find(buff => GroundEffects.Contains(buff.DisplayName)).DisplayName;

                _graphics.DrawCircleInWorld(worldPosition, positionedComponent.Size, GroundEffects.GetColor(buffName).ToDx(), 2);
                _graphics.DrawText(buffName, screenPosition, FontAlign.Center);
            }

            bool _shouldDraw(Entity entity)
            {
                return entity.Buffs != null
                    && entity.Buffs.Exists(buff => GroundEffects.Contains(buff.DisplayName))
                    && entity.IsHostile
                    && entity.Path != null
                    && entity.Path.Contains("ground_effects");
            }
        }
    }
}
