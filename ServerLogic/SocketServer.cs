using Godot;
using GSF.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.ServerLogic
{
    internal class SocketServer
    {

/*        private StreamPeerTcp _client; // the connected client

        public void Start(string address, int port)
        {
            GD.Print("Starting server at " + address + ":" + port);
            var ip = IPAddress.Parse(address); // optional if you want specific binding
            Error err = _server.Listen(port, ip.ToString());

            if (err != Error.Ok)
            {
                GD.PrintErr("Server failed to start: " + err);
            }
            else
            {
                GD.Print("Server started successfully.");
            }
        }

        public void Poll()
        {
            _server.Poll(); // Important to poll server

            if (_server.IsConnectionAvailable())
            {
                _client = _server.TakeConnection();
                GD.Print("Client connected!");
            }

            if (_client != null && _client.GetStatus() == StreamPeerTcp.Status.Connected)
            {
                if (_client.GetAvailableBytes() > 0)
                {
                    string received = _client.GetUtf8String(_client.GetAvailableBytes());
                    GD.Print("Received from client: ", received);
                }
            }
        }

        public void SendMessage(string message)
        {
            if (_client != null && _client.GetStatus() == StreamPeerTcp.Status.Connected)
            {
                _client.PutUtf8String(message);
            }
        }

        public bool HasClient()
        {
            return _client != null && _client.GetStatus() == StreamPeerTcp.Status.Connected;
        }*/
    }
}
