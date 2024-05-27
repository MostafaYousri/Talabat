using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T :BaseEntity
    {
        // dbContext.Products.Where(p => p.id == Id).Include(p => p.ProductBrand).Include(p => p.ProductType)

        // Sign for Propert for where Condition[Where(p => p.id == Id)]

        public Expression<Func<T ,bool>> Criteria { get; set; }

        // Sign for Propert for List of Include [Include(p => p.ProductBrand).Include(p => p.ProductType)]

        public List<Expression<Func<T,object>>> Includes { get; set; }

        // Sign for Propert for OrderBy[OrderBy (p => p.name)]
        public Expression<Func<T , object>> OrderBy{ get; set; }

        // Sign for Propert for OrderBy[OrderBy (p => p.name)]
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        // Take(2)
        public int Take { get; set; }
        // Skip(2)
        public int Skip { get; set; }

        //
        public bool IsPaginationEnabled { get; set; }
    }
}
