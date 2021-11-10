/*

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace NetworkPrimitives.Utilities
{
    public abstract class ImmutableListWrapperWithBuilder<TDerived, TItem, TBuilder>
        where TDerived : ImmutableListWrapperWithBuilder<TDerived, TItem, TBuilder>
        where TBuilder : ImmutableListWrapperBuilder<TBuilder, TItem, TDerived>
    {
        internal ImmutableListWrapperWithBuilder()
        {
            
        }
        //public abstract TBuilder ToBuilder();
    }
    public abstract class ImmutableListWrapperWithBuilder<TDerived, TItem, TEnumerator, TBuilder>
        where TDerived : ImmutableListWrapperWithBuilder<TDerived, TItem, TEnumerator, TBuilder>
        where TBuilder : ImmutableListWrapperBuilder<TBuilder, TItem, TDerived>
    {
        internal ImmutableListWrapperWithBuilder()
        {
            
        }
        //public abstract TBuilder ToBuilder();
    }

    public abstract class ImmutableListWrapper<TDerived, TItem> : IReadOnlyList<TItem>
        where TDerived : ImmutableListWrapper<TDerived, TItem>
    {
        protected abstract IEqualityComparer<TItem> EqualityComparer { get; }
        protected abstract TDerived CreateInstance(ImmutableList<TItem> items);
        protected ImmutableList<TItem> Items { get; }
        internal ImmutableListWrapper(ImmutableList<TItem> items) => this.Items = items;
        public int Count => Items.Count;
        public bool IsEmpty => Count == 0;
        public TItem this[int index] => this.Items[index];
        public TDerived Add(TItem item) => CreateInstance(this.Items.Add(item));
        public TDerived AddRange(IEnumerable<TItem> items) => CreateInstance(this.Items.AddRange(items));
        public TDerived Clear() => CreateInstance(ImmutableList<TItem>.Empty);
        public TItem? Find(Predicate<TItem> predicate) => this.Items.Find(predicate); 
        public TDerived FindAll(Predicate<TItem> predicate) => CreateInstance(this.Items.FindAll(predicate)); 
        public TDerived Insert(int index, TItem item) => CreateInstance(this.Items.Insert(index, item));
        public int IndexOf(TItem item) => this.Items.IndexOf(item, 0, Count, EqualityComparer);
        public TDerived InsertRange(int index, IEnumerable<TItem> items) => CreateInstance(this.Items.InsertRange(index, items));
        public ref readonly TItem ItemRef(int index) => ref this.Items.ItemRef(index);
        public int LastIndexOf(TItem item) => this.Items.LastIndexOf(item, 0, Count, EqualityComparer);
        public TDerived Remove(TItem item) => CreateInstance(this.Items.Remove(item, EqualityComparer));
        public TDerived GetRange(int index, int count) => CreateInstance(this.Items.GetRange(index, count));
        public TDerived RemoveAll(Predicate<TItem> predicate) => CreateInstance(this.Items.RemoveAll(predicate));
        public TDerived RemoveRange(IEnumerable<TItem> items) => CreateInstance(this.Items.RemoveRange(items, EqualityComparer));
        public TDerived RemoveAt(int index) => CreateInstance(this.Items.RemoveAt(index));
        public TDerived Replace(TItem oldValue, TItem newValue) => CreateInstance(this.Items.Replace(oldValue, newValue, EqualityComparer));
        public TDerived Reverse() => CreateInstance(this.Items.Reverse());
        public TDerived Reverse(int index, int count) => CreateInstance(this.Items.Reverse(index, count));
        public TDerived SetItem(int index, TItem item) => CreateInstance(this.Items.SetItem(index, item));
        protected virtual IEnumerator<TItem> GetClassEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetClassEnumerator();
        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => GetClassEnumerator();
    }
}


*/