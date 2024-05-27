using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string Id);

        Task<CustomerBasket?> UpdateAsync(CustomerBasket item);

        Task<bool> DeleteAsync(string Id);
    }
}
