using System.Net;
using System.Net.Sockets;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public class NetRepository
    {
        public void Connect(string ip, int port)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(iPEndPoint);

            socket.Listen(1);
        }
    }
}