using Godot;
using System;

public partial class SideMenu : Control
{
    private TextEdit _codeInput;
    private Control _menuPanel;
    private Button _launchButton;

    public override void _Ready()
    {
        // Find the existing cog button
        var cogButton = GetNode<TextureButton>("TextureButton"); // Adjust if in a different path
        if (cogButton != null)
        {
            cogButton.Connect("pressed", Callable.From(ToggleMenu));
        }
        else
        {
            GD.PrintErr("TextureButton not found!");
            return;
        }

        // Create the menu panel (hidden by default)
        _menuPanel = new Control();
        _menuPanel.Visible = false;
        _menuPanel.SizeFlagsHorizontal = Control.SizeFlags.Expand;
        _menuPanel.SizeFlagsVertical = Control.SizeFlags.Expand;
        _menuPanel.Modulate = new Color(0, 0, 0, 0.8f); // Semi-transparent overlay
        AddChild(_menuPanel);

        // Create a container for the menu
        var panel = new Panel();
        panel.Size = new Vector2(400, 300);
        panel.Position = new Vector2(100, 100);
        panel.AddThemeStyleboxOverride("panel", new StyleBoxFlat { BgColor = new Color(0.15f, 0.15f, 0.15f) });
        _menuPanel.AddChild(panel);

        // Create a TextEdit box for entering code
        _codeInput = new TextEdit();
        _codeInput.Size = new Vector2(380, 200);
        _codeInput.Position = new Vector2(10, 10);
        _codeInput.WrapMode = TextEdit.LineWrappingMode.Boundary;
        panel.AddChild(_codeInput);

        // Create a Launch Button
        _launchButton = new Button();
        _launchButton.Text = "Launch";
        _launchButton.Position = new Vector2(10, 220);
        _launchButton.Size = new Vector2(380, 40);
        _launchButton.Connect("pressed", Callable.From(OnLaunchPressed));
        panel.AddChild(_launchButton);
    }

    private void ToggleMenu()
    {
        _menuPanel.Visible = !_menuPanel.Visible;
    }

    private void OnLaunchPressed()
    {
        string code = _codeInput.Text;
        GD.Print("Executing code:\n" + code);
        _menuPanel.Visible = false; // Hide menu after launching
    }
}
