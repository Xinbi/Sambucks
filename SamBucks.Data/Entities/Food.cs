using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sambucks.Data.Entities
{
    public class Food
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }

        public virtual ICollection<Size> Sizes { get; set; }
    }
}
