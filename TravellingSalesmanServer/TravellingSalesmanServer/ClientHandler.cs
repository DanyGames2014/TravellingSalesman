using TravellingSalesmanServer.Networking;
using System.Diagnostics;
using System.Net.Sockets;
using TravellingSalesmanClient;

namespace TravellingSalesmanServer
{
    public class ClientHandler
    {
        // References to other modules
        private Server server;
        private Logger logger;

        // TCP Client
        public TcpClient client;
        private NetworkStream ns;
        private StreamWriter sw;
        private StreamReader sr;

        // Signalization
        public bool cleanable = true;
        public bool run = true;

        // State
        Stopwatch timeout;

        // Buffers
        public List<string> outgoingBuffer;
        public List<string> incomingBuffer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tcpClient">TcpClient to handle</param>
        /// <param name="server">Reference to the server</param>
        public ClientHandler(TcpClient tcpClient, Server server)
        {
            // Assign References to Server and Logger and chat Manager
            this.server = server;
            logger = server.logger;

            // TCP Networking
            client = tcpClient;
            ns = client.GetStream();
            sw = new(ns);
            sw.AutoFlush = true;
            sr = new(ns);

            // Create the stopwatch for timeout tracking
            timeout = new Stopwatch();

            // By default the handler is not cleanable
            cleanable = false;

            // Initialize Buffer
            outgoingBuffer = new List<string>();
            incomingBuffer = new List<string>();
        }

        /// <summary>
        /// Adds a packet to the outgoing buffer
        /// </summary>
        /// <param name="packet"></param>
        public void Send(Packet packet)
        {
            outgoingBuffer.Add(packet.Serialize());
            SendBuffer();
        }

        /// <summary>
        /// Adds a string message to the buffer, used for dumb terminals
        /// </summary>
        /// <param name="message">string to add</param>
        public void Send(string message)
        {
            outgoingBuffer.Add(message);
            SendBuffer();
        }

        /// <summary>
        /// Tries to send all messages from the buffer
        /// </summary>
        public void SendBuffer()
        {
            foreach (var item in outgoingBuffer)
            {
                sw.WriteLine(item);
                logger.WriteLine("SENT : " + item, LogLevel.NET_DEBUG);
            }
            outgoingBuffer.Clear();
        }

        /// <summary>
        /// Tries to receive avalible data from the client to the incoming buffer
        /// </summary>
        public void Receive()
        {
            while(ns.DataAvailable)
            {
                string msg = sr.ReadLine()+"";
                logger.WriteLine("RECEIVED : " + msg,  LogLevel.NET_DEBUG);
                if(msg.Length > 0)
                {
                    incomingBuffer.Add(msg);
                }
            }
        }

        /// <summary>
        /// Reads data from the incoming buffer
        /// </summary>
        /// <param name="destructive">if to delete the data from the buffer after it has been read, defaults to true.</param>
        /// <returns>Read Data</returns>
        public string ReadBuffer(bool destructive = true)
        {
            logger.WriteDebug("Incoming Buffer Size : " + incomingBuffer.Count);
            if (BufferAvalible())
            {
                string msg = incomingBuffer[0];
                if(destructive)
                {
                    incomingBuffer.Remove(msg);
                }
                return msg;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns if there is data avalible to read
        /// </summary>
        /// <returns></returns>
        public bool BufferAvalible() => (incomingBuffer.Count > 0);

        /// <summary>
        /// Cleans the buffer of potentially invalid messages
        /// </summary>
        public void CleanBuffer()
        {
            for (int i = 0; i < incomingBuffer.Count; i++)
            {
                if (incomingBuffer[i].Length == 0)
                {
                    incomingBuffer.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// The main method that handles data from client
        /// </summary>
        public void Run()
        {
            if (timeout.ElapsedMilliseconds < 60000)
            {
                try
                {
                    Receive();

                    if (BufferAvalible())
                    {
                        using (Packet packet = Packet.Deserialize(ReadBuffer()))
                        {
                            switch (packet.packetType)
                            {
                                case PacketType.SUBMIT_RESULT:
                                    try
                                    {
                                        if (server.BestResult.cost > Convert.ToInt32(packet.getData("cost")))
                                        {
                                            server.BestResult.cost = Convert.ToInt32(packet.getData("cost"));
                                            server.BestResult.route = packet.getData("route");
                                            logger.WriteInfo("Found new best route !");
                                            logger.WriteInfo($"{server.BestResult}");
                                        }
                                    }
                                    catch (Exception)
                                    {

                                        throw;
                                    }

                                    break;
                            }
                        }
                    }
                }
                catch (IOException e)
                {
                    logger.WriteError("Connection error with client", e);
                    cleanable = true;
                }
                catch (Exception e)
                {
                    logger.WriteError("Error", e);
                }
            }
            else
            {
                logger.WriteDebug("Ending connection with client");
                cleanable = true;
            }
        }
    }
}
