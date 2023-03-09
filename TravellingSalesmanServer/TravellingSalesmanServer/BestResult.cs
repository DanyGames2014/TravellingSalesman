using System.Text;

namespace TravellingSalesmanClient
{
    /// <summary>
    /// Represents a best found route
    /// </summary>
    public class BestResult
    {
        /// <summary>
        /// Cost of the route
        /// </summary>
        public int cost;

        /// <summary>
        /// String representing the best route
        /// </summary>
        public string route;

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String that represents the best route</returns>
        public override string? ToString()
        {
            return "Best Route With Cost " + cost + " " + route;
        }

        public string? RouteString()
        {
            return route;
        }
    }
}
