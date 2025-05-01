using Godot;
using Pliant.Forest;
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


        var grammar = new PdlGrammarGenerator().Generate(definition);
        var parser = new ParseEngine(grammar);

        var calculatorInput = "5+30 * 2 + 1";
        var parseRunner = new ParseRunner(parser, calculatorInput);


        

        var recognized = false;
        var errorPosition = 0;
        while (!parseRunner.EndOfStream())
        {
            recognized = parseRunner.Read();
            if (!recognized)
            {
                errorPosition = parseRunner.Position;
                break;
            }
        }

        // For a parse to be accepted, all parse rules are completed and a trace
        // has been made back to the parse root.
        // A parse must be recognized in order for acceptance to have meaning.
        var accepted = false;
        if (recognized)
        {
            accepted = parseRunner.ParseEngine.IsAccepted();
            if (!accepted)
                errorPosition = parseRunner.Position;
        }
        Console.WriteLine($"Recognized: {recognized}, Accepted: {accepted}");
        if (!recognized || !accepted)
            Console.Error.WriteLine($"Error at position {errorPosition}");

        var parseForestRoot = parser.GetParseForestRootNode();


        var parseTree = new InternalTreeNode(
            parseForestRoot,
            new SelectFirstChildDisambiguationAlgorithm());
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
