using Combinatorics.Collections;

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
        /// All Possible Trips
        /// </summary>
        Permutations<int> trips;

        /// <summary>
        /// Current best result
        /// </summary>
        public BestResult bestResult;

        /// <summary>
        /// Position in the permutation array
        /// </summary>
        private ulong pos = 0;

        /// <summary>
        /// Trip Enumerator
        /// </summary>
        IEnumerator<IReadOnlyList<int>> tripEnumerator;

        /// <summary>
        /// Constructor, initializes the work manager
        /// </summary>
        /// <param name="cities">ints representing the avalible cities</param>
        /// <param name="routes">Array of routes between the cities</param>
        /// <param name="startingCity">int of the city to start in</param>
        public WorkManager(int[] cities, Route[] routes, int startingCity)
        {
            // Cities
            this.cities = cities;

            // Random
            Random rnd = new Random();

            // All Possible Trips
            trips = new Permutations<int>(cities);

            // Routes
            this.routes = routes;

            // Trip Enumerator
            tripEnumerator = trips.GetEnumerator();

            // Starting City
            this.startingCity = startingCity;

            // Best Result
            bestResult = new BestResult(int.MaxValue, null);
        }

        /// <summary>
        /// Requests work
        /// </summary>
        /// <returns>List of permutations to go through</returns>
        public IReadOnlyList<int>? getWork()
        {
            lock (trips)
            {
                if (tripEnumerator.MoveNext()) 
                {
                    pos += 1;
                    return tripEnumerator.Current;
                }
                return null;
            }
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
    }
}
