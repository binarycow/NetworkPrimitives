using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#pragma warning disable CS1591


namespace NetworkPrimitives.Ipv4;

public partial class Ipv4SubnetDictionary<TValue>
{
    public TValue this[Ipv4Address address]
    {
        get => this.Get(address);
        set => this.Set(address, value);
    }
    public TValue this[Ipv4Subnet subnet]
    {
        get
        {
            if (!this.TryGetValue(subnet, out var value) || value.Count == 0)
                throw new KeyNotFoundException();
            return value[0];
        }
        set => this.Set(subnet, value);
    }


    private TValue Get(Ipv4Address address) => this.TryGetValue(address, out var value) ? value : throw new KeyNotFoundException();
    
    private IReadOnlyList<TValue>? Get(Ipv4Subnet subnet)
    {
        _ = this.TryGetValue(subnet, out var value);
        return value;
    }
    
#if NETSTANDARD2_0
    private bool NonNullTryGetValue(Ipv4Subnet subnet, out IReadOnlyList<TValue> value)
    {
        value = Array.Empty<TValue>(); // Null-forgiving operator
        if (!this.TryGetValue(subnet, out var nullableValue)) return false;
        value = nullableValue;
        return true;
    }
    bool IReadOnlyDictionary<Ipv4Subnet, IReadOnlyList<TValue>>.TryGetValue(Ipv4Subnet address, out IReadOnlyList<TValue> value)
        => NonNullTryGetValue(address, out value);
#endif

    public bool TryGetValue(Ipv4Subnet subnet, [NotNullWhen(true)] out IReadOnlyList<TValue>? value)
    {
        value = default;
        var nodes = LocateMatchingNode(this.rootNode, subnet);
        if (nodes is null)
            return false;
        List<TValue>? list = null;
        while (nodes.TryPop(out var node))
        {
            if (node is not IValueNode valueNode) continue;
            list ??= new ();
            list.Add(valueNode.Value);
        }
        value = list?.AsReadOnly();
        return value is not null;
    }
    
#if NETSTANDARD2_0
    private bool NonNullTryGetValue(Ipv4Address address, out TValue value)
    {
        value = default!; // Null-forgiving operator
        if (!this.TryGetValue(address, out var nullableValue)) return false;
        value = nullableValue;
        return true;
    }
    bool IReadOnlyDictionary<Ipv4Address, TValue>.TryGetValue(Ipv4Address address, out TValue value)
        => NonNullTryGetValue(address, out value);
    /*
    bool IDictionary<Ipv4Address, TValue>.TryGetValue(Ipv4Address address, out TValue value)
        => NonNullTryGetValue(address, out value);
    */
#endif
    
    public bool TryGetValue(Ipv4Address address, [MaybeNullWhen(false)] out TValue value)
    {
        value = default;
        var nodes = LocateMatchingNode(this.rootNode, new (address, Ipv4Cidr.Slash32));
        if (nodes is null)
            return false;
        while (nodes.TryPop(out var node))
        {
            if (node is not IValueNode valueNode) continue;
            value = valueNode.Value;
            return true;
        }
        return false;
    }

    public bool ContainsKey(Ipv4Subnet subnet) => ContainsKey(this.rootNode, subnet);
    public bool ContainsKey(Ipv4Address address) => ContainsKey(this.rootNode, new (address, Ipv4Cidr.Slash32));

    private static bool ContainsKey(Node? node, Ipv4Subnet subnet)
    {
        while (node is not null)
        {
            switch (node)
            {
                case IValueNode valueNode when valueNode.Subnet.Contains(subnet):
                    return true;
                case BranchNode(_, var (leftSubnet, left), var (rightSubnet, right)) when leftSubnet.Contains(subnet):
                    node = left;
                    continue;
                case BranchNode(_, var (leftSubnet, left), var (rightSubnet, right)) when rightSubnet.Contains(subnet):
                    node = right;
                    continue;
                default:
                    node = null;
                    continue;
            }
        }
        return false;
    }
    private static Stack<Node>? LocateMatchingNode(Node? node, Ipv4Subnet address)
    {
        Stack<Node>? stack = null;
        while (node is not null)
        {
            stack ??= new();
            stack.Push(node);
            switch (node)
            {
                case BranchNode(_, var (leftSubnet, left), var (rightSubnet, right)) when leftSubnet.Contains(address):
                    node = left;
                    continue;
                case BranchNode(_, var (leftSubnet, left), var (rightSubnet, right)) when rightSubnet.Contains(address):
                    node = right;
                    continue;
                default:
                    node = null;
                    continue;
            }
        }
        return stack;
    }
}