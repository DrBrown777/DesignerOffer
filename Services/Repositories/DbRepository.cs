using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Designer_Offer.Services.Repositories
{
    class DbRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly PrimeContext _db;
        private readonly DbSet<T> _Set;

        public bool AutoSaveChanges { get; set; }

        public DbRepository(PrimeContext db)
        {
            _db = db;
            _Set = _db.Set<T>();
            AutoSaveChanges = true;
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
            
            _db.Set<T>().Remove(item);

            if (AutoSaveChanges)
                _db.SaveChanges();
        }

        public async Task RemoveAsync(int id, CancellationToken Cancel = default)
        {
            var item = Get(id);

            if (item is null) return;
            
            _db.Set<T>().Remove(item);

            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
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

    internal class BuildRepository : DbRepository<Builds>
    {
        public override IQueryable<Builds> Items => base.Items
            .Include(item => item.Clients)
            .Include(item => item.Projects)
            .Include(item => item.Projects.Employees)
            .Include(item => item.Projects.Offers);

        public BuildRepository(PrimeContext db) : base(db) { }
    }

    internal class ClientRepository : DbRepository<Clients>
    {
        public override IQueryable<Clients> Items => base.Items
            .Include(item => item.Builds);

        public ClientRepository(PrimeContext db) : base(db) { }
    }

    internal class EmployeeRepository : DbRepository<Employees>
    {
        public override IQueryable<Employees> Items => base.Items
            .Include(item => item.UsersData)
            .Include(item => item.Companies);

        public EmployeeRepository(PrimeContext db) : base(db) { }
    }

    internal class SupplierRepository : DbRepository<Suppliers>
    {
        public override IQueryable<Suppliers> Items => base.Items
            .Include(item => item.Products);

        public SupplierRepository(PrimeContext db) : base(db) { }
    }

    internal class ManufacturerRepository : DbRepository<Manufacturers>
    {
        public override IQueryable<Manufacturers> Items => base.Items
            .Include(item => item.Products);

        public ManufacturerRepository(PrimeContext db) : base(db) { }
    }
    
    internal class OfferRepository : DbRepository<Offers>
    {
        public override IQueryable<Offers> Items => base.Items
            .Include(item => item.Parts);
        public OfferRepository(PrimeContext db) : base(db) { }
    }

    internal class ProductRepository : DbRepository<Products>
    {
        public override IQueryable<Products> Items => base.Items
            .Include(item => item.Categories.Sections)
            .Include(item => item.ProductPart);

        public ProductRepository(PrimeContext db) : base(db) { }
    }

    internal class PartsRepository : DbRepository<Parts>
    {
        public override IQueryable<Parts> Items => base.Items
            .Include(item => item.ProductPart)
            .Include(item => item.InstallPart);
        public PartsRepository(PrimeContext db) : base(db) { }
    }
}
