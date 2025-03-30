using Godot;
using System;

public partial class GenericLevelUI : Control
{

    private SignalBus _signalBus;
    protected OtherUiElements _uiContainer;
    public Panel _infoPanel;
    protected Button _closeButton;
    protected Godot.Label _infoLabel;
    protected Godot.Label _robotInfoLabel;
    protected Godot.Label _tileInfoLabel;
    public override void _Ready()
    {
        var defaultTheme = ResourceLoader.Load<Theme>("res://UI/Themes/TestTheme.tres");
        _signalBus = GetNode<SignalBus>("/root/SignalBus");
        _signalBus.Connect("GridClicked", new Callable(this, nameof(DrawUi)));
        _uiContainer = GetNode<OtherUiElements>("OtherUIElements");


        _infoPanel = new Panel();
        _infoPanel.Size = new Vector2(150, 150);
        _infoPanel.ZIndex = 4000;
        _infoPanel.MouseFilter = Control.MouseFilterEnum.Pass;

        StyleBoxFlat styleBox = new StyleBoxFlat();
        styleBox.BorderWidthLeft = 2;
        styleBox.BorderWidthTop = 2;
        styleBox.BorderWidthRight = 2;
        styleBox.BorderWidthBottom = 2;
        styleBox.SetExpandMarginAll(10);
        styleBox.SetContentMarginAll(10);
        styleBox.BorderColor = new Color(1, 1, 1);

        _infoPanel.AddThemeStyleboxOverride("panel", styleBox);


        VBoxContainer vbox = new VBoxContainer();
        vbox.SizeFlagsHorizontal = SizeFlags.Expand;
        vbox.SizeFlagsVertical = SizeFlags.Expand;
        vbox.AddThemeConstantOverride("separation", 10);




        StyleBoxFlat styleBoxMargin = new StyleBoxFlat();
        styleBoxMargin.ExpandMarginTop= 20;
        styleBoxMargin.ExpandMarginTop = 20;
        styleBoxMargin.ExpandMarginRight = 20;
        styleBoxMargin.ExpandMarginBottom = 20;

        _infoLabel = new Godot.Label();
        _infoLabel.Text = "Test Label1";
        _infoLabel.AddThemeStyleboxOverride("panel", styleBoxMargin);
        _infoLabel.Theme = defaultTheme;
        vbox.AddChild(_infoLabel);

        _robotInfoLabel = new Godot.Label();
        _robotInfoLabel.Text = "Test Label";
        _robotInfoLabel.AddThemeStyleboxOverride("panel", styleBoxMargin);
        _robotInfoLabel.Theme = defaultTheme;
        vbox.AddChild(_robotInfoLabel);

        _tileInfoLabel = new Godot.Label();
        _tileInfoLabel.Text = "Test Label";
        _tileInfoLabel.Theme = defaultTheme;
        _tileInfoLabel.AddThemeStyleboxOverride("panel", styleBoxMargin);
        vbox.AddChild(_tileInfoLabel);

        _closeButton = new Button();
        _closeButton.Text = "Close";
        _closeButton.Theme = defaultTheme;

        _closeButton.Size = new Vector2(50, 50);
        _closeButton.Connect("pressed", Callable.From(OnCloseButtonPressed));
        _closeButton.AddThemeStyleboxOverride("panel", styleBoxMargin);
        _closeButton.AnchorLeft = 0.5f;
        _closeButton.AnchorRight = 0.5f;
        _closeButton.AnchorTop = 0.5f;
        _closeButton.AnchorBottom = 0.5f;
        vbox.AddChild(_closeButton);
        _infoPanel.AddChild(vbox);
        _uiContainer.AddChild(_infoPanel);
    }
    private void OnCloseButtonPressed()
    {
        _infoPanel.Hide();
        _infoPanel.
                        
        _uiContainer.UpdateElement();
    }


    private void UpdateUI(string robotInfo, string layerInfo, Vector2I gridPos, Vector2 mousePos)
    {

/*        string infoText = $"Grid Position: {gridPos}\n{robotInfo}\n{layerInfo}";*/


        //_infoLabel.Text = infoText;


/*        _infoPanel.Visible = true;*/

        _infoPanel.Position = mousePos;
        _tileInfoLabel.Text = layerInfo;
        _robotInfoLabel.Text = robotInfo;
        
/*        _infoLabel.QueueRedraw();
        _infoPanel.QueueRedraw();*/
    }


    public void DrawUi(Robot robotOnTile, TileData selectedTile, Vector2I gridPos, Vector2 globalMousePosition, Vector2 localMousePosition)
    {
        _infoPanel.Visible = true;

        string robotInfo = robotOnTile != null ? $"Robot: {robotOnTile.Name}" : "No robot on this tile.";
        bool breakable = selectedTile != null ?selectedTile.GetCustomData("Breakable").AsBool(): false;

        string tileData = breakable ? "Tile is breakable" : "Tile is not breakable";
        UpdateUI(robotInfo, tileData, gridPos, localMousePosition);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
