namespace NetworkPrimitives
{
    internal static class CharExtensions
    {
        public static bool IsHex(this char ch) => ch switch
        {
            >= '0' and <= '9' => true,
            >= 'a' and <= 'f' => true,
            >= 'A' and <= 'F' => true,
            _ => false,
        };
    }
}