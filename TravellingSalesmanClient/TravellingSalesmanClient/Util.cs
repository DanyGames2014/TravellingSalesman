namespace TravellingSalesmanClient
{
    internal class Util
    {
        internal static ulong ConvertToUInt64orZero(string value)
        {
			try
			{
				return Convert.ToUInt64(value);
			}
			catch (Exception)
			{
				return 0;
			}
        }
    }
}
