using Godot;
using NewGameProject.GameScenes.Levels;
using NewGameProject.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class SideMenu : Control
{
    public List<Control> uiItems = new List<Control>();
    private TextEdit _codeInput;
    private Control _menuPanel;
    private Button _launchButton;
    private string _path = "user://saved_text.txt";
    private SignalBus _signalBus;
    private Queue<string> commandQueue = new Queue<string>();
    private GenericLevel currentLevel;
    private bool _simulationStarted = false;
    [Export] 
    private float GameSpeed = 1.0f;
    public override void _Ready()
    {
        _signalBus = GetNode<SignalBus>("/root/SignalBus");
        var playButton = GetNode<TextureButton>("PlayButton"); 
        
        if (playButton != null)
        {
            playButton.Connect("pressed", Callable.From(OnPlayPressed));
            uiItems.Add(playButton);
        }
        else
        {
            GD.PrintErr("TextureButton not found!");
            return;
        }

        var restartSimulation = GetNode<TextureButton>("RestartSimulation");
        if (restartSimulation != null)
        {
            restartSimulation.Connect("pressed", Callable.From(OnResetPressed));
            uiItems.Add(restartSimulation);
        }
        else
        {   
            GD.PrintErr("RestartSimulation not found!");
            return;
        }


        var hideGridButton = GetNode<TextureButton>("HideGrid");
        if (hideGridButton != null)
        {
            hideGridButton.Connect("pressed", Callable.From(OnHidePressed));
            uiItems.Add(hideGridButton);
        }
        else
        {
            GD.PrintErr("RestartSimulation not found!");
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
        uiItems.Add(_menuPanel);
       
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

        _codeInput.Text = FileHelper.LoadText(_path);

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

    private void testHideThis()
    {
        GD.Print("Test hiding!");
        this.Hide();
    }
    private void OnLaunchPressed()
    {
        string code = _codeInput.Text;
        FileHelper.SaveText(code, _path);
    }


    private void SimulationEnded()
    {
        _simulationStarted = false;
    }
    private async void OnPlayPressed()
    {
        await Reset();
        if (!_simulationStarted)
        {
            _simulationStarted = true;
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

    }

    private async Task Reset()
    {
        if (_signalBus.CurrentLevel != null)
        {
            _signalBus.EmitSignal(nameof(SignalBus.SimulationAborted));
            SimulationEnded();
            await _signalBus.CurrentLevel.ResetLevel();
        }
        else
        {
            GD.Print("Level not loaded");
        }

    }
    private async void OnResetPressed()
    {
        await Reset();
    }

    private void OnHidePressed()
    {
        _signalBus.EmitToggleGrid();    
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
