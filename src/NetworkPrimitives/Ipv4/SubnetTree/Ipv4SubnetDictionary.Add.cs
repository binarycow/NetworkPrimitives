#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace NetworkPrimitives.Ipv4;

public partial class Ipv4SubnetDictionary<TValue>
{
    
    [PublicAPI]
    public void Add(Ipv4AddressRange range, TValue value)
    {
        if (range.IsSubnet(out var subnet))
        {
            Add(subnet, value);
            return;
        }
        foreach (var address in range)
        {
            Add(address, value);
        }
    }
    
    [PublicAPI]
    public void Set(Ipv4Address address, TValue value) 
        => Set(new Ipv4Subnet(address, Ipv4Cidr.Slash32), value);

    [PublicAPI]
    public void Set(Ipv4Subnet subnet, TValue value)
    {
        _ = TryAddRoot(this.rootNode, subnet, value, AddSetMode.Set, out var newRoot);
        this.rootNode = Consolidate(newRoot);
    }
    
    [PublicAPI]
    public void Add(Ipv4Address address, TValue value) 
        => Add(new Ipv4Subnet(address, Ipv4Cidr.Slash32), value);

    [PublicAPI]
    public void Add(Ipv4Subnet subnet, TValue value)
    {
        _ = TryAddRoot(this.rootNode, subnet, value, AddSetMode.Add, out var newRoot);
        this.rootNode = Consolidate(newRoot);
    }

    [PublicAPI]
    public bool TryAdd(Ipv4Address address, TValue value)
        => TryAdd(new Ipv4Subnet(address, Ipv4Cidr.Slash32), value);

    [PublicAPI]
    public bool TryAdd(Ipv4Subnet subnet, TValue value)
    {
        if (!TryAddRoot(this.rootNode, subnet, value, AddSetMode.TryAdd, out var newRoot))
            return false;
        this.rootNode = Consolidate(newRoot);
        return true;
    }

    private bool TryAddRoot(
        Node? rNode,
        Ipv4Subnet subnet,
        TValue value,
        AddSetMode mode,
        [NotNullWhen(true)] out Node? result
    )
    {
        result = (Cidr: subnet.Cidr.Value, Root: rNode, Mode: mode) switch
        {
            (Cidr: _, Root: _, Mode: not (AddSetMode.Add or AddSetMode.Set or AddSetMode.TryAdd))
                => throw new ArgumentOutOfRangeException(nameof(mode)),
            
            (Cidr: < 0 or > 32, Root: _, Mode: _) => throw new InvalidOperationException(),
            
            (Cidr: 0, Root: BranchValueNode or LeafNode, Mode: AddSetMode.Add)
                => ThrowDuplicateKey(subnet, nameof(subnet)),
            (Cidr: 0, Root: BranchValueNode or LeafNode, Mode: AddSetMode.TryAdd)
                => null,
            (Cidr: 0, Root: BranchValueNode node, Mode: AddSetMode.Set)
                => node with { Value = value },
            (Cidr: 0, Root: LeafNode node, Mode: AddSetMode.Set)
                => node with { Value = value },
            
            (Cidr: 0, Root: BranchNode node, Mode: _)
                => BranchValueNode.From(node, value),
            (Cidr: 0, Root: null, Mode: _) 
                => new LeafNode(default, value),
            
            
            (Cidr: > 0, Root: BranchValueNode or BranchNode, Mode: _) 
                => AddToNode(rNode, subnet, value, mode),
            (Cidr: > 0, Root: LeafNode node, Mode: _) 
                => AddToNode(BranchValueNode.From(node), subnet, value, mode),
            (Cidr: > 0, Root: null, Mode: _) 
                => AddToNode(BranchNode.Create(default, null, null), subnet, value, mode),
            
            _ => throw new InvalidOperationException(),
        };
        if (rNode?.Subnet is not null && rNode.Subnet != default)
        {
            ;
        }
        return result is not null;
    }

