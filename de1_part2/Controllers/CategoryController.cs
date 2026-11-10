using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using de1_part2.Models;
namespace de1_part2.Controllers
{
    public class CategoryController : Controller
    {
        //giả lập CSDL
        public static List<Category> categories = new List<Category>
        {
            new Category { CategoryId = 1, Name = "Category 1", Description = "Description 1", CreatedDate = new DateTime(31/10/2026)},
            new Category { CategoryId = 2, Name = "Category 2", Description = "Description 2", CreatedDate = new DateTime(31/10/2026)},
            new Category { CategoryId = 3, Name = "Category 3", Description = "Description 3", CreatedDate = new DateTime(31/10/2026)}
        };

        // GET: CategoryController
        public ActionResult Index(String searchString)
        {   
            var categoryList = categories;
            if(!String.IsNullOrEmpty(searchString))
            {
                categoryList = categories.Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewBag.SearchString = searchString;
            return View(categoryList);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var category = categories.FirstOrDefault(c => c.CategoryId == id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CategoryId = categories.Count + 1;
                categories.Add(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            var category = categories.FirstOrDefault(c => c.CategoryId == id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            var editcategory = categories.FirstOrDefault(c => c.CategoryId == id);
            if (editcategory == null)
            {
                return NotFound();
            }
            editcategory.Name = category.Name;
            editcategory.Description = category.Description;
            editcategory.CreatedDate = category.CreatedDate;
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            var category = categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            categories.RemoveAll(c => c.CategoryId == id);
            return RedirectToAction(nameof(Index));
        }
    }
}
