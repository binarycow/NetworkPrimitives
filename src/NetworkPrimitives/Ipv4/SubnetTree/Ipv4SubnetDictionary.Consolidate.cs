using System;
using System.Collections.Generic;

#pragma warning disable CS1591
namespace NetworkPrimitives.Ipv4;

public partial class Ipv4SubnetDictionary<TValue>
{
    

    private Node? Consolidate(Node? node) 
        => this.consolidate
            ? Consolidate(node, this.valueComparer, out _) 
            : node;

    private static (Node? Node, bool Changed) Consolidate(Node? node, IEqualityComparer<TValue> valueComparer)
    {
        var newNode = Consolidate(node, valueComparer, out var changed);
        return (newNode, changed);
    }
    
    private static Node? Consolidate(Node? originalNode, IEqualityComparer<TValue> valueComparer, out bool changed)
    {
        return originalNode switch
        {
            not (null or LeafNode or BranchNode) => throw new InvalidOperationException(),
            null => Unchanged(null, out changed),
            LeafNode => Unchanged(originalNode, out changed),
            BranchNode node => ConsolidateBranch(node, valueComparer, out changed),
        };

        static Node? Unchanged(Node? value, out bool changed)
        {
            changed = false;
            return value;
        }
        static Node? Changed(Node? value, out bool changed)
        {
            changed = true;
            return value;
        }

        static Node? ConsolidateBranch(
            BranchNode node, 
            IEqualityComparer<TValue> valueComparer, 
            out bool changed
        )
        {
            var (subnet, (_, leftNode), (_, rightNode)) = node;
            var (newLeft, leftChanged) = Consolidate(leftNode, valueComparer);
            var (newRight, rightChanged) = Consolidate(rightNode, valueComparer);
            var forceChanged = leftChanged || rightChanged;

            return ConsolidateBranch2(node, subnet, newLeft, newRight, valueComparer, forceChanged, out changed);
        }

        
        static Node? ConsolidateBranch2(
            BranchNode node,
            Ipv4Subnet subnet,
            Node? leftNode,
            Node? rightNode,
            IEqualityComparer<TValue> valueComparer,
            bool forceChanged,
            out bool changed
        )
        {
            return (Node: node, Force: forceChanged, Left: leftNode, Right: rightNode) switch
            {
                #region Guards

                (Node: _, Force: _, Left: not(null or LeafNode or BranchValueNode or BranchNode), Right: _)
                    => throw new InvalidOperationException(),
                (Node: _, Force: _, Left: _, Right: not(null or LeafNode or BranchValueNode or BranchNode))
                    => throw new InvalidOperationException(),

                #endregion Guards

                #region Can consolidate - Both children are null

                (Node: BranchValueNode(_, var nodeValue), Force: _, Left: null, Right: null)
                    => Changed(new LeafNode(subnet, nodeValue), out changed),
                
                (Node: _, Force: _, Left: null, Right: null)
                    => Changed(null, out changed),

                #endregion Can consolidate - Both children are null
                

                #region Can't consolidate - both this node and the left node are value nodes - but with different values

                (Node: BranchValueNode(_, var nodeValue), Force: true, Left: IValueNode(_, var leftValue), Right: _)
                    when valueComparer.Equals(nodeValue, leftValue) is false
                    => Changed(BranchValueNode.Create(subnet, nodeValue, leftNode, rightNode), out changed),
                
                (Node: BranchValueNode(_, var nodeValue), Force: true, Left: _, Right: IValueNode(_, var rightValue))
                    when valueComparer.Equals(nodeValue, rightValue) is false
                    => Changed(BranchValueNode.Create(subnet, nodeValue, leftNode, rightNode), out changed),
                
                (Node: BranchValueNode(_, var nodeValue), Force: false, Left: IValueNode(_, var leftValue), Right: _)
                    when valueComparer.Equals(nodeValue, leftValue) is false
                    => Unchanged(node, out changed),
                
                (Node: BranchValueNode(_, var nodeValue), Force: false, Left: _, Right: IValueNode(_, var rightValue))
                    when valueComparer.Equals(nodeValue, rightValue) is false
                    => Unchanged(node, out changed),

                #endregion Can't consolidate - both this node and the left node are value nodes - but with different values

                #region Can consolidate - both children have the same value as this node

                (Node: BranchValueNode(_, var nodeValue), Force: _, Left: BranchValueNode left, Right: BranchValueNode right)
                    when valueComparer.Equals(nodeValue, left.Value) && valueComparer.Equals(left.Value, right.Value)
                    => Changed(BranchValueNode.Create(subnet, left.Value, left.WithoutValue(), right.WithoutValue()), out changed),
                
                (Node: BranchValueNode(_, var nodeValue), Force: _, Left: BranchValueNode left, Right: LeafNode right)
                    when valueComparer.Equals(nodeValue, left.Value) && valueComparer.Equals(left.Value, right.Value)
                    => Changed(BranchValueNode.Create(subnet, left.Value, left.WithoutValue(), null), out changed),
                
                (Node: BranchValueNode(_, var nodeValue), Force: _, Left: LeafNode left, Right: BranchValueNode right)
                    when valueComparer.Equals(nodeValue, left.Value) && valueComparer.Equals(left.Value, right.Value)
                    => Changed(BranchValueNode.Create(subnet, left.Value, null, right.WithoutValue()), out changed),
                
                (Node: BranchValueNode(_, var nodeValue), Force: _, Left: LeafNode left, Right: LeafNode right)
                    when valueComparer.Equals(nodeValue, left.Value) && valueComparer.Equals(left.Value, right.Value)
                    => Changed(BranchValueNode.Create(subnet, left.Value, null, right), out changed),
                
                (Node: _, Force: _, Left: BranchValueNode left, Right: BranchValueNode right)
                    when valueComparer.Equals(left.Value, right.Value)
                    => Changed(BranchValueNode.Create(subnet, left.Value, left.WithoutValue(), right.WithoutValue()), out changed),
                
                (Node: _, Force: _, Left: BranchValueNode left, Right: LeafNode right)
                    when valueComparer.Equals(left.Value, right.Value)
                    => Changed(BranchValueNode.Create(subnet, left.Value, left.WithoutValue(), null), out changed),
                
                (Node: _, Force: _, Left: LeafNode left, Right: BranchValueNode right)
                    when valueComparer.Equals(left.Value, right.Value)
                    => Changed(BranchValueNode.Create(subnet, left.Value, null, right.WithoutValue()), out changed),
                
                (Node: _, Force: _, Left: LeafNode left, Right: LeafNode right)
                    when valueComparer.Equals(left.Value, right.Value)
                    => Changed(BranchValueNode.Create(subnet, left.Value, null, right), out changed),

                #endregion Can consolidate - both children have the same value as this node


                #region Can't consolidate

                (Node: BranchValueNode(_, var nodeValue), Force: true, Left: _, Right: _) 
                    => Changed(BranchValueNode.Create(subnet, nodeValue, leftNode, rightNode), out changed),
                
                (Node: _, Force: true, Left: _, Right: _) 
                    => Changed(BranchValueNode.Create(subnet, leftNode, rightNode), out changed),
                
                (Node: _, Force: false, Left: _, Right: _) 
                    => Unchanged(node, out changed),

                #endregion Can't consolidate
            };
        }
    }

}