using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[] { 
                    new Product{ ProductID = 1, Name = "P1", Category = "Vegetables", Description = "apple", Price = 100M }
                }.AsQueryable());

            Cart cart = new Cart();

            CartController target = new CartController(mock.Object);

            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_Goes_To_Cart_Screen()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(
                new Product[] { 
                    new Product{ ProductID = 1, Name = "P1", Category = "Vegetables", Description = "apple", Price = 100M }
                }.AsQueryable());

            Cart cart = new Cart();

            CartController target = new CartController(mock.Object);

            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_cart_Contents()
        {
            Cart cart = new Cart();

            CartController target = new CartController(null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        //[TestMethod]
        //public void Can_Add_New_Lines()
        //{
        //    //Arrange
        //    Product p1 = new Product { ProductID = 1, Name = "p1" };
        //    Product p2 = new Product { ProductID = 2, Name = "p2" };

        //    //Arrange
        //    Cart target = new Cart();

        //    //Act
        //    target.AddItem(p1, 1);
        //    target.AddItem(p2, 1);

        //    CartLine[] results = target.Lines.ToArray();

        //    //Assert
        //    Assert.AreEqual(results[0].Product, p1);
        //    Assert.AreEqual(results[1].Product, p2);
        //}

        //[TestMethod]
        //public void Can_Remove_Line()
        //{
        //    //Arrange
        //    Product p1 = new Product { ProductID = 1, Name = "p1" };
        //    Product p2 = new Product { ProductID = 2, Name = "p2" };
        //    Product p3 = new Product { ProductID = 3, Name = "p3" };

        //    //Arrange
        //    Cart target = new Cart();

        //    //act
        //    target.AddItem(p1, 1);
        //    target.AddItem(p2, 3);
        //    target.AddItem(p3, 5);
        //    target.AddItem(p2, 1);

        //    //act
        //    target.RemoveLine(p2);

        //    //Assert
        //    Assert.AreEqual(target.Lines.Where(p => p.Product == p2).Count(), 0);
        //    Assert.AreEqual(target.Lines.Count(), 2);
        //}

        //[TestMethod]
        //public void Calculate_Cart_Total()
        //{
        //    Product p1 = new Product { ProductID = 1, Name = "p1", Price = 100M };
        //    Product p2 = new Product { ProductID = 2, Name = "p2", Price = 50M };

        //    Cart target = new Cart();

        //    target.AddItem(p1, 1);
        //    target.AddItem(p2, 1);
        //    target.AddItem(p1, 3);

        //    decimal result = target.ComputeTotalValue();

        //    Assert.AreEqual(result, 450M);
        //}

        //[TestMethod]
        //public void Can_Clear_Contents()
        //{
        //    Product p1 = new Product { ProductID = 1, Name = "p1", Price = 100M };
        //    Product p2 = new Product { ProductID = 2, Name = "p2", Price = 50M };

        //    Cart target = new Cart();

        //    target.AddItem(p1, 1);
        //    target.AddItem(p2, 1);

        //    target.Clear();

        //    Assert.AreEqual(target.Lines.Count(), 0);
        //}
    }
}