    private Node? AddToNode(
        Node originalNode,
        Ipv4Subnet subnet,
        TValue value,
        AddSetMode mode
    )
    {
        var ret = (SameCidr: originalNode.Subnet.Mask == subnet.Mask, Node: originalNode, Mode: mode) switch
        {
            (SameCidr: _, Node: _, Mode: not (AddSetMode.Add or AddSetMode.Set or AddSetMode.TryAdd))
                => throw new ArgumentOutOfRangeException(nameof(mode)),
            
            (SameCidr: true, Node: LeafNode or BranchValueNode, Mode: AddSetMode.Add)
                => ThrowDuplicateKey(subnet, nameof(subnet)),
            (SameCidr: true, Node: LeafNode or BranchValueNode, Mode: AddSetMode.TryAdd)
                => null,
            (SameCidr: true, Node: LeafNode node, Mode: AddSetMode.Set)
                => node with { Value = value },
            (SameCidr: true, Node: BranchValueNode node, Mode: AddSetMode.Set)
                => node with { Value = value },
            
            (SameCidr: true, Node: BranchNode node, Mode: _)
                => BranchValueNode.From(node, value),

            (SameCidr: false, Node: LeafNode node, Mode: _)
                => AddToChildren(BranchValueNode.From(node), subnet, value, mode),
            (SameCidr: false, Node: BranchNode node, Mode: _)
                => AddToChildren(node, subnet, value, mode),

            _ => throw new InvalidOperationException(),
        };
        if (ret?.Subnet != originalNode.Subnet)
        {
            ;
        }
        return ret;
    }
    
    
    private Node? AddToChildren(
        BranchNode node,
        Ipv4Subnet subnet,
        TValue value,
        AddSetMode mode
    )
    {
        if (subnet.Mask == node.ChildMask)
        {
            return ChildExactSubnet(node, subnet, value, mode, this.valueComparer);
        }
        return ChildInexactSubnet();

        Node? ChildInexactSubnet()
        {
            var (_, leftSide, rightSide) = node;
            if (TryChildSideInexactSubnet(leftSide, out var result))
                return BranchNode.WithLeft(node, result);
            if (TryChildSideInexactSubnet(rightSide, out result))
                return BranchNode.WithRight(node, result);
            return result;
        }

        bool TryChildSideInexactSubnet(
            NodeAndSubnet side,
            [NotNullWhen(true)] out Node? result
        )
        {
            result = default;
            var (sideSubnet, sideNode) = side;
            if (sideSubnet.Contains(subnet) is false) return false;
            sideNode ??= BranchNode.Create(side.Subnet, null, null);
            result = AddToNode(sideNode, subnet, value, mode);
            return result is not null;
        }
        
        
        static Node? ChildExactSubnet(
            BranchNode node,
            Ipv4Subnet subnet,
            TValue value,
            AddSetMode mode,
            IEqualityComparer<TValue> valueComparer
        )
        {
            var (_, leftSide, rightSide) = node;
            if (TryChildSideExactSubnet(leftSide, subnet, value, mode, out var result))
                return BranchNode.WithLeft(node, result);
            if (TryChildSideExactSubnet(rightSide, subnet, value, mode, out result))
                return BranchNode.WithRight(node, result);
            return result;
        }
        
        static bool TryChildSideExactSubnet(
            NodeAndSubnet side,
            Ipv4Subnet subnet,
            TValue value,
            AddSetMode mode, 
            [NotNullWhen(true)] out Node? result
        )
        {
            result = default;
            var (sideSubnet, sideNode) = side;
            if (sideSubnet.Contains(subnet) is false) return false;
            result = (Node: sideNode, Mode: mode) switch
            {
                (Node: _, Mode: not (AddSetMode.Add or AddSetMode.Set or AddSetMode.TryAdd)) 
                    => throw new ArgumentOutOfRangeException(nameof(mode)),
                (Node: not (null or LeafNode or BranchValueNode or BranchNode), Mode: _) 
                    => throw new InvalidOperationException(),
                
                (Node: null, Mode: _) 
                    => new LeafNode(subnet, value),
                
                (Node: LeafNode or BranchValueNode, Mode: AddSetMode.TryAdd) 
                    => null,
                (Node: LeafNode or BranchValueNode, Mode: AddSetMode.Add) 
                    => ThrowDuplicateKey(subnet, nameof(subnet)),
                (Node: LeafNode leaf, Mode: AddSetMode.Set)
                    => leaf with { Value = value },
                (Node: BranchValueNode leaf, Mode: AddSetMode.Set) 
                    => leaf with { Value = value },
                
                (Node: BranchNode bn, Mode: _) => BranchValueNode.From(bn, value),
            };
            return result is not null;
        }
    }
}