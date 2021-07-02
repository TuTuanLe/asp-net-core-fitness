using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Models
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            RoleAccounts = new HashSet<RoleAccount>();
        }
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string status { get; set; }
        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}
