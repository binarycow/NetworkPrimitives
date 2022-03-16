using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable CS1591


namespace NetworkPrimitives.Ipv4;

public partial class Ipv4SubnetDictionary<TValue>
{
    private class NodeEnumerator : IEnumerable<Node>, IEnumerator<Node>
    {
        private const int STATE_NOT_STARTED = 0;
        private const int STATE_STARTED = 1;
        private const int STATE_FINISHED = 2;
        
        private readonly Node? rootNode;
        private readonly Queue<Node> queue = new();
        private int state = STATE_NOT_STARTED;

        public NodeEnumerator(Ipv4SubnetDictionary<TValue> dictionary) : this(dictionary.rootNode)
        {
        }

        public NodeEnumerator(Node? rootNode) => this.rootNode = rootNode;
        private NodeEnumerator(Node? rootNode, IEnumerable<Node> queueNodes, int state)
        {
            this.rootNode = rootNode;
            this.state = state;
            foreach (var node in queueNodes)
                this.queue.Enqueue(node);
        }
        
        public void Reset()
        {
            this.queue.Clear();
            this.state = STATE_NOT_STARTED;
        }

        public bool MoveNext()
        {
            switch (this.state)
            {
                case STATE_NOT_STARTED when this.rootNode is null:
                    this.state = STATE_FINISHED;
                    return false;
                case STATE_NOT_STARTED:
                    this.state = STATE_STARTED;
                    this.queue.Enqueue(this.rootNode);
                    goto case STATE_STARTED;
                case STATE_STARTED:
                    this.current = HandleMoveNext();
                    this.state = this.current is null
                        ? STATE_FINISHED
                        : STATE_STARTED;
                    return this.state == STATE_STARTED;
                default:
                    return false;
            }
        }

        private Node? HandleMoveNext()
        {
            if (!this.queue.TryDequeue(out var nextNode))
            {
                return null;
            }
            if (nextNode is not BranchNode(_, var (leftSubnet, left), var (rightSubnet, right))) return nextNode;
            this.QueueNode(left);
            this.QueueNode(right);
            return nextNode;
        }

        private void QueueNode(Node? node)
        {
            if (node is not null)
                this.queue.Enqueue(node);
        }

        public NodeEnumerator Clone() => new (this.rootNode, this.queue, this.state);
        private Node? current = null;
        public Node Current => this.current ?? throw new InvalidOperationException();
        public NodeEnumerator GetEnumerator() => this;
        
        
        object? IEnumerator.Current => this.Current;
        void IDisposable.Dispose()
        {
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        IEnumerator<Node> IEnumerable<Node>.GetEnumerator() => this.GetEnumerator();
    }
}