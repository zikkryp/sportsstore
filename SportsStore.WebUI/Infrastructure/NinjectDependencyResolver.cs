using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;

            AddBindings();
        }

        private IKernel kernel;

        public object GetService(Type service)
        {
            return this.kernel.TryGet(service);
        }

        public IEnumerable<object> GetServices(Type service)
        {
            return this.kernel.GetAll(service);
        }

        private void AddBindings()
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
        }

        //public void AddBindings()
        //{
        //    Mock<IProductRepository> mock = new Mock<IProductRepository>();
        //    mock.Setup(m => m.Products).Returns(new List<Product> {
        //        new Product{ Name = "Football", Price = 25},
        //        new Product{ Name = "Surf board", Price = 179},
        //        new Product{ Name = "Running shoes", Price = 95}
        //    });

        //    kernel.Bind<IProductRepository>().ToConstant(mock.Object);
        //}
    }
}