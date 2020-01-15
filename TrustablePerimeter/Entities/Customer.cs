using System.Collections.Generic;

namespace TrustablePerimeter.Entities
{
    public class Customer : AbstractEntity
    {
        public Account Account { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
