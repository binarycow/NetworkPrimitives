namespace NetworkPrimitives
{
    /// <summary>
    /// Represents the desired / required casing of letters.
    /// </summary>
    public enum Casing
    {
        /// <summary>
        /// Casing is not specified or unknown.
        /// </summary>
        NotSpecified,
        /// <summary>
        /// Casing is uppercase, or must be uppercase
        /// </summary>
        Uppercase,
        /// <summary>
        /// Casing is lowercase, or must be lowercase
        /// </summary>
        Lowercase,
    }
}