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
        /// <summary>
        /// Get - Edit
        /// </summary>
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _db.Category.Find(id);
            if (obj == null) return NotFound();
            return View(obj);
        }
        /// Post - Edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }
        /// <summary>
        /// Get - Delete
        /// </summary>
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _db.Category.Find(id);
            if (obj == null) return NotFound();
            return View(obj);
        }
        /// Post - Edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int ? id)
        {
            var obj = _db.Category.Find(id);
            if (obj != null)
            {
                _db.Category.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else return NotFound();

            //return View(obj);
        }
    }

}
