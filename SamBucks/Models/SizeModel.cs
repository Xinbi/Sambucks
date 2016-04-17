using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sambucks.Models
{
    public class SizeModel
    {
        public List<LinkModel> Links { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
