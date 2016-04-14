namespace OnlineHotel.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OnlineHotelDbContext : DbContext
    {
        public OnlineHotelDbContext()
            : base("name=OnlineHotelDbContext")
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; }
        public virtual DbSet<BANGDUYETPHONG> BANGDUYETPHONGs { get; set; }
        public virtual DbSet<CONTACT> CONTACTs { get; set; }
        public virtual DbSet<CT_HOADON> CT_HOADON { get; set; }
        public virtual DbSet<HOADON> HOADONs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<LOAIPHONG> LOAIPHONGs { get; set; }
        public virtual DbSet<PHIEUTHUEPHONG> PHIEUTHUEPHONGs { get; set; }
        public virtual DbSet<PHONG> PHONGs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.UserName)
                .IsFixedLength();

            modelBuilder.Entity<CONTACT>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<CONTACT>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<HOADON>()
                .HasMany(e => e.CT_HOADON)
                .WithRequired(e => e.HOADON)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LOAIPHONG>()
                .Property(e => e.ChiTiet)
                .IsFixedLength();

            modelBuilder.Entity<LOAIPHONG>()
                .HasMany(e => e.PHONGs)
                .WithMany(e => e.LOAIPHONGs)
                .Map(m => m.ToTable("LOAIPHONGPHONGs"));
        }
    }
}
