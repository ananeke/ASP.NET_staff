using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList= _db.ApplicationType;
            return View(objList);
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
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
            
        }
        /// <summary>
        /// Get - Edit
        /// </summary>
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _db.ApplicationType.Find(id);
            if (obj == null) return NotFound();
            return View(obj);
        }
        /// <summary>
        /// Post - Edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);

        }
        /// <summary>
        /// Get - Delete
        /// </summary>
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null) return NotFound();
            var obj = _db.ApplicationType.Find(id);
            if (obj == null) return NotFound();
            return View(obj);
        }
        /// <summary>
        /// Post - Delete
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.ApplicationType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.ApplicationType.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
