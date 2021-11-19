using System;

namespace NetworkPrimitives.Yang
{
    public readonly struct DataError : IEquatable<DataError>
    {
        public string? ErrorMessage { get; }
        public string? ErrorAppTag { get; }

        public DataError(string? errorMessage, string? errorAppTag)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorAppTag = errorAppTag;
        }

        public bool Equals(DataError other) => this.ErrorMessage == other.ErrorMessage && this.ErrorAppTag == other.ErrorAppTag;
        public override bool Equals(object? obj) => obj is DataError other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.ErrorMessage, this.ErrorAppTag);
        public static bool operator ==(DataError left, DataError right) => left.Equals(right);
        public static bool operator !=(DataError left, DataError right) => !left.Equals(right);
    }
}