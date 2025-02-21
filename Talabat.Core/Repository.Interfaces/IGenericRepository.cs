using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repository.Interfaces
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<IReadOnlyList<T>> GetAllAsync();

		Task<T?> GetAsync(int id);
		Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecifications<T> spec);

		Task<T?> GetAsyncWithSpec(ISpecifications<T> spec);

		Task<int> GetCountAsync(ISpecifications<T> spec);

		Task Add(T entry);
		void update(T entry);
		void delete(T entry);
	}
}
