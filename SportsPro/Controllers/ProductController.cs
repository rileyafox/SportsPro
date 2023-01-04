using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private SportsProContext context { get; set; }

        public ProductController(SportsProContext ctx)
        {
            context = ctx;
        }

        [Route("[controller]s")]
        public IActionResult List()
        {
            List<Product> products = context.Products.OrderBy(p => p.ReleaseDate).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("AddEdit", new Product());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var product = context.Products.Find(id);
            return View("AddEdit", product);
        }

        [HttpPost]
        public IActionResult Save(Product product)
        {
            //Declare a string variable to build the specific success message to be stored in TempData
            string successMessage;


            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                {
                    context.Products.Add(product);
                    successMessage = product.Name + " was added.";
                }
                else
                {
                    context.Products.Update(product);
                    successMessage = product.Name + " was updated. ";
                }
                context.SaveChanges();
                //Use TempData to store the success message
                TempData["message"] = successMessage;
                return RedirectToAction("List");
            }
            else
            {
                if (product.ProductID == 0)
                {
                    ViewBag.Action = "Add";
                }
                else
                {
                    ViewBag.Action = "Edit";
                }                    
                return View(product);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            var p = context.Products.Find(product.ProductID);
            string productName = product.Name;

            context.Products.Remove(product);
            context.SaveChanges();
            //Use tempData to store a success message after a successful delete.
            TempData["message"] = productName + " was deleted.";

            return RedirectToAction("List");
        }
    }
}