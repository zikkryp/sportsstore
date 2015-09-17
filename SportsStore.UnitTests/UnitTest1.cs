using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        #region Can Paginate

        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(
                new Product[] 
                {
                    new Product{ ProductID = 1, Name = "P1"},
                    new Product{ ProductID = 2, Name = "P2"},
                    new Product{ ProductID = 3, Name = "P3"},
                    new Product{ ProductID = 4, Name = "P4"},
                    new Product{ ProductID = 5, Name = "P5"}
                }
            );

            ProductController controller = new ProductController(mock.Object);

            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            Product[] productArray = result.Products.ToArray();

            Assert.IsTrue(productArray.Length == 2);

            Assert.AreEqual(productArray[0].Name, "P4");
            Assert.AreEqual(productArray[1].Name, "P5");
        }

        #endregion

        #region Can generate links

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(
                  @"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        #endregion

        #region Can Send Pagination View Model

        /// <summary> Controller for Can_Send_Pagination_View_Model
        ///public ViewResult List(int page = 1)
        ///{
        ///    ProductsListViewModel model = new ProductsListViewModel
        ///    {
        ///        Products = repository.Products.OrderBy(p => p.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
        ///        PagingInfo = new PagingInfo
        ///        {
        ///            CurrentPage = page,
        ///            ItemsPerPage = PageSize,
        ///            TotalItems = repository.Products.Count()
        ///        }
        ///    };
        ///    return View(model);
        ///}
        /// </summary>
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ ProductID = 1, Name = "P1"},
                    new Product{ ProductID = 2, Name = "P2"},
                    new Product{ ProductID = 3, Name = "P3"},
                    new Product{ ProductID = 4, Name = "P4"},
                    new Product{ ProductID = 5, Name = "P5"}
                }
            );

            ProductController controller = new ProductController(mock.Object);

            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            PagingInfo pagingInfo = result.PagingInfo;

            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }

        #endregion

        #region Can Filter Products

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ ProductID = 1, Name = "P1", Category = "Cat1"},
                    new Product{ ProductID = 2, Name = "P2", Category = "Cat2"},
                    new Product{ ProductID = 3, Name = "P3", Category = "Cat1"},
                    new Product{ ProductID = 4, Name = "P4", Category = "Cat2"},
                    new Product{ ProductID = 5, Name = "P5", Category = "Cat3"}
                }
            );

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 4;

            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        #endregion

        #region Can Create Categories

        [TestMethod]
        public void Can_Create_categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID = 1, Name = "P1", Category = "Apples"},
                new Product{ ProductID = 2, Name = "P2", Category = "Apples"},
                new Product{ ProductID = 3, Name = "P3", Category = "Potatoes"},
                new Product{ ProductID = 4, Name = "P4", Category = "Oranges"}
            });

            NavController target = new NavController(mock.Object);

            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Potatoes");
        }

        #endregion

        #region Indicate Selected Category

        [TestMethod]
        public void Indicate_Selected_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(
                new Product[]{
                    new Product { ProductID = 1, Name = "P1", Category = "Apples"},
                    new Product { ProductID = 4, Name = "P2", Category = "Oranges"}
                });

            NavController target = new NavController(mock.Object);

            string category = "Apples";

            string result = target.Menu(category).ViewBag.SelectedCategory;

            Assert.AreEqual(category, result);
        }

        #endregion

        #region Generate Category Specific Product Count

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ ProductID = 1, Name = "P1", Category = "Cat1"},
                    new Product{ ProductID = 2, Name = "P2", Category = "Cat2"},
                    new Product{ ProductID = 3, Name = "P3", Category = "Cat1"},
                    new Product{ ProductID = 4, Name = "P4", Category = "Cat2"},
                    new Product{ ProductID = 5, Name = "P5", Category = "Cat3"}
                }
            );

            ProductController controller = new ProductController(mock.Object);

            int res = ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;
            Assert.AreEqual(res, 2);
            Assert.AreEqual(resAll, 5);
        }

        #endregion
    }
}
