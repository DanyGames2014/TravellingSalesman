namespace TravellingSalesmanClient
{
    /// <summary>
    /// Class representing a route
    /// </summary>
    internal class Route
    {
        /// <summary>
        /// City 1
        /// </summary>
        public int from;
        /// <summary>
        /// City 2
        /// </summary>
        public int to;
        /// <summary>
        /// Cost of travel between them
        /// </summary>
        public int cost;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="from">City 1</param>
        /// <param name="to">City 2</param>
        /// <param name="cost">Cost</param>
        public Route(int from, int to, int cost)
        {
            this.from = from;
            this.to = to;
            this.cost = cost;
        }
    }
}
