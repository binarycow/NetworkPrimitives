using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace NetworkPrimitives.Ipv4;

public partial class Ipv4SubnetDictionary<TValue>
{
    private enum AddSetMode
    {
        Add,
        TryAdd,
        Set
    }
    
    [PublicAPI]
    public Ipv4SubnetDictionary(IEqualityComparer<TValue>? valueComparer = null)
        : this(true, null, valueComparer)
    {
    }
    
    [PublicAPI]
    public Ipv4SubnetDictionary(bool consolidate, IEqualityComparer<TValue>? valueComparer = null)
        : this(consolidate, null, valueComparer)
    {
    }
    private Ipv4SubnetDictionary(bool consolidate, Node? rootNode, IEqualityComparer<TValue>? valueComparer)
    {
        this.valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
        this.consolidate = consolidate;
        this.rootNode = rootNode;
    }

    private readonly bool consolidate;
    private Node? rootNode;
    private readonly IEqualityComparer<TValue> valueComparer;

    [DoesNotReturn]
    private static Node ThrowDuplicateKey<TKey>(TKey key, string parameterName)
    {
        throw new ArgumentException($"Duplicate key '{key}' added.", parameterName);
    }

    public void Clear() => this.rootNode = null;

    public Ipv4SubnetDictionary<TValue> Clone() => new (this.consolidate, this.rootNode, this.valueComparer);

    public int Count => throw new NotImplementedException();

    public INode? RootNodeInterface => this.rootNode;

    [PublicAPI]
    public IEnumerable<Ipv4Address> Addresses => new AddressKeyCollection(this);
    
    [PublicAPI]
    public IEnumerable<Ipv4Subnet> Subnets => new SubnetKeyCollection(this);


}