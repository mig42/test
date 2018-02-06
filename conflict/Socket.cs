using System.Net;

namespace Network
{
    internal class ServerSocket
    {
        internal int Recv(byte[] buffer)
        {
            System.IO.Read(buffer);
        }

        internal void Listen()
        {
            // do the listen on a port
            // and whatever it is needed
            // to listen
        }
    }

    internal class DNS
    {
        internal string GetHostByName(string addr)
        {
            // this method returns the host
            // when you give an addr
            return CalculateHostByName(addr);
        }
    }

    internal class ClientSocket
    {

        internal int Send(byte[] buffer)
        {
            System.IO.Write(buffer);
        }

        internal void ConnectTo(string addr)
        {
            // connect to a client
            Net.ConnectTo(addr);
        }
    }
}
