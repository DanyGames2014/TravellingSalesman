using System.Net.Sockets;

namespace TravellingSalesmanClient
{
    public class Client
    {
        // TCP Networking
        TcpClient tcpClient;
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        List<Packet> messageBuffer;

        // Args
        public string serverAddress;
        public int serverPort;
        public string inputFilePath;
        public string outputFilePath;

        // Search Params
        int startingCity = 1;
        public ulong shotCount;
        int[] cities = new int[] { };
        Route[] routes = new Route[] { };

        // Run
        public bool run = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostname">Hostname</param>
        /// <param name="serverPort">Server Port</param>
        /// <param name="input">File path with input data</param>
        /// <param name="output">File path to output data to</param>
        /// <param name="shotCount">Shot Count</param>
        public Client(string hostname = "127.0.0.1", int serverPort = 65525, string input = "input.csv", string output = "output.txt", ulong shotCount = 2500000)
        {
            messageBuffer = new List<Packet>();

            this.serverAddress = hostname;
            this.inputFilePath = input;
            this.outputFilePath = output;
            this.shotCount = shotCount;

            try
            {
                this.serverPort = Convert.ToInt32(serverPort);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Invalid Port");
                Environment.Exit(1);
            }
        }

        public void Send(Packet packet)
        {
            messageBuffer.Add(packet);

            SendBuffer();
        }

        public void SendBuffer()
        {
            if (tcpClient.Connected)
            {
                foreach (var item in messageBuffer)
                {
                    //Console.WriteLine("SENT : " + item.Serialize());
                    sw.WriteLine(item.Serialize());
                    sw.Flush();
                }
                messageBuffer.Clear();
            }
        }

        public void Run()
        {
            // Temp Lists to insert stuff read from config file
            List<int> citiesList = new List<int>();
            List<Route> routesList = new List<Route>();

            // Read Config File
            string[] a = null;

            try
            {
                a = File.ReadAllLines(inputFilePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.Error.WriteLine("File Not Found!");
                Console.ReadKey();
                Environment.Exit(1);
            }
            

            // Parse Config File
            for (int i = 0; i < a.Length; i++)
            {
                // Line 1 - Starting City
                if (i == 0)
                {
                    startingCity = Convert.ToInt32(a[i]);
                }

                // Line 2 - Colon Separated City Numbers
                else if (i == 1)
                {
                    var citiesLine = a[i].Split(",");
                    foreach (var item in citiesLine)
                    {
                        int tempCity = Convert.ToInt32(item);
                        if (!(tempCity == startingCity))
                        {
                            citiesList.Add(tempCity);
                        }
                    }

                }

                // Lines 3+ - Routes
                else
                {
                    var lineSplit = a[i].Split(",");
                    if (lineSplit.Length == 3)
                    {
                        Route route = new Route(
                            Convert.ToInt32(lineSplit[0]),
                            Convert.ToInt32(lineSplit[1]),
                            Convert.ToInt32(lineSplit[2])
                        );

                        routesList.Add(route);
                    }
                }
            }

            // Convert Lists to Arrays for Performance when they will be heavily accessed
            cities = citiesList.ToArray();
            routes = routesList.ToArray();

            // Work Manager
            WorkManager workManager = new(cities, routes, startingCity, shotCount);

            // When all the work is done, display a promp to press any key instead of exiting
            if (workManager.bestResult == null)
            {
                Console.WriteLine("No Result Found!");
                Environment.Exit(0);
            }


            Console.WriteLine("\n" + workManager.bestResult);
            StreamWriter outputFile = File.CreateText(outputFilePath);
            outputFile.WriteLine(workManager.bestResult.ToString());
            outputFile.Flush();
            outputFile.Dispose();

            Console.WriteLine("Sending Result");
            Packet result = new Packet(PacketType.SUBMIT_RESULT);
            result.addData("cost", workManager.bestResult.cost+"");
            result.addData("route", workManager.bestResult.RouteString());

            Connect(true);
            try
            {
                Thread.Sleep(1000);
                if (tcpClient.Connected)
                {
                    Send(result);
                }
                Thread.Sleep(1000);
            }
            catch (IOException ex)
            {
                while (!tcpClient.Connected)
                {
                    Console.WriteLine("Connection Fail, Retrying");
                    Thread.Sleep(5000);
                    Connect(false);
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        public void Connect(bool exitOnFail)
        {
            try
            {
                tcpClient = new();
                tcpClient.Connect(serverAddress, serverPort);
                ns = tcpClient.GetStream();
                sw = new StreamWriter(ns);
                sr = new StreamReader(ns);
                sw.AutoFlush = true;
            }
            catch (InvalidOperationException e)
            {
                tcpClient.Dispose();
                ns.Dispose();
                sw.Dispose();
                sr.Dispose();
                return;
            }

            catch (Exception e)
            {

                Console.Clear();
                Console.Error.WriteLine("Connection Failed!");
                Console.Error.WriteLine(e.Message);

                if (exitOnFail)
                {
                    Console.Error.WriteLine("Exiting Application!");
                    Environment.Exit(1);
                }

                return;
            }
        }
    }
}
