using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;

            foreach(var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(x => x.ID == obj.CategoryID);
            }
            return View(objList);
        }
        /// <summary>
        /// Get - Upsert
        /// </summary>
        public IActionResult Upsert(int? id)
        {
            

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(x => 
                new SelectListItem 
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                })
            };

            if (id == null)
            {
                //for creating new produkt
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _db.Product.Find(id);
                if (productViewModel.Product == null)
                    return NotFound();
                return View(productViewModel);
            }
        }
        /// <summary>
        /// Post - Upsert
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.ID == 0)
                {
                    //Creating
                    string upload = webRootPath + WebContext.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    var filePath = Path.Combine(upload, fileName + extension);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;
                    _db.Product.Add(productVM.Product);
                    


                }
                else
                {
                    //Updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(x => x.ID == productVM.Product.ID);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebContext.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (FileStream fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(productVM.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ID.ToString()
            });
            return View(productVM);
        }
        
        /// <summary>
        /// Get - Delete
        /// </summary>
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product product = _db.Product.Include(x => x.Category).FirstOrDefault(x => x.ID == id);
            if (product == null) return NotFound();
            return View(product);
        }
        /// Post - Edit
        /// </summary>
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WebContext.ImagePath;
            
            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            
            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");          

        }
    }

}
