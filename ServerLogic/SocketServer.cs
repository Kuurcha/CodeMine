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
    public partial class SocketServer : Node
    {

        public class ClientConnection
        {
            public StreamPeerTls TlsStream { get; set; }
            public string ClientId { get; set; }  
        }

        private Godot.TcpServer _server = new Godot.TcpServer();
        private List<ClientConnection> _clinets = new();

        private bool _isListening = false;


        private X509Certificate _certificate;
        private CryptoKey _privateKey = new CryptoKey();

        private SignalBus _signalBus;
        public override void _Ready()
        {
            _signalBus =  GetNode<SignalBus>("/root/SignalBus");
            _certificate = new X509Certificate();
            var certLoadResult = _certificate.Load("res://cert//cert.pem");
            if (certLoadResult != Error.Ok)
            {
                GD.PrintErr("Failed to load certificate");
            }

            var keyLoadResult = _privateKey.Load("res://cert//key.pem");
            if (keyLoadResult != Error.Ok)
            {
                GD.PrintErr("Failed to load private key");
            }
        }
        public void StartServer(string address = "0.0.0.0", ushort port = 5000)
        {
                
            var err = _server.Listen(port, address);
            if (err != Error.Ok)
            {
                GD.PrintErr("Failed to start TCP server: " + err);
                return;
            }

            _isListening = true;
            GD.Print($"TCP server listening on {address}:{port}");
        }

        public override void _Process(double delta)
        {
            if (!_isListening)
                return;

    
            if (_server.IsConnectionAvailable())
            {
                var peerConnection = _server.TakeConnection();
                var tls = new StreamPeerTls();
                var tls_options = TlsOptions.Server(_privateKey, _certificate);

                try
                {
                    tls.AcceptStream(peerConnection, tls_options);
                    var client = new ClientConnection
                    {
                        TlsStream = tls,
                        ClientId = Guid.NewGuid().ToString()
                    };
                    _signalBus.ClientGuid = client.ClientId;
                    _clinets.Add(client);

                    GD.Print("Client connected with TLS");
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"TLS AcceptStream failed: {ex.Message}");
                    GD.PrintErr(ex.StackTrace); 
                }
            }

            for (int i = _clinets.Count - 1; i >= 0; i--)
            {
                var client = _clinets[i];
                client.TlsStream.Poll();
                var status = client.TlsStream.GetStatus();
   

                switch (status)
                {
                    case StreamPeerTls.Status.Handshaking:
                        GD.Print($"Client {client.ClientId} TLS handshake in progress...");
                        break;

                    case StreamPeerTls.Status.Connected:
                        GD.Print($"Client {client.ClientId} TLS handshake completed!");
                        if (client.TlsStream.GetAvailableBytes() > 0 || client.TlsStream.GetStream().GetAvailableBytes() > 0)
                        {
                            string msg = client.TlsStream.GetUtf8String(client.TlsStream.GetAvailableBytes());
                            if (msg == "HELLO_GODOT")
                            {
                                GD.Print("Handshake successful with client: " + client.ClientId);
                                client.TlsStream.PutUtf8String("HELLO_CLIENT"); // Respond to confirm handshake
                            }
                            GD.Print("Received from client: " + msg);
                            _signalBus?.EmitSignal(nameof(SignalBus.SocketCommandRecieved), msg);
                        }
                        break;

                    case StreamPeerTls.Status.Error:
                        GD.PrintErr($"Client {client.ClientId} TLS handshake failed!");
                        // Handle cleanup or disconnect client
                        break;
                    case StreamPeerTls.Status.Disconnected:
                        _signalBus.ClientGuid = null;
                        GD.Print("Client disconnected");
                        _clinets.RemoveAt(i);
                        break;
                    default:
                        _signalBus.ClientGuid = null;
                        GD.Print("Client disconnected");
                        _clinets.RemoveAt(i);
                        GD.Print($"Client {client.ClientId} TLS status: {status}");
                        break;
                }


            }

        }

        public void SendMessageToClient(string clientId, string message)
        {
            var client =  _clinets.FirstOrDefault(c => c.ClientId == clientId);
            if (client != null && client.TlsStream.GetStatus() == StreamPeerTls.Status.Connected)
            {
                client.TlsStream.PutUtf8String(message);
                
            }
        }
    }
}
