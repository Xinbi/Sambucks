using System;
using System.Collections.Generic;
using System.Linq;

namespace Sambucks.Data.Entities
{
    public class OrderEntry
    {
        public int Id { get; set; }
        public Food FoodItem { get; set; }
        public Size Size { get; set; }
        public virtual Order Order { get; set; }

    }
}
