using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvalutor<T> where T:BaseEntity
    {
        // Fun To Build Query 
        // dbContext.Set<T>().Where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType)

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery , ISpecifications<T> spec)
        {
            var Query = inputQuery; // dbContext.Set<T>().
            
            if (spec.Criteria is not null) //p => p.Id == id
                Query = Query.Where(spec.Criteria); // dbContext.Set<T>().Where(p => p.Id == id)

            if (spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);
            if (spec.OrderByDescending is not null)
                Query = Query.OrderByDescending(spec.OrderByDescending);

            if (spec.IsPaginationEnabled)
                Query = Query.Skip(spec.Skip).Take(spec.Take);

            // p => p.ProductBrand   p => p.ProductType
            Query = spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

            // dbContext.Set<T>().Where(p => p.Id == id).Include(p => p.ProductBrand)
            // dbContext.Set<T>().Where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType)

            return Query;
        }
    }
}
