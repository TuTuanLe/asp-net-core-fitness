﻿using fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.ViewComponents
{

    [ViewComponent(Name ="CartTop")]
    public class CartTopViewComponent: ViewComponent
    {
        private DatabaseContext db;

        public CartTopViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        //async Task<IViewComponentResult>
        public IViewComponentResult Invoke()
        {
            List<Category> categories = db.Categories.Where(c => c.Status).ToList();
            return View("Index", categories);
        }
    }
}
