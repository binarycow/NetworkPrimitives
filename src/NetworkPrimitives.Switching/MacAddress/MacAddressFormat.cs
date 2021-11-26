#nullable enable

using System;

namespace NetworkPrimitives.Switching
{
    public readonly struct MacAddressFormat : IFormatProvider, ICustomFormatter, IEquatable<MacAddressFormat>
    {

        object IFormatProvider.GetFormat(Type? formatType) => throw new NotImplementedException();

        string ICustomFormatter.Format(string? format, object? arg, IFormatProvider? formatProvider) => throw new NotImplementedException();


        private readonly int providedNibblesPerGroup;
        
        
        internal int NumberOfGroups => 12 / NibblesPerGroup;
        public int NumberOfSeparators => NumberOfGroups - 1;
        public int CharactersRequired => 12 + NumberOfSeparators;
        public int NibblesPerGroup => this.providedNibblesPerGroup > 0 ? this.providedNibblesPerGroup : 12;
        public char GroupSeparator { get; }
        public Casing Casing { get; }

        public void Deconstruct(out int nibblesPerGroup, out char groupSeparator, out Casing casing)
        {
            nibblesPerGroup = this.NibblesPerGroup;
            groupSeparator = this.GroupSeparator;
            casing = this.Casing;
        }

        public MacAddressFormat(
            int nibblesPerGroup, 
            char groupSeparator,
            Casing casing = Casing.NotSpecified
        )
        {
            if(nibblesPerGroup is not (1 or 2 or 3 or 4 or 6 or 12))
                throw new ArgumentOutOfRangeException(nameof(nibblesPerGroup), "value must be a factor of 12.");
            if (groupSeparator == '\0' && nibblesPerGroup is not 12)
                throw new ArgumentOutOfRangeException(nameof(groupSeparator), "Group separator can only be omitted if the number of nibbles per group is 12.");
            
            this.providedNibblesPerGroup = nibblesPerGroup;
            this.GroupSeparator = groupSeparator;
            this.Casing = casing;
        }

        #region Equality
        public bool Equals(MacAddressFormat other) => this.NibblesPerGroup == other.NibblesPerGroup && this.GroupSeparator == other.GroupSeparator && this.Casing == other.Casing;
        public override bool Equals(object? obj) => obj is MacAddressFormat other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.NibblesPerGroup, this.GroupSeparator, (int)this.Casing);
        public static bool operator ==(MacAddressFormat left, MacAddressFormat right) => left.Equals(right);
        public static bool operator !=(MacAddressFormat left, MacAddressFormat right) => !left.Equals(right);
        #endregion Equality

    }
}