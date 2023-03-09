using TravellingSalesmanServer.Networking;

namespace TravellingSalesmanServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
        }
    }
}