using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        private IProductRepository repository;

        public int PageSize = 4;

        [HttpGet]
        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products.Where(p => category == null || p.Category == category).OrderBy(p => p.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                { 
                    CurrentPage = page, 
                    ItemsPerPage = PageSize, 
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                int id = 0;

                if (repository.Products.Count() > 0)
                {
                    id = repository.Products.LastOrDefault().ProductID + 1;
                }

                product.ProductID = id;

                repository.Add(product);

                return RedirectToAction("List");
            }

            return View(product);
        }
    }
}