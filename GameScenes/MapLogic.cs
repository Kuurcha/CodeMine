using Godot;
using System;
public partial class MapLogic : Node2D
{

    private SignalBus _signalBus;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _signalBus = GetNode<SignalBus>("/root/SignalBus");
        _signalBus.SimulationStarted += ProcessInput;
        //CenterScene();
    }

    public void ProcessInput(string code)
    {
        GD.Print("execution file passed ahahahaha loh");
        GD.Print(code);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    

}
