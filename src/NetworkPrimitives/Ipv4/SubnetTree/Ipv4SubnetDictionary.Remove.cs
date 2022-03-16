#pragma warning disable CS1591
/*
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetworkPrimitives.Ipv4;

public partial class Ipv4SubnetDictionary<TValue>
{
    public bool Remove(Ipv4Address address) 
        => Remove(new Ipv4Subnet(address, Ipv4Cidr.Slash32));
    public bool Remove(Ipv4Address address, TValue value) 
        => Remove(new Ipv4Subnet(address, Ipv4Cidr.Slash32), value);
    
    public bool Remove(Ipv4Subnet subnet)
    {
        var newNode = Remove(this.rootNode, subnet, out var removed);
        this.rootNode = Consolidate(newNode);
        return removed;
    }
    
    public bool Remove(Ipv4Subnet subnet, TValue value)
    {
        var newNode = RemoveWithValue(this.rootNode, subnet, value, out var removed);
        this.rootNode = Consolidate(newNode);
        return removed;
    }

    
    private Node? RemoveWithValue(
        Node? originalNode,
        Ipv4Subnet subnet,
        TValue value,
        out bool removed
    ) => RemoveWithValue(originalNode, subnet, value, this.valueComparer, out removed);

    private static Node? Remove(
        Node? originalNode,
        Ipv4Subnet subnet,
        out bool removed
    )
    {
        if (originalNode is null)
        {
            removed = false;
            return null;
        }
        if (originalNode.Subnet == subnet)
        {
            removed = true;
            return null;
        }

        switch (originalNode)
        {
            case LeafNode leafNode:
                return Remove(leafNode.Split(), subnet, out removed);
            case BranchNode branch:
                Node? newNode;
                if (branch.Left.Subnet.Contains(subnet))
                {
                    newNode = Remove(branch.Left.Node, subnet, out removed);
                    return removed ? BranchNode.WithLeft(branch, newNode) : branch;
                }
                newNode = Remove(branch.Right.Node, subnet, out removed);
                return removed ? BranchNode.WithRight(branch, newNode) : branch;
            default:
                throw new InvalidOperationException();
        }
    }
    
    
    private static Node? RemoveWithValue(
        Node? originalNode,
        Ipv4Subnet subnet,
        TValue value,
        IEqualityComparer<TValue> valueComparer,
        out bool removed
    )
    {
        Node? newNode;
        if (originalNode is null)
        {
            removed = false;
            return null;
        }
        
        if (originalNode.Subnet == subnet)
        {
            (removed, newNode) = originalNode switch
            {
                BranchValueNode node when valueComparer.Equals(node.Value, value) 
                    => (Removed: true, Node: node.WithoutValue()),
                LeafNode node when valueComparer.Equals(node.Value, value)
                    => (Removed: true, Node: null),
                BranchValueNode or BranchNode or LeafNode 
                    => (Removed: false, Node: originalNode),
                _ => throw new InvalidOperationException(),
            };
            removed = originalNode is IValueNode(_, var nodeValue) && valueComparer.Equals(nodeValue, value);
            return newNode;
        }
        
        if (originalNode is not BranchNode branch)
        {
            removed = false;
            return originalNode;
        }
        
        
        if (branch.Left.Subnet.Contains(subnet))
        {
            newNode = RemoveWithValue(branch.Left.Node, subnet, value, valueComparer, out removed);
            return removed ? BranchNode.WithLeft(branch, newNode) : branch;
        }
        
        newNode = RemoveWithValue(branch.Right.Node, subnet, value, valueComparer, out removed);
        return removed ? BranchNode.WithRight(branch, newNode) : branch;

    }
}
*/