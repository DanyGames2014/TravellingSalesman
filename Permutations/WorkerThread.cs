namespace Permutations
{
    /// <summary>
    /// Worker for solving problem
    /// </summary>
    internal class WorkerThread
    {
        /// <summary>
        /// Pending Work
        /// </summary>
        private IReadOnlyList<int> pendingWork;

        /// <summary>
        /// Reference to the work manager
        /// </summary>
        private WorkManager workManager;

        /// <summary>
        /// If the work loop should run
        /// </summary>
        private bool run = true;

        /// <summary>
        /// Routes between cities
        /// </summary>
        private Route[] routes;

        /// <summary>
        /// City to start in
        /// </summary>
        private int startingCity;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="workManager">Work Manager Reference</param>
        public WorkerThread(WorkManager workManager)
        {
            this.workManager = workManager;
            routes = workManager.routes;
            this.startingCity = workManager.startingCity;
        }

        /// <summary>
        /// Start the Worker
        /// </summary>
        public void Start()
        {
            while (run)
            {
                pendingWork = workManager.getWork();
                if(pendingWork == null)
                {
                    run = false;
                    break;
                }
                Run();
            }
        }

        /// <summary>
        /// Runs a single pass
        /// </summary>
        private void Run()
        {
            List<int> trip = new List<int>() { startingCity };
            trip.AddRange(pendingWork);
            trip.Add(startingCity);

            int tripcost = 0;

            for (int i = 0; i < trip.Count - 1; i++)
            {
                var validroutes = routes.Where(x => x.from == trip[i] && x.to == trip[i + 1]);

                if (validroutes.Any())
                {
                    tripcost += validroutes.First().cost;
                }
                else
                {
                    continue;
                }
            }


            if (tripcost < workManager.bestResult.cost)
            {
                workManager.submitResult(trip, tripcost);
            }
        }
    }
}
