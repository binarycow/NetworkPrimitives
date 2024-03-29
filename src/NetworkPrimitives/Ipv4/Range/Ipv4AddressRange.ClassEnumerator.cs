﻿#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkPrimitives.Ipv4
{
    public readonly partial struct Ipv4AddressRange
    {
        private class ClassEnumerator : IEnumerator<Ipv4Address>, IEnumerable<Ipv4Address>
        {
            private readonly Ipv4Address startAddress;
            private readonly ulong totalAddresses;
            private const int STATE_NOT_STARTED = 0;
            private const int STATE_IN_PROGRESS = 1;
            private const int STATE_FINISHED = 2;

            private int state;
            private uint counter;
            private Ipv4Address current;

            public ClassEnumerator(Ipv4Address startAddress, ulong totalAddresses)
            {
                this.startAddress = startAddress;
                this.totalAddresses = totalAddresses;
                this.state = default;
                this.current = default;
                this.counter = default;
                this.Reset();
            }

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

            public void Reset()
            {
                this.state = default;
                this.counter = default;
                this.current = default;
            }

            object IEnumerator.Current => this.Current;

            public Ipv4Address Current => state switch
            {
                0 => throw new InvalidOperationException("Enumeration not yet started."),
                1 => this.current,
                _ => throw new InvalidOperationException("Enumeration already finished."),
            };
            void IDisposable.Dispose()
            {
            }

            public IEnumerator<Ipv4Address> GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}