#nullable enable

using System;

namespace NetworkPrimitives
{
    [AttributeUsage(
        validOn: AttributeTargets.Assembly 
            | AttributeTargets.Class 
            | AttributeTargets.Constructor 
            | AttributeTargets.Event 
            | AttributeTargets.Method
            | AttributeTargets.Property
            | AttributeTargets.Struct, 
        Inherited=false
    )]
    [ExcludeFromCodeCoverage("Internal")]
    internal sealed class ExcludeFromCodeCoverageAttribute : Attribute
    {
        public ExcludeFromCodeCoverageAttribute(string? reason = null) => Reason = Reason;
        public string? Reason { get; set; }
    }
}