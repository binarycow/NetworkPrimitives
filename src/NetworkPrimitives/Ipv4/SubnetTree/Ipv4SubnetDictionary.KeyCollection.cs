using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

#pragma warning disable CS1591
namespace NetworkPrimitives.Ipv4;

public partial class Ipv4SubnetDictionary<TValue>
{
    public abstract class KeyCollection<TKey> : ICollection<TKey> 
        where TKey : struct
    {
        protected Node? RootNode { get; }
        private protected KeyCollection(Ipv4SubnetDictionary<TValue> dictionary) => this.RootNode = dictionary.rootNode;
        
        public abstract IEnumerator<TKey> GetEnumerator();
        public abstract bool Contains(TKey item);
        public abstract int Count { get; }
        public bool IsReadOnly => true;
        IEnumerator IEnumerable.GetEnumerator() => throw new System.NotImplementedException();

        #region Not Supported

        void ICollection<TKey>.CopyTo(TKey[] array, int arrayIndex)
        {
            using var enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
                array[arrayIndex++] = enumerator.Current;
        }
        void ICollection<TKey>.Add(TKey item) => throw new InvalidOperationException();
        void ICollection<TKey>.Clear() => throw new InvalidOperationException();
        bool ICollection<TKey>.Remove(TKey item) => throw new InvalidOperationException();
        #endregion Not Supported
    }

    public class SubnetKeyCollection : KeyCollection<Ipv4Subnet>
    {
        internal SubnetKeyCollection(Ipv4SubnetDictionary<TValue> dictionary) : base(dictionary)
        {
        }

        private int GetCount()
        {
            if (cachedCount is not null)
                return this.cachedCount.Value;
            
            var count = 0;
            using var enumerator = GetNodeEnumerator(this.RootNode);
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is IValueNode)
                    ++count;
            }
            this.cachedCount = count;
            return count;
        }

        
        public override IEnumerator<Ipv4Subnet> GetEnumerator() => new SubnetEnumerator(GetNodeEnumerator(RootNode));
        public override bool Contains(Ipv4Subnet item) => ContainsKey(RootNode, item);
        private int? cachedCount;
        
        public override int Count => this.GetCount();
        
        
        private class SubnetEnumerator : IEnumerator<Ipv4Subnet>
        {
            private readonly NodeEnumerator nodeEnumerator;
            private const int STATE_NOT_STARTED = 0;
            private const int STATE_STARTED = 1;
            private const int STATE_FINISHED = 2;
            private int state;
            public SubnetEnumerator(NodeEnumerator nodeEnumerator)
            {
                this.nodeEnumerator = nodeEnumerator;
                this.state = STATE_NOT_STARTED;
            }

            public void Reset()
            {
                this.nodeEnumerator.Reset();
                this.state = STATE_NOT_STARTED;
            }


            public bool MoveNext()
            {
                switch (this.state)
                {
                    case STATE_NOT_STARTED:
                        this.state = STATE_STARTED;
                        goto case STATE_STARTED;
                    case STATE_STARTED:
                        if (HandleMoveNext())
                            return true;
                        this.state = STATE_FINISHED;
                        this.Current = default;
                        return false;
                    default:
                        return false;
                }
            }

            private bool HandleMoveNext()
            {
                while (this.nodeEnumerator.MoveNext())
                {
                    if (this.nodeEnumerator.Current is not IValueNode node) continue;
                    this.Current = node.Subnet;
                    return true;
                }
                return false;
            }
            public Ipv4Subnet Current { get; private set; }
            
            
            object IEnumerator.Current => this.Current;
            void IDisposable.Dispose()
            {
            }
        }
    }


    public class AddressKeyCollection : KeyCollection<Ipv4Address>
    {
        internal AddressKeyCollection(Ipv4SubnetDictionary<TValue> dictionary) : base(dictionary)
        {
        }

        public override IEnumerator<Ipv4Address> GetEnumerator() => throw new NotImplementedException();
        public override bool Contains(Ipv4Address item) => throw new NotImplementedException();
        public override int Count => throw new NotImplementedException();
    }

    public class ValueCollection : ICollection<TValue>
    {
        private readonly Node? rootNode;
        private ulong? cachedCount;

        internal ValueCollection(Ipv4SubnetDictionary<TValue> dictionary) : this(dictionary.rootNode)
        {
        }
        private ValueCollection(Node? rootNode)
        {
            this.rootNode = rootNode;
        }


        private ulong GetCount()
        {
            if (cachedCount is not null)
                return this.cachedCount.Value;
            var count = GetCount(this.rootNode);
            this.cachedCount = count;
            return count;
        }
        
        private static ulong GetCount(Node? node)
        {
            if (node is null)
                return 0;
            var queue = new Queue<Node>();
            queue.Enqueue(node);
            ulong count = 0;
            while (queue.TryDequeue(out node))
            {
                switch (node)
                {
                    case IValueNode:
                        count += node.Subnet.TotalHosts;
                        break;
                    case BranchNode(_, var left, var right):
                        if (left.Node is not null)
                            queue.Enqueue(left.Node);
                        if (right.Node is not null)
                            queue.Enqueue(right.Node);
                        break;
                }
            }
            return count;
        }
        
        
        private static IEnumerable<TValue> GetValues(Node? node)
        {
            if (node is null)
                yield break;
            var queue = new Queue<Node>();
            queue.Enqueue(node);
            while (queue.TryDequeue(out node))
            {
                if (node is IValueNode(_, var value))
                {
                    yield return value;
                }
                if (node is not IBranchNode branchNode) continue;
                
                if (branchNode.Left is not null)
                    queue.Enqueue(branchNode.Left);
                if (branchNode.Right is not null)
                    queue.Enqueue(branchNode.Right);
            }
        }
        
        
        public IEnumerator<TValue> GetEnumerator() => throw new NotImplementedException();
        public bool Contains(TValue item) => throw new InvalidOperationException();
        public int Count => (int)this.GetCount();
        public ulong LongCount => this.GetCount();
        
        public bool IsReadOnly => true;
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
        
        #region Not Supported
        void ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex) => throw new InvalidOperationException();
        void ICollection<TValue>.Add(TValue item) => throw new InvalidOperationException();
        void ICollection<TValue>.Clear() => throw new InvalidOperationException();
        bool ICollection<TValue>.Remove(TValue item) => throw new InvalidOperationException();
        #endregion Not Supported
    }
}