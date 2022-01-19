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

        public virtual DbSet<Builds> Builds { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<Configs> Configs { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<InstallPart> InstallPart { get; set; }
        public virtual DbSet<Installs> Installs { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<Offers> Offers { get; set; }
        public virtual DbSet<Parts> Parts { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<ProductPart> ProductPart { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Sections> Sections { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<UsersData> UsersData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Builds>()
                .HasOptional(e => e.Projects)
                .WithRequired(e => e.Builds)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Installs)
                .WithOptional(e => e.Categories)
                .HasForeignKey(e => e.Category_Id);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Categories)
                .HasForeignKey(e => e.Category_Id);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Sections)
                .WithMany(e => e.Categories)
                .Map(m => m.ToTable("SectionCategory").MapLeftKey("Category_Id").MapRightKey("Section_Id"));

            modelBuilder.Entity<Clients>()
                .HasMany(e => e.Builds)
                .WithOptional(e => e.Clients)
                .HasForeignKey(e => e.Client_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Companies>()
                .HasMany(e => e.Employees)
                .WithOptional(e => e.Companies)
                .HasForeignKey(e => e.Company_Id);

            modelBuilder.Entity<Companies>()
                .HasMany(e => e.Positions)
                .WithMany(e => e.Companies)
                .Map(m => m.ToTable("CompanyPosition").MapLeftKey("Company_Id").MapRightKey("Position_Id"));

            modelBuilder.Entity<Employees>()
                .HasMany(e => e.Projects)
                .WithOptional(e => e.Employees)
                .HasForeignKey(e => e.Employee_Id);

            modelBuilder.Entity<Employees>()
                .HasOptional(e => e.UsersData)
                .WithRequired(e => e.Employees)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Installs>()
                .HasMany(e => e.InstallPart)
                .WithRequired(e => e.Installs)
                .HasForeignKey(e => e.Install_Id);

            modelBuilder.Entity<Manufacturers>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Manufacturers)
                .HasForeignKey(e => e.Manufacturer_Id);

            modelBuilder.Entity<Offers>()
                .HasOptional(e => e.Configs)
                .WithRequired(e => e.Offers)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Offers>()
                .HasMany(e => e.Parts)
                .WithRequired(e => e.Offers)
                .HasForeignKey(e => e.Offer_Id);

            modelBuilder.Entity<Parts>()
                .HasMany(e => e.InstallPart)
                .WithRequired(e => e.Parts)
                .HasForeignKey(e => e.Part_Id);

            modelBuilder.Entity<Parts>()
                .HasMany(e => e.ProductPart)
                .WithRequired(e => e.Parts)
                .HasForeignKey(e => e.Part_Id);

            modelBuilder.Entity<Positions>()
                .HasMany(e => e.Employees)
                .WithOptional(e => e.Positions)
                .HasForeignKey(e => e.Position_Id);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.ProductPart)
                .WithRequired(e => e.Products)
                .HasForeignKey(e => e.Product_Id);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.Suppliers)
                .WithMany(e => e.Products)
                .Map(m => m.ToTable("ProductSupplier").MapLeftKey("Product_Id").MapRightKey("Supplier_Id"));

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.Offers)
                .WithRequired(e => e.Projects)
                .HasForeignKey(e => e.Project_Id);

            modelBuilder.Entity<Sections>()
                .HasMany(e => e.Offers)
                .WithOptional(e => e.Sections)
                .HasForeignKey(e => e.Section_Id);

            modelBuilder.Entity<Units>()
                .HasMany(e => e.Installs)
                .WithOptional(e => e.Units)
                .HasForeignKey(e => e.Unit_Id);

            modelBuilder.Entity<Units>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Units)
                .HasForeignKey(e => e.Unit_id);
        }
    }
}
