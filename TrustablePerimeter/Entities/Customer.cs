using System;
using System.Collections.Generic;

namespace TrustablePerimeter.Entities
{
    public class Customer
    {
        public Account[] Accounts { get; private set; }
        public string Name { get; private set; }
        public int Id { get; private set; }

        public Customer(IReadOnlyDictionary<string, object> data)
        {

        }
    }
}
