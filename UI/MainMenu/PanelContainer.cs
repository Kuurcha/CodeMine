    using Godot;
using System;

public partial class PanelContainer : Godot.PanelContainer
{
    public override void _Ready()
    {
        // Create a VBoxContainer to organize buttons vertically
        var vbox = new VBoxContainer();
        vbox.SizeFlagsHorizontal = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
        vbox.SizeFlagsVertical = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
        vbox.Alignment = BoxContainer.AlignmentMode.Center;

        // Create and configure buttons
        var playButton = CreateButton("Play", "play");
        var settingsButton = CreateButton("Settings", "settings");
        var exitButton = CreateButton("Exit", "exit");

        // Connect button signals
        playButton.Pressed += OnPlayPressed;
        settingsButton.Pressed += OnSettingsPressed;
        exitButton.Pressed += OnExitPressed;

        // Add buttons to the VBoxContainer
        vbox.AddChild(playButton);
        vbox.AddChild(settingsButton);
        vbox.AddChild(exitButton);

        // Add VBoxContainer to the PanelContainer
        AddChild(vbox);
    }

    private Button CreateButton(string text, string name)
    {
        var button = new Button();
        button.Text = text;
        button.Name = name;
        button.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
        button.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
        button.CustomMinimumSize = new Vector2(200, 50);
        return button;
    }

    private void OnPlayPressed()
    {
        GD.Print("Play button pressed! Load game scene here.");
        GetTree().ChangeSceneToFile("res://UI/LevelSelection/LevelSelect.tscn");
    }

    private void OnSettingsPressed()
    {
        GD.Print("Settings button pressed! Open settings menu.");
        GetTree().ChangeSceneToFile("res://Scenes/SettingsScene.tscn");
    }

    private void OnExitPressed()
    {
        GD.Print("Exit button pressed! Quitting game.");
        GetTree().Quit();
    }
}



