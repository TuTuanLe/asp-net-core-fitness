using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public string PathPhoto { get; set; }
    }
}
