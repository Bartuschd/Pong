using System.Net;
using System.Net.Sockets;

class NetworkHost
{
    int port = 7777;
    TcpListener tcpListener;
    TcpClient? connectedClient;

    public NetworkHost() {
        tcpListener = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
        tcpListener.Start();
    }

    public void Stop()
    {
        tcpListener.Stop();
    }

    public void CheckForClient()
    {
        if(connectedClient == null && tcpListener.Pending())
        {
            connectedClient = tcpListener.AcceptTcpClient();
            Console.WriteLine("Client ist verbunden!");
            return;
        }
    }
}