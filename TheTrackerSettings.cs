﻿using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace TheTracker;

public class TheTrackerSettings : ISettings
{
    //Mandatory setting to allow enabling/disabling your plugin
    public ToggleNode Enable { get; set; } = new ToggleNode(false);
    public ToggleNode MoveWindow { get; set; } = new ToggleNode(false);

    //Put all your settings here if you can.
    //There's a bunch of ready-made setting nodes,
    //nested menu support and even custom callbacks are supported.
    //If you want to override DrawSettings instead, you better have a very good reason.
    public string GroundEffects = string.Empty;
    public string Monsters = string.Empty;
    public string PlayerBuffs = string.Empty;
    public string MapMods = string.Empty;
    public string StrongboxMods = string.Empty;
}