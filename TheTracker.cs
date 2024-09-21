using ExileCore;
using ExileCore.PoEMemory.MemoryObjects;
using ImGuiNET;

namespace TheTracker;

public partial class TheTracker : BaseSettingsPlugin<TheTrackerSettings>
{
    private GroundEffectTracker _groundEffectTracker;
    private MonsterTracker _monsterTracker;
    private StrongboxTracker _strongboxTracker;
    private PlayerBuffsTracker _playerBuffsTracker;
    private MapModTracker _mapModsTracker;

    public override bool Initialise()
    {
        //Perform one-time initialization here

        //Maybe load you custom config (only do so if builtin settings are inadequate for the job)
        //var configPath = Path.Join(ConfigDirectory, "custom_config.txt");
        //if (File.Exists(configPath))
        //{
        //    var data = File.ReadAllText(configPath);
        //}

        _groundEffectTracker = new(GameController, Graphics, Settings);
        _monsterTracker = new(GameController, Graphics, Settings);
        _strongboxTracker = new(GameController, Graphics, Settings);
        _playerBuffsTracker = new(GameController, Settings);
        _mapModsTracker = new(GameController, Settings);

        return true;
    }

    public override void AreaChange(AreaInstance area)
    {
        //Perform once-per-zone processing here
        //For example, Radar builds the zone map texture here
    }

    public override Job Tick()
    {
        //Perform non-render-related work here, e.g. position calculation.
        //This method is still called on every frame, so to really gain
        //an advantage over just throwing everything in the Render method
        //you have to return a custom job, but this is a bit of an advanced technique
        //here's how, just in case:
        //return new Job($"{nameof(TheTracker)}MainJob", () =>
        //{
        //    var a = Math.Sqrt(7);
        //});

        //otherwise, just run your code here
        //var a = Math.Sqrt(7);
        return null;
    }

    public override void Render()
    {
        _groundEffectTracker.DrawGroundEffects();
        _monsterTracker.DrawMonsters();
        _strongboxTracker.DrawStrongboxMods();
        _playerBuffsTracker.DrawPlayerBuffs();
        _mapModsTracker.DrawMapMods();
    }

    public override void EntityAdded(Entity entity)
    {
        //If you have a reason to process every entity only once,
        //this is a good place to do so.
        //You may want to use a queue and run the actual
        //processing (if any) inside the Tick method.
    }

    public override void DrawSettings()
    {
        base.DrawSettings();

        ImGui.Text("Player Buffs");
        ImGui.InputTextMultiline("##PlayerBuffs", ref Settings.PlayerBuffs, 8000, new System.Numerics.Vector2(600, 100));
        ImGui.Text("Ground Effects");
        ImGui.InputTextMultiline("##GroundEffects", ref Settings.GroundEffects, 8000, new System.Numerics.Vector2(600, 100));
        ImGui.Text("Monsters");
        ImGui.InputTextMultiline("##Monsters", ref Settings.Monsters, 8000, new System.Numerics.Vector2(600, 100));
        ImGui.Text("Map Mods");
        ImGui.InputTextMultiline("##MapMods", ref Settings.MapMods, 8000, new System.Numerics.Vector2(600, 100));
        ImGui.Text("Strongbox Mods");
        ImGui.InputTextMultiline("##StrongboxMods", ref Settings.StrongboxMods, 8000, new System.Numerics.Vector2(600, 100));
    }
}