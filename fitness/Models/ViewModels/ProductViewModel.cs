using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public SelectList Categories { get; set; }
    }
}
