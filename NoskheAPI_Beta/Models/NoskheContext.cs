using Microsoft.EntityFrameworkCore;

namespace NoskheAPI_Beta.Models
{
    public class NoskheContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=NoskheDBNew.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CosmeticShoppingCart>()
                .HasKey(bc => new { bc.CosmeticId, bc.ShoppingCartId });

            modelBuilder.Entity<CosmeticShoppingCart>()
                .HasOne(bc => bc.Cosmetic)
                .WithMany(b => b.CosmeticShoppingCarts)
                .HasForeignKey(bc => bc.CosmeticId);

            modelBuilder.Entity<CosmeticShoppingCart>()
                .HasOne(bc => bc.ShoppingCart)
                .WithMany(c => c.CosmeticShoppingCarts)
                .HasForeignKey(bc => bc.ShoppingCartId);
            // -----------------------------------------------------------
            modelBuilder.Entity<MedicineShoppingCart>()
                .HasKey(bc => new { bc.MedicineId, bc.ShoppingCartId });

            modelBuilder.Entity<MedicineShoppingCart>()
                .HasOne(bc => bc.Medicine)
                .WithMany(b => b.MedicineShoppingCarts)
                .HasForeignKey(bc => bc.MedicineId);

            modelBuilder.Entity<MedicineShoppingCart>()
                .HasOne(bc => bc.ShoppingCart)
                .WithMany(c => c.MedicineShoppingCarts)
                .HasForeignKey(bc => bc.ShoppingCartId);
        }
        public DbSet<Cosmetic> Cosmetics { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Notation> Notations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CosmeticShoppingCart> CosmeticShoppingCarts { get; set; }
        public DbSet<MedicineShoppingCart> MedicineShoppingCarts { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Occurrence> Occurrences { get; set; }
        public DbSet<Settle> Settles { get; set; }
        public DbSet<CustomerTextMessage> CustomerTextMessages { get; set; }
        public DbSet<CourierTextMessage> CourierTextMessages { get; set; }
        public DbSet<CustomerToken> CustomerTokens { get; set; }
        public DbSet<CustomerResetPasswordToken> CustomerResetPasswordToken { get; set; }
        public DbSet<PharmacyToken> PharmacyTokens { get; set; }
        public DbSet<ServiceMapping> ServiceMappings { get; set; }
    }
}