namespace Permutations
{
    internal class WorkManager
    {
        /// <summary>
        /// ints representing cities
        /// </summary>
        public int[] cities;

        /// <summary>
        /// City to start in
        /// </summary>
        public int startingCity = 1;

        // Locks
        /// <summary>
        /// route array lock
        /// </summary>
        private object routeLock = new object();

        private Route[] _routes;
        /// <summary>
        /// Routes between cities
        /// </summary>
        public Route[] routes
        {
            get
            {
                lock (routeLock)
                {
                    return _routes;
                }
            }

            init
            {
                lock(routeLock)
                {
                    _routes = value;
                }
            }
        }

        /// <summary>
        /// Current best result
        /// </summary>
        public BestResult bestResult;

        /// <summary>
        /// Position in the permutation array
        /// </summary>
        private ulong pos = 0;

        /// <summary>
        /// Threads
        /// </summary>
        public List<Thread> threads = new List<Thread>();

        /// <summary>
        /// If the work manager should run
        /// </summary>
        public bool run = true;

        /// <summary>
        /// Random number generator
        /// </summary>
        private Random rnd;

        /// <summary>
        /// Amount of "shots" the program will take
        /// </summary>
        private ulong maximumShots;

        /// <summary>
        /// Constructor, initializes the work manager
        /// </summary>
        /// <param name="cities">ints representing the avalible cities</param>
        /// <param name="routes">Array of routes between the cities</param>
        /// <param name="startingCity">int of the city to start in</param>
        /// <param name="maximumShots">Amount of "shots" the program will take</param>
        public WorkManager(int[] cities, Route[] routes, int startingCity, ulong maximumShots)
        {
            // Cities
            this.cities = cities;

            // Random
            rnd = new Random();

            // Routes
            this.routes = routes;

            // Maximum shots
            this.maximumShots = maximumShots;

            // Starting City
            this.startingCity = startingCity;

            // Best Result
            bestResult = new BestResult(int.MaxValue, null);

            // Create threads
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var temp = new WorkerThread(this);
                threads.Add(new Thread(temp.Start));
            }

            // Strat the thread
            foreach (var item in threads)
            {
                item.Start();
            }

            // Block the main thread until a solution is found
            while (run)
            {
                Thread.Sleep(250);
            }
        }

        public void Stop()
        {
            run = false;
        }

        /// <summary>
        /// Requests work
        /// </summary>
        public IReadOnlyList<int>? getWork()
        {
            return randomTrip();
        }

        /// <summary>
        /// Submit a result
        /// </summary>
        /// <param name="trip">The list of cities</param>
        /// <param name="cost">Cost of the route</param>
        public void submitResult(List<int> trip, int cost)
        {
            if(cost < bestResult.cost)
            {
                bestResult.route = trip.ToArray();
                bestResult.cost = cost;
                Console.WriteLine("New " + bestResult);
            }
        }

        /// <summary>
        /// Generates a random trip
        /// </summary>
        public List<int> randomTrip()
        {
            if (pos >= maximumShots)
            {
                return null;
            }

            List<int> avalibleCities = new List<int>();
            avalibleCities = cities.ToList();

            List<int> trip = new List<int>();

            while(avalibleCities.Count > 0)
            {
                int num = rnd.Next(0, avalibleCities.Count);
                trip.Add(avalibleCities[num]);
                avalibleCities.RemoveAt(num);
            }

            pos++;

            return trip;
        }
    }
}
