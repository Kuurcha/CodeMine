using Godot;
using GSF.Console;
using System;

public class SocketClient
{
    private StreamPeerTcp _client = new StreamPeerTcp();
    private bool _isConnected = false;
    public SignalBus SignalBus { internal get; set; }
    public void Connect(string address, int port)
    {
        GD.Print("Connecting to " + address + ":" + port);
        Error err = _client.ConnectToHost(address, port);
        if (err != Error.Ok)
        {
            GD.PrintErr("Connection failed: " + err);
        }
        else
        {
            GD.Print("Connected to server.");
            _isConnected = true;
        }
    }

    public void SendMessage(string message)
    {
        if (_isConnected && _client.GetStatus() == StreamPeerTcp.Status.Connected)
        {
            _client.PutUtf8String(message);
        }
    }

    public void Poll()
    {
        if (!_isConnected)
            return;

        _client.Poll();
        var status = _client.GetStatus();
        if (status == StreamPeerTcp.Status.Connected)
        {

            if (_client.GetAvailableBytes() > 0)
            {
                string received = _client.GetUtf8String(_client.GetAvailableBytes());
                //check if valid command
                SignalBus.EmitSignal(nameof(SignalBus.CommandRecieved), received);
            }
        }
    }

    public bool IsConnected()
    {
        return _isConnected && _client.GetStatus() == StreamPeerTcp.Status.Connected;
    }
}

