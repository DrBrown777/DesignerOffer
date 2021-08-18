using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Designer_Offer.Data
{
    public partial class PrimeContext : DbContext
    {
        public PrimeContext()
            : base("name=PrimeContext")
        {
        }

        public virtual DbSet<Build> Build { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Install> Install { get; set; }
        public virtual DbSet<InstallPart> InstallPart { get; set; }
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Offer> Offer { get; set; }
        public virtual DbSet<Part> Part { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductPart> ProductPart { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UserData> UserData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Build>()
                .HasOptional(e => e.Project)
                .WithRequired(e => e.Build)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Install)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.Category_Id);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Product)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.Category_Id);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Section)
                .WithMany(e => e.Category)
                .Map(m => m.ToTable("SectionCategory"));

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Build)
                .WithOptional(e => e.Client)
                .HasForeignKey(e => e.Client_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Employee)
                .WithOptional(e => e.Company)
                .HasForeignKey(e => e.Company_Id);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Position)
                .WithMany(e => e.Company)
                .Map(m => m.ToTable("CompanyPosition"));

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Project)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_Id);

            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.UserData)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Install>()
                .HasMany(e => e.InstallPart)
                .WithRequired(e => e.Install)
                .HasForeignKey(e => e.Install_Id);

            modelBuilder.Entity<Manufacturer>()
                .HasMany(e => e.Product)
                .WithOptional(e => e.Manufacturer)
                .HasForeignKey(e => e.Manufacturer_Id);

            modelBuilder.Entity<Offer>()
                .HasOptional(e => e.Config)
                .WithRequired(e => e.Offer)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Offer>()
                .HasMany(e => e.Part)
                .WithRequired(e => e.Offer)
                .HasForeignKey(e => e.Offer_Id);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.InstallPart)
                .WithRequired(e => e.Part)
                .HasForeignKey(e => e.Part_Id);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.ProductPart)
                .WithRequired(e => e.Part)
                .HasForeignKey(e => e.Part_Id);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Employee)
                .WithOptional(e => e.Position)
                .HasForeignKey(e => e.Position_Id);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductPart)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_Id);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Supplier)
                .WithMany(e => e.Product)
                .Map(m => m.ToTable("ProductSupplier"));

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Offer)
                .WithRequired(e => e.Project)
                .HasForeignKey(e => e.Project_Id);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.Offer)
                .WithOptional(e => e.Section)
                .HasForeignKey(e => e.Section_Id);

            modelBuilder.Entity<Unit>()
                .HasMany(e => e.Install)
                .WithOptional(e => e.Unit)
                .HasForeignKey(e => e.Unit_Id);

            modelBuilder.Entity<Unit>()
                .HasMany(e => e.Product)
                .WithOptional(e => e.Unit)
                .HasForeignKey(e => e.Unit_id);
        }
    }
}
