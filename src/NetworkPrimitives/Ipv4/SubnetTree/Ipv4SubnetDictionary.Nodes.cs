using System.Collections;
using System.Collections.Generic;

#pragma warning disable CS1591

namespace NetworkPrimitives.Ipv4;

public static class NodeExtensions
{
    public static void Deconstruct<TValue>(
        this Ipv4SubnetDictionary<TValue>.IValueNode node,
        out Ipv4Subnet subnet,
        out TValue value
    )
    {
        subnet = node.Subnet;
        value = node.Value;
    }
    
    
    public static void Deconstruct<TValue>(
        this Ipv4SubnetDictionary<TValue>.IBranchNode node,
        out Ipv4Subnet subnet,
        out Ipv4SubnetDictionary<TValue>.Node? left,
        out Ipv4SubnetDictionary<TValue>.Node? right
    )
    {
        subnet = node.Subnet;
        left = node.Left;
        right = node.Right;
    }
}

public partial class Ipv4SubnetDictionary<TValue>
{

    public readonly record struct NodeAndSubnet(Ipv4Subnet Subnet, Node? Node);


    
    public interface INode
    {
        Ipv4Subnet Subnet { get; }
        public (Ipv4Subnet Subnet, TValue? Value, Ipv4Subnet? Left, Ipv4Subnet? Right) ToNodeTuple();
    }
    public interface IValueNode : INode
    {
        TValue Value { get; }
    }
    public interface IBranchNode : INode
    {
        Node? Left { get; }
        Node? Right { get; }
        Ipv4Subnet LeftSubnet { get; }
        Ipv4Subnet RightSubnet { get; }
    }
    public interface IBranchValueNode : IBranchNode, IValueNode
    {
    }
    
    public abstract record Node(
        Ipv4Subnet Subnet
    ) : INode
    {
        public abstract (Ipv4Subnet Subnet, TValue? Value, Ipv4Subnet? Left, Ipv4Subnet? Right) ToNodeTuple();
    }

    public record LeafNode(
        Ipv4Subnet Subnet,
        TValue Value
    ) : Node(Subnet), IValueNode
    {
        public override (Ipv4Subnet Subnet, TValue? Value, Ipv4Subnet? Left, Ipv4Subnet? Right) ToNodeTuple() 
            => (this.Subnet, this.Value, null, null);

        public BranchNode Split()
        {
            _ = this.Subnet.TrySplit(out var leftSubnet, out var rightSubnet);
            return BranchNode.Create(
                Subnet, 
                new LeafNode(leftSubnet, this.Value),
                new LeafNode(rightSubnet, this.Value)
            );
        }
    }

    public record BranchNode(
        Ipv4Subnet Subnet,
        NodeAndSubnet Left,
        NodeAndSubnet Right
    ) : Node(Subnet), IBranchNode
    {
        public Ipv4SubnetMask ChildMask => Left.Subnet.Mask;
        public Ipv4Cidr ChildCidr => Left.Subnet.Cidr;
        
        public static BranchNode Create(
            Ipv4Subnet subnet,
            Node? left,
            Node? right
        )
        {
            _ = subnet.TrySplit(out var leftSubnet, out var rightSubnet);
            return new (subnet, new (leftSubnet, left), new (rightSubnet, right));
        }
        public static BranchValueNode Create(
            Ipv4Subnet subnet,
            TValue value,
            Node? left,
            Node? right
        )
        {
            _ = subnet.TrySplit(out var leftSubnet, out var rightSubnet);
            return new (subnet, value, new (leftSubnet, left), new (rightSubnet, right));
        }

        Node? IBranchNode.Left => this.Left.Node;

        Node? IBranchNode.Right => this.Right.Node;

        Ipv4Subnet IBranchNode.LeftSubnet => this.Left.Subnet;

        Ipv4Subnet IBranchNode.RightSubnet => this.Right.Subnet;

        public override (Ipv4Subnet Subnet, TValue? Value, Ipv4Subnet? Left, Ipv4Subnet? Right) ToNodeTuple() 
            => (this.Subnet, default, this.Left.Subnet, this.Right.Subnet);

        public static BranchNode WithLeft(BranchNode node, Node? left)
            => node is BranchValueNode bvn
                ? bvn with { Left = bvn.Left with { Node = left } }
                : node with { Left = node.Left with { Node = left } };
        
        public static BranchNode WithRight(BranchNode node, Node? right) 
            => node is BranchValueNode bvn 
                ? bvn with { Right = bvn.Right with { Node = right } }
                : node with { Right = node.Right with { Node = right } };
    }

    
    public record BranchValueNode(
        Ipv4Subnet Subnet,
        TValue Value,
        NodeAndSubnet Left,
        NodeAndSubnet Right
    ) : BranchNode(Subnet, Left, Right), IBranchValueNode
    {
        
        public override (Ipv4Subnet Subnet, TValue? Value, Ipv4Subnet? Left, Ipv4Subnet? Right) ToNodeTuple() 
            => (this.Subnet, this.Value, this.Left.Subnet, this.Right.Subnet);
        
        public static BranchValueNode From(BranchNode node, TValue value) 
            => new (node.Subnet, value, node.Left, node.Right);
        // ReSharper disable once SuggestBaseTypeForParameter
        public static BranchValueNode From(LeafNode node) 
            => Create(node.Subnet, node.Value, null, null);

        public BranchNode WithoutValue() => new (this.Subnet, this.Left, this.Right);
    }
}