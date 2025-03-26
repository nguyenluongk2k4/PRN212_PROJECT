using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PRN212_PROJECT.Models;

public partial class ChickenPrnContext : DbContext
{
    public static ChickenPrnContext Ins = new ChickenPrnContext();
    public ChickenPrnContext()
    {
        if (Ins == null) Ins = this;

    }

    public ChickenPrnContext(DbContextOptions<ChickenPrnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<ComboDetail> ComboDetails { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<OrderDetailCombo> OrderDetailCombos { get; set; }

    public virtual DbSet<OrderDetailFood> OrderDetailFoods { get; set; }

    public virtual DbSet<OrderTable> OrderTables { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TypeOfFood> TypeOfFoods { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=123;database= ChickenPRN;TrustServerCertificate=True");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("MyCnn")); }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AccountId)
                .ValueGeneratedOnAdd()
                .HasColumnName("account_id");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.ComboId).HasName("PK__Combo__DD42580EF6C6F0A5");

            entity.ToTable("Combo");

            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.ComboName).HasMaxLength(50);
        });

        modelBuilder.Entity<ComboDetail>(entity =>
        {
            entity.HasKey(e => e.ComboDetailId).HasName("PK__ComboDet__7C0A2CD9FBC197D4");

            entity.ToTable("ComboDetail");

            entity.Property(e => e.ComboDetailId).HasColumnName("ComboDetailID");
            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");

            entity.HasOne(d => d.Combo).WithMany(p => p.ComboDetails)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__ComboDeta__Combo__5535A963");

            entity.HasOne(d => d.Food).WithMany(p => p.ComboDetails)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__ComboDeta__FoodI__5629CD9C");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CBF4FB9933");

            entity.ToTable("Food");

            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.FoodName).HasMaxLength(50);
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("image");

            entity.HasOne(d => d.FoodTypeNavigation).WithMany(p => p.Foods)
                .HasForeignKey(d => d.FoodType)
                .HasConstraintName("FK__Food__FoodType__571DF1D5");
        });

        modelBuilder.Entity<OrderDetailCombo>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2AC1C6A279");

            entity.ToTable("OrderDetailCombo");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");

            entity.HasOne(d => d.Combo).WithMany(p => p.OrderDetailCombos)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__OrderDeta__Combo__5812160E");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailCombos)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__59063A47");
        });

        modelBuilder.Entity<OrderDetailFood>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2AA8542A89");

            entity.ToTable("OrderDetailFood");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");

            entity.HasOne(d => d.Food).WithMany(p => p.OrderDetailFoods)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__OrderDeta__FoodI__59FA5E80");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailFoods)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__5AEE82B9");
        });

        modelBuilder.Entity<OrderTable>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderTab__0809337D194A219F");

            entity.ToTable("OrderTable");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("customerName");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IsPaid).HasColumnName("isPaid");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CCCB76C084");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<TypeOfFood>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__TypeOfFo__516F03957BC7B7C3");

            entity.ToTable("TypeOfFood");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
