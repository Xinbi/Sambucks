using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sambucks.Data.Entities
{
    public class Size
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual Food Food { get; set; }
    }
}
