using Godot;
using Godot.Collections;
using NewGameProject.GameScenes.Levels;
using NewGameProject.ServerLogic;
using Pliant.Grammars;
using Pliant.Runtime;
using System;

public partial class SignalBus : Node
{

    public string LevelPath { get; set; } = string.Empty;
    public SocketServer SocketServer { get; set; }
    public IGrammar CurrentGrammar { get; set; }
    public GenericLevel CurrentLevel { get; set; }

    public string? ClientGuid = null;

    public static int TileSize = 16;
    public Vector2 LevelOrigin { get;    set; }
    public void SetCurrentLevel(GenericLevel level)
    {
        CurrentLevel = level;
    }
    [Signal]
    public delegate void MineTileEventHandler(Vector2I position, float amount);

    [Signal]
    public delegate void ProcessedCommandRecievedEventHandler(Dictionary data);

    [Signal]
    public delegate void SimulationEndedEventHandler();

    [Signal]
    public delegate void SimulationAbortedEventHandler();


    [Signal]
    public delegate void GridClickedEventHandler(Robot robotInfo, TileData tile, Vector2I gridPosition, Vector2 globalMousePosition, Vector2 localMousePosition);


    [Signal]
    public delegate void ToggleGridEventHandler();

    [Signal]
    public delegate void SocketCommandRecievedEventHandler(string data);

    [Signal]
    public delegate void BusReadyEventHandler();

    public override void _Ready()
    {

        EmitSignal(nameof(BusReady));
    }
    public void EmitToggleGrid()
    {
        EmitSignal(nameof(ToggleGrid));
    }

    public void SendSocketMessage(string message)
    {
        if (this.ClientGuid != null)
            this.SocketServer.SendMessageToClient(this.ClientGuid, message);
    }
}
