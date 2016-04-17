using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sambucks.Models
{
    public class OrderEntryModel
    {
        public List<LinkModel> Links { get; set; }

        public string FoodDescription { get; set; }

        public string SizeDescription { get; set; }

        public string SizeUrl { get; set; }
    }
}
