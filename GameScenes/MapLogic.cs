using Godot;
using Pliant.Forest;
using Pliant.Grammars;
using Pliant.Languages.Pdl;
using Pliant.Runtime;
using Pliant.Tree;
using System;
using System.IO;
public partial class MapLogic : Node2D
{

    private SignalBus _signalBus;

    private SocketClient _socketClient = new SocketClient();


    public override void _Ready()
	{
        _signalBus = GetNode<SignalBus>("/root/SignalBus");
        _socketClient.SignalBus = _signalBus;
        _socketClient.Connect("localhost", 4000);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "GameScenes","calculator.pdl");
        var content = File.ReadAllText(path);
        var pdlParser = new PdlParser();
        var definition = pdlParser.Parse(content);


        IGrammar grammar = new PdlGrammarGenerator().Generate(definition);

        _signalBus.CurrentGrammar = grammar;
        _signalBus.SocketClient = _socketClient;

       
    }

    public void ProcessInput(string code)
    {
/*        GD.Print("execution file passed ahahahaha loh");
        GD.Print(code);*/
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        _socketClient.Poll(); 

        if (_socketClient.IsConnected())
        {
/*            _socketClient.SendMessage("Hello from Godot!");*/
        }
    }

    

}
