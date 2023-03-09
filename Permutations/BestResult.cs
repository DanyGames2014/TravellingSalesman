using System.Text;

namespace Permutations
{
    /// <summary>
    /// Represents a best found route
    /// </summary>
    internal class BestResult
    {
        /// <summary>
        /// Cost of the route
        /// </summary>
        public int cost;
        /// <summary>
        /// Array of ints representing the route through cities
        /// </summary>
        public int[] route;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cost">cost</param>
        /// <param name="route">route</param>
        public BestResult(int cost, int[] route)
        {
            this.cost = cost;
            this.route = route;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String that represents the best route</returns>
        public override string? ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Best Route With Cost " + cost);
            sb.Append(" [");
            for (int i = 0; i < route.Length-1; i++)
            {
                sb.Append(route[i].ToString()+",");
            }
            sb.Append((route[route.Length-1] + "]").ToString());

            return sb.ToString();
        }
    }
}
