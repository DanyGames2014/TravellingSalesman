namespace TravellingSalesmanClient
{
    public enum PacketType
    {
        // Unknown Packet Type, usually used when error handling
        // No Data Stored
        UNKNOWN = 0,

        // Packet that submits a result
        SUBMIT_RESULT = 1
    }
}
