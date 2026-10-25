using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using ProductCatalog.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProductCatalog.Controllers
{
    public class ProductsController : Controller
    {
        private readonly string _filePath;

        public ProductsController(IWebHostEnvironment environment)
        {
            // Set the file path for the products JSON file
            _filePath = Path.Combine(
                environment.ContentRootPath,
                "data",
                "products.json"
            );

            // Load the products from the JSON file if it exists
            // Otherwise create an empty list
            if (!System.IO.File.Exists(_filePath))
            {
                System.IO.File.WriteAllText(_filePath, "[]");
            } else
            {
                var json = System.IO.File.ReadAllText(_filePath);
                var products = JsonSerializer.Deserialize<List<Product>>(json);
                if (products == null)
                {
                    System.IO.File.WriteAllText(_filePath, "[]");
                }
            }
        }

        // Save products
        private void SaveProducts(List<Product> products)
        {   
            //Serializing the list (Product)
            var json = JsonSerializer.Serialize(products);
            System.IO.File.WriteAllText(_filePath, json);
        }

        // Load products from the JSON file
        private List<Product> LoadProducts()
        {
            var json = System.IO.File.ReadAllText(_filePath);

            var products = JsonSerializer.Deserialize<List<Product>>(json);

            return products ?? new List<Product>();
        }

        // GET: /Products
        public IActionResult Index()
        {
            var products = LoadProducts();
            return View(products);
        }

        //Get details of each product
        public IActionResult Details(int Id)
        {
            var products = LoadProducts();
            var product = products.FirstOrDefault(p => p.Id == Id);
            if (product == null)
            {
                return NotFound();
            } else
            {
                return View(product);
            }
        }

        // GET: /Products/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var products = LoadProducts();
                product.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
                products.Add(product);
                SaveProducts(products);
                return RedirectToAction("Index");
            }
            else { return View(product); }
        }
    }
}