using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> spec)
        {
            var query = inputQuery;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
			if (spec.OrderByDesc != null)
			{
				query = query.OrderByDescending(spec.OrderByDesc);
			}
            if (spec.IspaginationEnabled)
            {
                query= query.Skip(spec.Skip).Take(spec.Take);
            }

			query = spec.Includes.Aggregate(query, (CurrentQuery, QueryExpression) => CurrentQuery.Include(QueryExpression));

            return query;
        }
    }
}
