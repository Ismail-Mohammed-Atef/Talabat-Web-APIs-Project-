using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repository.Interfaces;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreDbContext _context;
		private Hashtable _repository;

		public UnitOfWork(StoreDbContext context)
        {
			_context = context;
			_repository = new Hashtable();
		}
        public async Task<int> Complete() => await _context.SaveChangesAsync();
		

	

		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var type = typeof(TEntity);
			if (!_repository.ContainsKey(type)) {
				var repo = new GenericRepository<TEntity>(_context);
				_repository.Add(type, repo);
			}
			return _repository[type] as IGenericRepository<TEntity>;

		}

		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			await _context.DisposeAsync();
		}
	}
}
