#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkPrimitives.Ipv4
{
    public partial class Ipv4AddressRangeList
    {
        /// <summary>
        /// A mutable list of <see cref="Ipv4AddressRange"/>
        /// </summary>
        public sealed class Builder : IList<Ipv4AddressRange>
        {
            private readonly List<Ipv4AddressRange> ranges;

            internal Builder(List<Ipv4AddressRange> ranges)
            {
                this.ranges = ranges;
            }

            /// <summary>
            /// Create an immutable version of this <see cref="Builder"/>
            /// </summary>
            /// <returns>
            /// An instance of <see cref="Ipv4AddressRangeList"/>
            /// </returns>
            public Ipv4AddressRangeList ToImmutable() => new Ipv4AddressRangeList(this.ranges.AsReadOnly());
            

            
            IEnumerator<Ipv4AddressRange> IEnumerable<Ipv4AddressRange>.GetEnumerator() => ranges.GetEnumerator();

            /// <summary>
            /// Get a ref struct enumerator to iterate over the ranges covered by this range list.
            /// </summary>
            /// <returns>
            /// A ref struct enumerator
            /// </returns>
            public Ipv4AddressRangeListEnumerator GetEnumerator() => new (new (ranges));
        
            /// <summary>
            /// Get a listing of all individual addresses in this set of ranges.
            /// </summary>
            /// <returns>
            /// An <see cref="Ipv4AddressListSpan"/> that represents all individual IP addresses in this instance.
            /// </returns>
            public Ipv4AddressListSpan GetAllAddresses() => Ipv4AddressListSpan.CreateNew(ranges);

            IEnumerator IEnumerable.GetEnumerator() => ranges.GetEnumerator();
            
            /// <summary>
            /// Add a range to this instance.
            /// </summary>
            /// <param name="item">
            /// The <see cref="Ipv4AddressRange"/> to add
            /// </param>
            public void Add(Ipv4AddressRange item) => this.ranges.Add(item);

            /// <summary>
            /// Removes all elements from this list.
            /// </summary>
            public void Clear() => this.ranges.Clear();

            /// <summary>
            /// Determines whether a range is in this list.
            /// </summary>
            /// <param name="item">
            /// The range to locate in this list.
            /// </param>
            /// <returns>
            /// <see langword="true"/> if the range is found in this list; otherwise, false.
            /// </returns>
            public bool Contains(Ipv4AddressRange item) => this.ranges.Contains(item);


            /// <summary>
            /// Remove a range from this instance.
            /// </summary>
            /// The <see cref="Ipv4AddressRange"/> to remove
            /// <returns>
            /// <see langword="true"/> if the range was removed, otherwise <see langword="false"/>
            /// </returns>
            public bool Remove(Ipv4AddressRange item) => this.ranges.Remove(item);

            /// <summary>
            /// The number of items in this instance.
            /// </summary>
            public int Count => this.ranges.Count;


            /// <summary>
            /// Returns the zero-based index of the first occurrence of a value in this instance
            /// </summary>
            /// <param name="item">
            /// The range to locate in the list.
            /// </param>
            /// <returns>
            /// The zero-based index of the first occurrence of the range within the this list, if found; otherwise, -1.
            /// </returns>
            public int IndexOf(Ipv4AddressRange item) => this.ranges.IndexOf(item);

            /// <summary>
            /// Inserts a range into the list at the specified index.
            /// </summary>
            /// <param name="index">
            /// The zero-based index at which <paramref name="item"/> should be inserted.
            /// </param>
            /// <param name="item">
            /// The range to insert
            /// </param>
            public void Insert(int index, Ipv4AddressRange item) => this.ranges.Insert(index, item);

            /// <summary>
            /// Removes the range at the specified index of this list.
            /// </summary>
            /// <param name="index">
            /// The zero-based index of the range to remove.
            /// </param>
            public void RemoveAt(int index) => this.ranges.RemoveAt(index);

            /// <summary>
            /// Gets or sets the range at the specified index
            /// </summary>
            /// <param name="index">
            /// The zero basd index of the range to get or set.
            /// </param>
            public Ipv4AddressRange this[int index]
            {
                get => this.ranges[index];
                set => this.ranges[index] = value;
            }

            bool ICollection<Ipv4AddressRange>.IsReadOnly => false;
            void ICollection<Ipv4AddressRange>.CopyTo(Ipv4AddressRange[] array, int arrayIndex) => this.ranges.CopyTo(array, arrayIndex);
            
            
        }
    }
}