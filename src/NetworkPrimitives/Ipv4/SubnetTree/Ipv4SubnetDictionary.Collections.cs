using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS1591

namespace NetworkPrimitives.Ipv4;

public partial class Ipv4SubnetDictionary<TValue> 
    : IReadOnlyDictionary<Ipv4Address, TValue>,
        //IDictionary<Ipv4Address, TValue>,
        //ICollection<KeyValuePair<Ipv4Address,TValue>>,
        
        IEnumerable<KeyValuePair<Ipv4Subnet, IReadOnlyList<TValue>>>,
        IReadOnlyCollection<KeyValuePair<Ipv4Subnet, IReadOnlyList<TValue>>>,
        IReadOnlyDictionary<Ipv4Subnet, IReadOnlyList<TValue>>
{
    private static NodeEnumerator GetNodeEnumerator(Node? node) => new (node);
    private NodeEnumerator GetNodeEnumerator() => GetNodeEnumerator(this.rootNode);
    
    public IEnumerable<(Ipv4Subnet Subnet, TValue? Value, Ipv4Subnet? Left, Ipv4Subnet? Right)> Nodes() 
        => this.GetNodeEnumerator().Select(n => n.ToNodeTuple());


    public IEnumerable<(Ipv4Subnet Subnet, TValue Value)> SubnetsAsTuples()
    {
        using var enumerator = GetNodeEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current is IValueNode valueNode)
                yield return (valueNode.Subnet, valueNode.Value);
        }
    }
    private IEnumerable<(Ipv4Address Address, TValue Value)> AddressesAsTuples()
    {
        using var enumerator = GetNodeEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current is not IValueNode valueNode) continue;
            foreach (var address in valueNode.Subnet.GetAllAddresses().ToEnumerable())
                yield return (address, valueNode.Value);
        }
    }
    private IEnumerable<KeyValuePair<Ipv4Subnet, TValue>> SubnetsAsKeyValuePairs()
    {
        using var enumerator = GetNodeEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current is IValueNode valueNode)
                yield return new(valueNode.Subnet, valueNode.Value);
        }
    }
    private IEnumerable<KeyValuePair<Ipv4Address, TValue>> AddressesAsKeyValuePairs()
    {
        using var enumerator = GetNodeEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current is not IValueNode valueNode) continue;
            foreach (var address in valueNode.Subnet.GetAllAddresses().ToEnumerable())
                yield return new(address, valueNode.Value);
        }
    }
    
    
    
    #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator() 
        => this.SubnetsAsKeyValuePairs().GetEnumerator();

    #endregion IEnumerable

    #region IEnumerable<KeyValuePair<Ipv4Address, TValue>>
    
    IEnumerator<KeyValuePair<Ipv4Address, TValue>> IEnumerable<KeyValuePair<Ipv4Address, TValue>>.GetEnumerator()
        => this.AddressesAsKeyValuePairs().GetEnumerator();
    
    #endregion IEnumerable<KeyValuePair<Ipv4Address, TValue>>

    #region IReadOnlyDictionary<Ipv4Address, TValue>

    IEnumerable<TValue> IReadOnlyDictionary<Ipv4Address, TValue>.Values
        => new ValueCollection(this);
    
    IEnumerable<Ipv4Address> IReadOnlyDictionary<Ipv4Address, TValue>.Keys 
        => new AddressKeyCollection(this);
    
    #endregion IReadOnlyDictionary<Ipv4Address, TValue>


    #region ICollection<KeyValuePair<Ipv4Address,TValue>>
    /*
    void ICollection<KeyValuePair<Ipv4Address, TValue>>.Add(KeyValuePair<Ipv4Address, TValue> item)
        => this.Add(item.Key, item.Value);

    bool ICollection<KeyValuePair<Ipv4Address, TValue>>.Contains(KeyValuePair<Ipv4Address, TValue> item)
        => this.TryGetValue(item.Key, out var value) && this.valueComparer.Equals(value, item.Value);
    
    void ICollection<KeyValuePair<Ipv4Address, TValue>>.CopyTo(KeyValuePair<Ipv4Address, TValue>[] array, int arrayIndex) 
        => throw new NotImplementedException();
    
    bool ICollection<KeyValuePair<Ipv4Address, TValue>>.Remove(KeyValuePair<Ipv4Address, TValue> item) 
        => ((ICollection<KeyValuePair<Ipv4Address, TValue>>)this).Contains(item) 
            && this.Remove(item.Key);
    
    bool ICollection<KeyValuePair<Ipv4Address, TValue>>.IsReadOnly => false;
    */
    #endregion ICollection<KeyValuePair<Ipv4Address,TValue>>
    
    
    #region IDictionary<Ipv4Address, TValue>

    /*
    bool IDictionary<Ipv4Address, TValue>.Remove(Ipv4Address key)
        => this.Remove(key);
    
    ICollection<TValue> IDictionary<Ipv4Address, TValue>.Values 
        => new ValueCollection(this);
    
    ICollection<Ipv4Address> IDictionary<Ipv4Address, TValue>.Keys 
        => new AddressKeyCollection(this);
    */
    #endregion IDictionary<Ipv4Address, TValue>


    #region IEnumerable<KeyValuePair<Ipv4Subnet,IReadOnlyList<TValue>>>

    IEnumerator<KeyValuePair<Ipv4Subnet, IReadOnlyList<TValue>>> IEnumerable<KeyValuePair<Ipv4Subnet, IReadOnlyList<TValue>>>.GetEnumerator()
    {
        var dict = this.Clone();
        return dict.GetNodeEnumerator()
            .OfType<IValueNode>()
            .Select(ToKeyValuePair).GetEnumerator();
        KeyValuePair<Ipv4Subnet, IReadOnlyList<TValue>> ToKeyValuePair(IValueNode node) 
            => new (node.Subnet, dict.Get(node.Subnet) ?? Array.Empty<TValue>());
    }

    #endregion IEnumerable<KeyValuePair<Ipv4Subnet,IReadOnlyList<TValue>>>


    #region IReadOnlyDictionary<Ipv4Subnet, IReadOnlyList<TValue>>

    IEnumerable<Ipv4Subnet> IReadOnlyDictionary<Ipv4Subnet, IReadOnlyList<TValue>>.Keys
        => new SubnetKeyCollection(this);

    IEnumerable<IReadOnlyList<TValue>> IReadOnlyDictionary<Ipv4Subnet, IReadOnlyList<TValue>>.Values
        => throw new NotImplementedException();

    IReadOnlyList<TValue> IReadOnlyDictionary<Ipv4Subnet, IReadOnlyList<TValue>>.this[Ipv4Subnet subnet] 
        => Get(subnet) ?? Array.Empty<TValue>();

    #endregion IReadOnlyDictionary<Ipv4Subnet, IReadOnlyList<TValue>>
    
}