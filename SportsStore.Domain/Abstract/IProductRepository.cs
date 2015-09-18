using System.Collections.Generic;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Abstract
{
    public interface IProductRepository : IRecordOperations<Product>
    {
        IEnumerable<Product> Products { get; }
    }
}
