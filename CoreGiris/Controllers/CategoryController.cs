using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGiris.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreGiris.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            var db = new MyContext();
            var data = db.Categories.Include(x=>x.Products)
                //.ThenInclude(x=>x.suppliers)
                .OrderBy(x=>x.CategoryName).ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var db=new MyContext();
            db.Categories.Add(new Category()
            {
                CategoryName = model.CategoryName,
            });
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            var db=new MyContext();
            var category = db.Categories.Include(x=>x.Products).FirstOrDefault(x=>x.Id==id);
            if (category == null)
            {
                TempData["Message"] = "Kategori bulunamadı";
                return RedirectToAction("Index");
            }

            if (category.Products.Count > 0)
            {
                TempData["Message"] = "Kategori silinemedi";
                return RedirectToAction("Index");
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Edit(int id = 0)
        {
            var db = new MyContext();

            var category = db.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                TempData["Message"] = "Kategori bulunamadı";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var db = new MyContext();

            var category = db.Categories.FirstOrDefault(x => x.Id == model.Id);
            if (category == null)
            {
                TempData["Message"] = "Kategori bulunamadı";
                return RedirectToAction("Index");
            }
            category.CategoryName = model.CategoryName;
            db.SaveChanges();
            TempData["Message"] = "Kategori Güncelleme İşlemi Başarılı";

            return RedirectToAction("Index");
        }

    }
}