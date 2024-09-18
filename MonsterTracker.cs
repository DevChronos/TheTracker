using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System.Linq;

namespace TheTracker
{
    public class MonsterTracker(GameController gameController, Graphics graphics, TheTrackerSettings settings)
    {
        private readonly GameController _gameController = gameController;
        private readonly Graphics _graphics = graphics;
        private readonly TheTrackerSettings _settings = settings;

        private Camera Camera => _gameController.Game.IngameState.Camera;
        private string[] Monsters => _settings.Monsters.Split('\n');

        public void DrawMonsters()
        {
            var monsters = _gameController.EntityListWrapper.OnlyValidEntities.Where(_shouldDraw);

            foreach (var monster in monsters)
            {
                var worldPosition = _gameController.IngameState.Data.ToWorldWithTerrainHeight(monster.GridPosNum);
                var screenPosition = Camera.WorldToScreen(worldPosition);

                _graphics.DrawCircleInWorld(worldPosition, GetSize(monster), Monsters.GetColor(monster.Metadata).ToDx(), 2);
                _graphics.DrawText(monster.RenderName, screenPosition, FontAlign.Center);
            }

            bool _shouldDraw(Entity entity)
            {
                if (!Monsters.Contains(entity.Metadata)) return false;
                if (entity.Metadata == "Metadata/Monsters/Daemon/DaemonElderUnstableDoomCirclesSmall" && !entity.IsValid) return false;
                if (entity.Metadata == "Metadata/Monsters/AtlasInvaders/ConsumeMonsters/ConsumeBossStalkerOrbUberMaps__" && !entity.IsAlive) return false;
                if (entity.Metadata == "Metadata/Monsters/LightningOrb/ElderTentacleFiendUberMaps" && !entity.IsAlive) return false;

                if (entity.Metadata == "Metadata/Monsters/Daemon/UberMapExarchDaemon")
                {
                    var stateMachineComponent = entity.GetComponent<StateMachine>();
                    var stateValue = stateMachineComponent?.States?.FirstOrDefault()?.Value ?? 0;
                    if (stateValue > 1) return false;
                }

                return true;
            }
        }

        private static float GetSize(Entity entity)
        {
            if (entity.Metadata == "Metadata/Monsters/Daemon/UberMapExarchDaemon") return 200;
            if (entity.Metadata == "Metadata/Monsters/Daemon/DaemonElderUnstableDoomCirclesSmall") return 700;
            if (entity.Metadata.StartsWith("Metadata/Monsters/Daemons/BloodlinesBearerSelfBeaconDaemon")) return 700;

            var renderComponent = entity.GetComponent<Render>();
            if (renderComponent != null && renderComponent.BoundsNum.X > 50 && renderComponent.BoundsNum.X < 10000) return renderComponent.BoundsNum.X;

            var positionedComponent = entity.GetComponent<Positioned>();
            if (positionedComponent != null && positionedComponent.Size > 50 && positionedComponent.Size < 10000) return positionedComponent.Size;

            return 50;
        }
    }
}

/*
Metadata/Monsters/Daemon/UberMapExarchDaemon // Searing Rune
Metadata/Monsters/Statue/PetrificationStatue // Petrification Statue
Metadata/Monsters/LeagueAffliction/Volatile/AfflictionVolatile", // Delirium Volatile Core
Metadata/Monsters/VolatileCore/VolatileCoreUberMap // Volatile Core From T17
Metadata/Monsters/LightningOrb/ElderTentacleFiendUberMaps // Tentacle Fiend
Metadata/Monsters/Daemon/DaemonElderUnstableDoomCirclesSmall // Tentacle Fiend Circle
Metadata/Monsters/Daemons/BloodlinesBearerSelfBeaconDaemonLightning // Lightning Explosion
Metadata/Monsters/Daemons/BloodlinesBearerSelfBeaconDaemonCold // Cold Explosion
Metadata/Monsters/Daemons/BloodlinesBearerSelfBeaconDaemonFire // Fire Explosion
Metadata/Monsters/AtlasInvaders/ConsumeMonsters/ConsumeBossStalkerOrbUberMaps__ // Drowning Orb

Metadata/Monsters/Daemons/LightningCloneRetaliationDaemon",
Metadata/Monsters/Clone/MarauderLightningClone",
*/