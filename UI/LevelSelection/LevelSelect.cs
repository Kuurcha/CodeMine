using Godot;
using System;
using System.Collections.Generic;

public partial class LevelSelect :   Godot.PanelContainer
{
    private Dictionary<string, string> levelDictionary = new Dictionary<string, string>
    {
        { "Level 1", "res://GameScenes/Map.tscn" },
        { "Level 2", "res://Scenes/Level2.tscn" },
        { "Level 3", "res://Scenes/Level3.tscn" },
        { "Level 4", "res://Scenes/Level4.tscn" },
        { "Level 5", "res://Scenes/Level5.tscn" }
    };

    public override void _Ready()
    {

        var vbox = new VBoxContainer();
        vbox.SizeFlagsHorizontal = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
        vbox.SizeFlagsVertical = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
        vbox.Alignment = BoxContainer.AlignmentMode.Center;


        var grid = new GridContainer();
        grid.Columns = Mathf.CeilToInt(Mathf.Sqrt(levelDictionary.Count));
        grid.SizeFlagsHorizontal = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
        grid.SizeFlagsVertical = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
        grid.CustomMinimumSize = new Vector2(600, 400);
        vbox.AddChild(grid);


        foreach (var level in levelDictionary)
        {
            var levelButton = CreateButton(level.Key, () => LoadLevel(level.Value));
            grid.AddChild(levelButton);
        }


        var backButton = CreateButton("Back", OnBackPressed);
        vbox.AddChild(backButton);


        AddChild(vbox);
    }

    private Button CreateButton(string text, Action onPressedAction)
    {
        var button = new Button();
        button.Text = text;
        button.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
        button.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
        button.CustomMinimumSize = new Vector2(200, 50);
        button.Pressed += () => onPressedAction.Invoke();
        return button;
    }

    private void LoadLevel(string levelPath)
    {
        GD.Print($"Loading {levelPath}");
        GetTree().ChangeSceneToFile(levelPath);
    }

    private void OnBackPressed()
    {
        GD.Print("Returning to Main Menu...");
        GetTree().ChangeSceneToFile("res://UI/MainMenu/MainMenu.tscn");
    }
}




