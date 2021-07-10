using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Models
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            RoleAccounts = new HashSet<RoleAccount>();
            Invoices = new HashSet<Invoice>();
        }
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string NumberPhone { get; set; }
        public string password { get; set; }
        public string Adress { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public bool status { get; set; }
        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }


    }
}
