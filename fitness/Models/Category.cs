using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int? ParentId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
