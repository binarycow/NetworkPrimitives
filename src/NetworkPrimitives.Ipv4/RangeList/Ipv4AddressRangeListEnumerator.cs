#nullable enable

using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Allocation-less enumerator over a range of <see cref="Ipv4AddressRange"/>
    /// </summary>
    public ref struct Ipv4AddressRangeListEnumerator
    {
        private readonly ReadOnlyListSpan<Ipv4AddressRange> original;
        private ReadOnlyListSpan<Ipv4AddressRange> available;
        private Ipv4AddressRange current;
        internal Ipv4AddressRangeListEnumerator(ReadOnlyListSpan<Ipv4AddressRange> original)
        {
            this.available = this.original = original;
            this.current = default;
        }
        

        /// <summary>
        /// Advances the enumerator to the next range
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the enumerator was successfully advanced to the next element;
        /// <see langword="false"/>  if the enumerator has passed the end of the range.
        /// </returns>
        public bool MoveNext() => this.available.TrySliceFirst(out this.current);
        
        
        /// <summary>
        /// The current range
        /// </summary>
        public Ipv4AddressRange Current => current;
        
        
        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first range in the range.
        /// </summary>
        public void Reset()
        {
            this.available = this.original;
            this.current = default;
        }
    }
}