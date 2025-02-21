using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
	public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get; set; } = null;
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderBy { get; set; }
		public Expression<Func<T, object>> OrderByDesc { get; set; }
		public int Skip { get; set; }
		public int Take { get ; set ; }
		public bool IspaginationEnabled { get ; set; }

		public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }


        public void AddOrderBy(Expression<Func<T, object>> orderby)
        {
            OrderBy = orderby;
        }
		public void AddOrderByDesc(Expression<Func<T, object>> orderby)
		{
			OrderByDesc = orderby;
		}

		public void AddPagination(int take , int skip )
		{
				IspaginationEnabled = true;
				Skip = skip;
				Take = take;
			
		}
	}
}
