using Godot;
using NewGameProject.Helper;
using System;
using System.Collections.Generic;

public partial class SideMenu : Control
{
    private TextEdit _codeInput;
    private Control _menuPanel;
    private Button _launchButton;
    private string _path = "user://saved_text.txt";
    private SignalBus _signalBus;
    private Queue<string> commandQueue = new Queue<string>();

    [Export] 
    private float GameSpeed = 1.0f;
    public override void _Ready()
    {
        _signalBus = GetNode<SignalBus>("/root/SignalBus");
        var playButton = GetNode<TextureButton>("PlayButton"); 
        if (playButton != null)
        {
            playButton.Connect("pressed", Callable.From(OnPlayPressed));
        }
        else
        {
            GD.PrintErr("TextureButton not found!");
            return;
        }

        GD.Print("Loaded editor menu.");


        _menuPanel = new Control
        {
            Visible = true,
            SizeFlagsHorizontal = Control.SizeFlags.Expand,
            SizeFlagsVertical = Control.SizeFlags.Expand,
            Modulate = new Color(0.5f, 0.5f, 0.5f, 0.9f) 
        };
        AddChild(_menuPanel);

        // Create a container for the menu      
        var panel = new Panel
        {
            Size = new Vector2(367, 600),
            Position = new Vector2(0, 0)
        };


        var panelStyle = new StyleBoxFlat { BgColor = new Color(0.4f, 0.4f, 0.4f) }; 
        panel.AddThemeStyleboxOverride("panel", panelStyle);
        _menuPanel.AddChild(panel);

        // Create a TextEdit box for entering code
        _codeInput = new TextEdit
        {
            Size = new Vector2(350, 440),
            Position = new Vector2(10, 10),
            WrapMode = TextEdit.LineWrappingMode.Boundary
        };


        Theme textTheme = _codeInput.Theme ?? new Theme();
        textTheme.SetColor("font_color", "TextEdit", new Color(1, 1, 1));
        textTheme.SetColor("background_color", "TextEdit", new Color(0, 0, 0));
        textTheme.SetFontSize("Font Size", "TextEdit", 4);
        _codeInput.Theme = textTheme;
   

        panel.AddChild(_codeInput);

        // Create a Launch Button
        _launchButton = new Button
        {
            Text = "Save",
            Size = new Vector2(20, 20)
        };


        Theme buttonTheme = _launchButton.Theme ?? new Theme();
        buttonTheme.SetColor("font_color", "Button", new Color(1, 1, 1));
        buttonTheme.SetColor("font_color_pressed", "Button", new Color(1, 1, 1)); 
        buttonTheme.SetColor("bg_color", "Button", new Color(0, 0, 0));
        buttonTheme.SetFontSize("font_size", "Button", 10);
        _launchButton.Theme = buttonTheme;
        _launchButton.OffsetRight = 180;
        _launchButton.OffsetTop = 455;
        _launchButton.OffsetLeft = 180;
        _launchButton.OffsetBottom = 455;


        _launchButton.Connect("pressed", Callable.From(OnLaunchPressed));
        panel.AddChild(_launchButton);
    }


    private void OnLaunchPressed()
    {
        string code = _codeInput.Text;
        FileHelper.SaveText(code, _path);
    }

    private async void OnPlayPressed()
    {
        string executionFile = FileHelper.LoadText(_path);
        GD.Print("Execution file loaded.");


        string[] commands = executionFile.Split('\n');
        foreach (string command in commands)
        {
            _signalBus.EmitSignal(nameof(SignalBus.SimulationStarted), command);

            float delay = 1.0f / GameSpeed;
            await ToSignal(GetTree().CreateTimer(delay), "timeout");
        }


    }

/*    private async void ProcessNextCommand()
    {
        if (commandQueue.Count == 0)
        {
            GD.Print("All commands executed.");
            return;
        }

        string command = commandQueue.Dequeue();

    
        _signalBus.EmitSignal("SimulationStep", command);
        ProcessNextCommand();
    }
*/
}
