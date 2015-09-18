using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Abstract
{
    public interface IRecordOperations<T> where T: Product
    {
        void Add(T item);
        void Update(T item);
        void Remove(T item);
    }
}
