using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Designer_Offer.Services.Repositories
{
    internal class DbRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly PrimeContext _db;
        private readonly DbSet<T> _Set;

        public bool AutoSaveChanges { get; set; }

        public DbRepository()
        {
            _db = App.Host.Services.GetRequiredService<PrimeContext>();
            _Set = _db.Set<T>();
        }

        public virtual IQueryable<T> Items => _Set;

        public T Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            _db.Entry(item).State = EntityState.Added;

            if (AutoSaveChanges)
                _db.SaveChanges();

            return item;
        }

        public async Task<T> AddAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            _db.Entry(item).State = EntityState.Added;

            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);

            return item;
        }

        public T Get(int id)
        {
            return Items.SingleOrDefault(item => item.Id == id);
        }

        public async Task<T> GetAsync(int id, CancellationToken Cancel = default)
        {
            return await Items.SingleOrDefaultAsync(item => item.Id == id, Cancel)
                .ConfigureAwait(false);
        }

        public void Remove(int id)
        {
            var item = Get(id);

            if (item is null) return;
            //_db.Entry(item).State = EntityState.Deleted;
            _db.Set<T>().Remove(item);

            if (AutoSaveChanges)
                _db.SaveChanges();
        }

        public async Task RemoveAsync(int id, CancellationToken Cancel = default)
        {
            var item = Get(id);

            if (item is null) return;
            //_db.Entry(item).State = EntityState.Deleted;
            _db.Set<T>().Remove(item);

            if (AutoSaveChanges)
                await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Update(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                _db.SaveChanges();
        }

        public async Task UpdateAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }
    }
}
