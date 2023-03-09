namespace TravellingSalesmanClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Neko Salesman v4\n");
                Console.WriteLine("Usage : " + Environment.ProcessPath.Split(@"\").Last() + " [options]");
                Console.WriteLine("\nOptions : ");
                Console.WriteLine(" -a | -address <ip address>   Address of server to connect to.");
                Console.WriteLine(" -p | -port <port>            Specifies the port of the server, has no effect if -a isn't specified");
                Console.WriteLine(" -i | -in <filePath>          Specifies the input file path");
                Console.WriteLine(" -o | -out <filePath>         Specifies the output file path");
                Console.WriteLine(" -s | -shots <number>         Specifies the number of shots the program wil take");
            }
            else
            {
                Client client = new Client();

                try
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i][0].Equals('-'))
                        {
                            switch (args[i].Remove(0, 1))
                            {
                                case "a":
                                case "address":
                                    //Console.WriteLine("ADDRESS : " + args[i + 1]);
                                    client.serverAddress = args[i + 1];
                                    i++;
                                    break;

                                case "p":
                                case "port":
                                    //Console.WriteLine("PORT : " + args[i + 1]);
                                    client.serverPort = Convert.ToInt32(args[i + 1]);
                                    i++;
                                    break;

                                case "i":
                                case "in":
                                case "input":
                                    client.inputFilePath = args[i + 1];
                                    break;

                                case "o":
                                case "out":
                                case "output":
                                    client.outputFilePath = args[i + 1];
                                    break;

                                case "s":
                                case "shots":
                                    ulong shotCount = 0;
                                    ulong argParsed = args.Length > 2 ? Util.ConvertToUInt64orZero(args[i + 1]) : 0;
                                    shotCount = argParsed != 0 ? argParsed : 25000000;
                                    client.shotCount = shotCount;
                                    break;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                client.Run();

            }


        }
    }
}