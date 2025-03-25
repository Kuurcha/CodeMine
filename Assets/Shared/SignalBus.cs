using Godot;
using NewGameProject.GameScenes.Levels;
using System;

public partial class SignalBus : Node
{
    public string LevelPath { get; set; } = string.Empty;
    public GenericLevel CurrentLevel { get; set; }

    public void SetCurrentLevel(GenericLevel level)
    {
        CurrentLevel = level;
    }

    [Signal]
    public delegate void SimulationStartedEventHandler(string code);
}
