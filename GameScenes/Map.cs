using Godot;
using NewGameProject.GameScenes.Levels;
using System;

public partial class Map : Node2D
{

	public override void _Ready()
	{
		GD.Print("MAP INIT");

        SignalBus _signalBus = GetNode<SignalBus>("/root/SignalBus");
        string levelPath = _signalBus.LevelPath;
        if (!string.IsNullOrEmpty(levelPath))
        {

            PackedScene levelScene = GD.Load<PackedScene>(levelPath);

            if (levelScene != null)
            {

                Node levelInstance = levelScene.Instantiate();

                AddChild(levelInstance);

                if (levelInstance is GenericLevel genericLevel)
                {
                    _signalBus.CurrentLevel = genericLevel;
                    GD.Print($"Level {levelPath} loaded and stored in SignalBus.");
                }
                else
                {
                    GD.PrintErr($"Loaded scene {levelPath} is not a GenericLevel.");
                }

                GD.Print($"Level scene {levelPath} added as a child to Map node.");
            }
            else
            {
                GD.PrintErr($"Failed to load scene at {levelPath}");
            }
        }
        else
        {
            GD.PrintErr("No LevelPath set in SignalBus.");
        }

    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
