using Permutations;

namespace PermutationTesting
{
    internal struct Program
    {
        static void Main(string[] args)
        {
            // Search Parameters
            int startingCity = 1;
            int[] cities = new int[] { };
            Route[] routes = new Route[] { };
            ulong shotCount = 0;
            ulong arg2parsed = args.Length > 2 ? Util.ConvertToUInt64orZero(args[2]) : 0;
            shotCount = arg2parsed != 0 ? arg2parsed : 25000000;

            // Temp Lists to insert stuff read from config file
            List<int> citiesList = new List<int>();
            List<Route> routesList = new List<Route>();

            // Read Config File
            string inputFilePath = (args.Length>0 ? args[0] : null) ?? "input.csv";
            string outputFilePath = (args.Length > 1 ? args[1] : null) ?? "output.txt";
            string[] a = File.ReadAllLines(inputFilePath);

            // Parse Config File
            for (int i = 0; i < a.Length; i++)
            {
                // Line 1 - Starting City
                if(i == 0)
                {
                    startingCity = Convert.ToInt32(a[i]);
                }

                // Line 2 - Colon Separated City Numbers
                else if(i == 1)
                {
                    var citiesLine = a[i].Split(",");
                    foreach (var item in citiesLine)
                    {
                        int tempCity = Convert.ToInt32(item);
                        if(!(tempCity == startingCity))
                        {
                            citiesList.Add(tempCity);
                        }
                    }

                }

                // Lines 3+ - Routes
                else
                {
                    var lineSplit = a[i].Split(",");
                    if(lineSplit.Length == 3)
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
            if(workManager.bestResult == null)
            {
                Console.WriteLine("No Result Found!");
                Console.WriteLine("DONE! Press Any Key to Continue");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Console.WriteLine("\n" + workManager.bestResult);
            StreamWriter outputFile = File.CreateText(outputFilePath);
            outputFile.WriteLine(workManager.bestResult.ToString());
            outputFile.Flush();
            outputFile.Dispose();
            Console.WriteLine("DONE! Press Any Key to Continue");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}