using System;
using System.ComponentModel;

namespace mmo_shared {

    //taken from https://stackoverflow.com/a/5473526

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    [ImmutableObject(true)]
    public class OrderAttribute : Attribute {
        private readonly int order;
        public int Order { get { return order; } }
        public OrderAttribute(int order) { this.order = order; }
    }
}
