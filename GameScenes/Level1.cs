using Godot;
using NewGameProject.Misc.Factory;
using System;
using System.Collections.Generic;

public partial class Level1 : Node2D
{
    // Called when the node enters the scene tree for the first time.
    private List<Robot> _robots = new List<Robot>();

    public override void _Ready()
    {
        List<RobotData> robotConfigs = new List<RobotData>
        {
            new RobotData(new Vector2(0, 0), "robot_1", 80, new string[] { "move left", "attack" }),
            new RobotData(new Vector2(20, 20), "robot_2", 100, new string[] { "move right", "wait" }),
            new RobotData(new Vector2(40, 40), "robot_3", 120, new string[] { "move up", "jump" })
        };

        // Use the factory to create robots for the level
        _robots = RobotFactory.CreateRobotsForLevel(this, robotConfigs);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
