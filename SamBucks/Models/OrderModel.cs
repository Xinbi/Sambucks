using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sambucks.Models
{
    public class OrderModel
    {
        public List<LinkModel> Links { get; set; }

        public DateTime CurrentDate { get; set; }

        public string Status { get; set; }

        public IEnumerable<OrderEntryModel> Entries { get; set; }
    }
}
