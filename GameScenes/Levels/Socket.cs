using Godot;
using System;

public partial class Socket : Node
{
    private StreamPeerTcp client = new StreamPeerTcp();
    private bool isConnected = false;

    public override void _Ready()
    {
        GD.Print("Connecting...");
        Error err = client.ConnectToHost("127.0.0.1", 9999);  
        if (err != Error.Ok)
        {
            GD.PrintErr("Connection failed: " + err);
        }
        else
        {
            GD.Print("Connected to server.");
            isConnected = true;
        }
    }

    public override void _Process(double delta)
    {
        if (isConnected)
        {

            string message = "Hello from Godot!";
            client.PutUtf8String(message);


            if (client.GetAvailableBytes() > 0)
            {
                string response = client.GetUtf8String(client.GetAvailableBytes());
                GD.Print("Received from server: " + response);


                client.PutUtf8String("Echo: " + response);
            }
        }
    }
}
