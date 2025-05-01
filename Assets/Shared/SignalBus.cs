using Godot;
using NewGameProject.GameScenes.Levels;
using Pliant.Grammars;
using Pliant.Runtime;
using System;

public partial class SignalBus : Node
{
    public string LevelPath { get; set; } = string.Empty;

    public IGrammar CurrentGrammar { get; set; }
    public GenericLevel CurrentLevel { get; set; }

    public static int TileSize = 16;
    public Vector2 LevelOrigin { get;    set; }
    public void SetCurrentLevel(GenericLevel level)
    {
        CurrentLevel = level;
    }

    [Signal]
    public delegate void CommandRecievedEventHandler(string code);

    [Signal]
    public delegate void SimulationEndedEventHandler();

    [Signal]
    public delegate void SimulationAbortedEventHandler();


    [Signal]
    public delegate void GridClickedEventHandler(Robot robotInfo, TileData tile, Vector2I gridPosition, Vector2 globalMousePosition, Vector2 localMousePosition);


    [Signal]
    public delegate void ToggleGridEventHandler();

    public void EmitToggleGrid()
    {
        EmitSignal(nameof(ToggleGrid));
    }
}
