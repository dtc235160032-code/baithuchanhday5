using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using de1_part2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace de1_part2.Controllers
{
    public class ProductController : Controller
    {
        // giả lập CSDL
        public static List<Product> products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Product 1", Description = "Description 1", Price = 10.0m, CategoryId = 1 },
            new Product { ProductId = 2, Name = "Product 2", Description = "Description 2", Price = 20.0m, CategoryId = 2 },
            new Product { ProductId = 3, Name = "Product 3", Description = "Description 3", Price = 30.0m, CategoryId = 1 },
            new Product { ProductId = 4, Name = "Product 4", Description = "Description 4", Price = 40.0m, CategoryId = 3 },
            new Product { ProductId = 5, Name = "Product 5", Description = "Description 5", Price = 50.0m, CategoryId = 2 }
        };

        // Hàm Paging hỗ trợ tìm kiếm và sắp xếp theo ProductId hoặc Name
        public (ViewModel.ProductViewModel[] products, int pages, int page) Paging(int page, string orderBy = "ProductId", bool dsc = false, string searchString = null)
        {
            int pageSize = 3; // Số lượng sản phẩm trên mỗi trang

            // 1. Lấy dữ liệu kết hợp với Category
            var query = from p in products
                        join c in CategoryController.categories on p.CategoryId equals c.CategoryId
                        select new ViewModel.ProductViewModel
                        {
                            ProductId = p.ProductId,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                            CategoryName = c.Name
                        };

            // 2. Lọc theo từ khóa tìm kiếm (nếu có)
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            var listData = query.ToList();

            // 3. Sắp xếp theo ProductId hoặc Name (Tăng dần / Giảm dần)
            listData = (orderBy, dsc) switch
            {
                ("ProductId", true) => listData.OrderByDescending(p => p.ProductId).ToList(),
                ("ProductId", false) => listData.OrderBy(p => p.ProductId).ToList(),
                ("Name", true) => listData.OrderByDescending(p => p.Name).ToList(),
                ("Name", false) => listData.OrderBy(p => p.Name).ToList(),
                _ => listData.OrderBy(p => p.ProductId).ToList()
            };

            // 4. Tính toán tổng số trang
            int totalPages = (int)Math.Ceiling((double)listData.Count / pageSize);
            if (totalPages == 0) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            // 5. Phân trang SAU KHI đã sắp xếp và lọc
            var pagedProducts = listData.Skip((page - 1) * pageSize).Take(pageSize).ToArray();

            return (pagedProducts, totalPages, page);
        }

        // GET: ProductController
        public ActionResult Index(string searchString, string orderBy = "ProductId", bool dsc = false, int page = 1)
        {
            // Gọi hàm Paging xử lý toàn bộ logic tìm kiếm, sắp xếp và phân trang
            var result = Paging(page, orderBy, dsc, searchString);

            // Gán dữ liệu sang ViewData và ViewBag để hiển thị trên View
            ViewData["Pages"] = result.pages;
            ViewData["Page"] = result.page;
            ViewBag.searchString = searchString;
            ViewBag.OrderBy = orderBy;
            ViewBag.Dsc = dsc;

            return View(result.products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            var category = CategoryController.categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            var productViewModel = new ViewModel.ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = category?.Name
            };
            return View(productViewModel);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(
                CategoryController.categories, "CategoryId", "Name");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product createproduct)
        {
            if (ModelState.IsValid)
            {
                createproduct.ProductId = products.Count > 0 ? products.Max(p => p.ProductId) + 1 : 1;
                products.Add(createproduct);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(
                CategoryController.categories, "CategoryId", "Name", createproduct.CategoryId);
            return View(createproduct);
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(
                CategoryController.categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product editproduct)
        {
            if (ModelState.IsValid)
            {
                var product = products.FirstOrDefault(p => p.ProductId == editproduct.ProductId);
                if (product == null)
                {
                    return NotFound();
                }
                product.Name = editproduct.Name;
                product.Description = editproduct.Description;
                product.Price = editproduct.Price;
                product.CategoryId = editproduct.CategoryId;
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(
                CategoryController.categories, "CategoryId", "Name", editproduct.CategoryId);
            return View(editproduct);
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            var category = CategoryController.categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            ViewBag.CategoryName = category?.Name;
            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var product = products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            products.Remove(product);
            return RedirectToAction(nameof(Index));
        }
    }
}