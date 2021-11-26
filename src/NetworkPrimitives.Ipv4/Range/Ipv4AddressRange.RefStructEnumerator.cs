using System;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Allocation-less enumerator over a range of <see cref="Ipv4Address"/>
    /// </summary>
    public ref struct Ipv4AddressEnumerator
    {
        private readonly Ipv4Address startAddress;
        private readonly ulong totalAddresses;
        private const int STATE_NOT_STARTED = 0;
        private const int STATE_IN_PROGRESS = 1;
        private const int STATE_FINISHED = 2;

        private int state;
        private uint counter;
        private Ipv4Address current;

        internal Ipv4AddressEnumerator(Ipv4Address startAddress, ulong totalAddresses)
        {
            this.startAddress = startAddress;
            this.totalAddresses = totalAddresses;
            this.state = default;
            this.current = default;
            this.counter = default;
            this.Reset();
        }

        /// <summary>
        /// Advances the enumerator to the next address
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the enumerator was successfully advanced to the next element;
        /// <see langword="false"/>  if the enumerator has passed the end of the range.
        /// </returns>
        public bool MoveNext()
        {
            switch (this.state)
            {
                case STATE_NOT_STARTED:
                    this.current = startAddress;
                    this.state = STATE_IN_PROGRESS;
                    return true;
                case STATE_IN_PROGRESS when this.counter + 1 > totalAddresses:
                    this.state = STATE_FINISHED;
                    return false;
                case STATE_IN_PROGRESS:
                    ++this.counter;
                    this.current = this.startAddress.AddInternal(this.counter);
                    return true;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first address in the range.
        /// </summary>
        public void Reset()
        {
            this.state = default;
            this.counter = default;
            this.current = default;
        }
        
        /// <summary>
        /// The current address
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The enumerator has not yet started, or the enumerator has already finished.
        /// </exception>
        public Ipv4Address Current => state switch
        {
            0 => throw new InvalidOperationException("Enumeration not yet started."),
            1 => this.current,
            _ => throw new InvalidOperationException("Enumeration already finished."),
        };
    }
}