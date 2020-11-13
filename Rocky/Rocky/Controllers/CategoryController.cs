using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objListOfCategory = _db.Category;
            return View(objListOfCategory);
        }
        /// <summary>
        /// Get - Create
        /// </summary>
        public IActionResult Create()
        {
            
            return View();
        }
        /// <summary>
        /// Post - Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(category);
        }
    }

}
