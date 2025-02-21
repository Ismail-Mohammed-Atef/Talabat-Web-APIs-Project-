using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repository.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;
using Talabat.Repository.Specifications;

namespace Talabat.Repository.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreDbContext _context;

		public GenericRepository(StoreDbContext context)
        {
			_context = context;
		}
        public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			if (typeof(T) == typeof(Product))
			{
				return (IReadOnlyList<T>)await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
			}
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<T?> GetAsync(int id)
		{
			if (typeof(T) == typeof(Product))
			{
				return await _context.Products.Where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync() as T;
			}
			return await _context.Set<T>().FindAsync(id);
		}


		public  Task<T?> GetAsyncWithSpec(ISpecifications<T> spec)
		{
			return  SpecificationEvaluator<T>.GetQuery(_context.Set<T>(),spec).FirstOrDefaultAsync();
		}

		public async Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecifications<T> spec)
		{
			return await SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec).ToListAsync();
		}

		public async Task<int> GetCountAsync(ISpecifications<T> spec)
		{
			return await SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec).CountAsync();
		}

		public async Task Add(T entry)
		{
			await _context.Set<T>().AddAsync(entry);
		}

		public async void update(T entry)
		{
			 _context.Set<T>().Update(entry);
		}

		public void delete(T entry)
		{
			_context.Set<T>().Remove(entry);
		}
	}
}
