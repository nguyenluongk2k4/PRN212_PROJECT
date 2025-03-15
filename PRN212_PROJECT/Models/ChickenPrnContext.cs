using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PRN212_PROJECT.Models;

public partial class ChickenPrnContext : DbContext
{
    public static ChickenPrnContext Ins=new ChickenPrnContext();

    public ChickenPrnContext()
    {
        if (Ins == null)
        {
            Ins = this;
        }
    }

    public ChickenPrnContext(DbContextOptions<ChickenPrnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<ComboDetail> ComboDetails { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<TypeOfFood> TypeOfFoods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("MyCnn")); }
    }
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ChickenPRN; Trusted_Connection=SSPI;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.ComboId).HasName("PK__Combo__DD42580E9FF3F74F");

            entity.ToTable("Combo");

            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.ComboName).HasMaxLength(50);
        });

        modelBuilder.Entity<ComboDetail>(entity =>
        {
            entity.HasKey(e => e.ComboDetailId).HasName("PK__ComboDet__7C0A2CD94E431EC4");

            entity.ToTable("ComboDetail");

            entity.Property(e => e.ComboDetailId).HasColumnName("ComboDetailID");
            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");

            entity.HasOne(d => d.Combo).WithMany(p => p.ComboDetails)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__ComboDeta__Combo__3F466844");

            entity.HasOne(d => d.Food).WithMany(p => p.ComboDetails)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__ComboDeta__FoodI__3E52440B");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CB35B056A0");

            entity.ToTable("Food");

            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.FoodName).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(250);

            entity.HasOne(d => d.FoodTypeNavigation).WithMany(p => p.Foods)
                .HasForeignKey(d => d.FoodType)
                .HasConstraintName("FK__Food__FoodType__398D8EEE");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__0809337DCE51608F");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("customerName");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IsPaid).HasColumnName("isPaid");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2ACA30ACEC");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");

            entity.HasOne(d => d.Food).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__OrderDeta__FoodI__44FF419A");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__440B1D61");
        });

        modelBuilder.Entity<TypeOfFood>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__TypeOfFo__516F03957BA173CF");

            entity.ToTable("TypeOfFood");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
