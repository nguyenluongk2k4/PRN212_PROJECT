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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<ComboDetail> ComboDetails { get; set; }

    public virtual DbSet<Expenditure> Expenditures { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<OrderDetailCombo> OrderDetailCombos { get; set; }

    public virtual DbSet<OrderDetailFood> OrderDetailFoods { get; set; }

    public virtual DbSet<OrderTable> OrderTables { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierOrder> SupplierOrders { get; set; }

    public virtual DbSet<SupplierOrderDetail> SupplierOrderDetails { get; set; }

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
            entity.HasKey(e => e.AccountId).HasName("PK_account_id");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
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

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Role_id");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Describe).HasColumnName("describe");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.ComboId).HasName("PK__Combo__DD42580E2D9133A5");

            entity.ToTable("Combo");

            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.ComboName).HasMaxLength(50);
        });

        modelBuilder.Entity<ComboDetail>(entity =>
        {
            entity.HasKey(e => e.ComboDetailId).HasName("PK__ComboDet__7C0A2CD9FFEE072A");

            entity.ToTable("ComboDetail");

            entity.Property(e => e.ComboDetailId).HasColumnName("ComboDetailID");
            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");

            entity.HasOne(d => d.Combo).WithMany(p => p.ComboDetails)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__ComboDeta__Combo__4F7CD00D");

            entity.HasOne(d => d.Food).WithMany(p => p.ComboDetails)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__ComboDeta__FoodI__5070F446");
        });

        modelBuilder.Entity<Expenditure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expendit__3214EC0716420A92");

            entity.ToTable("Expenditure");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Executor).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.SupplierOrder).WithMany(p => p.Expenditures)
                .HasForeignKey(d => d.SupplierOrderId)
                .HasConstraintName("FK__Expenditu__Suppl__5165187F");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3213E83F92B576EA");

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rate).HasColumnName("rate");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CBA673C77C");

            entity.ToTable("Food");

            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.FoodName).HasMaxLength(50);
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("image");

            entity.HasOne(d => d.FoodTypeNavigation).WithMany(p => p.Foods)
                .HasForeignKey(d => d.FoodType)
                .HasConstraintName("FK__Food__FoodType__52593CB8");
        });

        modelBuilder.Entity<OrderDetailCombo>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2AD29B5B9E");

            entity.ToTable("OrderDetailCombo");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");

            entity.HasOne(d => d.Combo).WithMany(p => p.OrderDetailCombos)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__OrderDeta__Combo__534D60F1");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailCombos)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__5441852A");
        });

        modelBuilder.Entity<OrderDetailFood>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2A978AADD1");

            entity.ToTable("OrderDetailFood");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");

            entity.HasOne(d => d.Food).WithMany(p => p.OrderDetailFoods)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__OrderDeta__FoodI__5535A963");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailFoods)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__5629CD9C");
        });

        modelBuilder.Entity<OrderTable>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderTab__0809337D10164900");

            entity.ToTable("OrderTable");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("customerName");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IsPaid).HasColumnName("isPaid");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB2FCC1CEE50");

            entity.HasIndex(e => e.PermissionName, "UQ__Permissi__0FFDA3572C5FFA6D").IsUnique();

            entity.Property(e => e.PermissionName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Phone");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Cid).HasColumnName("cid");
            entity.Property(e => e.Describe).HasColumnName("describe");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ReleaseDate).HasColumnName("releaseDate");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_Phone_Category");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CC854290E7");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolePermi__Permi__66603565"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolePermi__RoleI__656C112C"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId").HasName("PK__RolePerm__6400A1A8463C131E");
                        j.ToTable("RolePermissions");
                    });
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC079EFC1B46");

            entity.ToTable("Supplier");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SupplierOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC0769227A62");

            entity.ToTable("SupplierOrder");

            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierO__Suppl__571DF1D5");
        });

        modelBuilder.Entity<SupplierOrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC07D467ED6D");

            entity.ToTable("SupplierOrderDetail");

            entity.Property(e => e.ProductName).HasMaxLength(100);

            entity.HasOne(d => d.SupplierOrder).WithMany(p => p.SupplierOrderDetails)
                .HasForeignKey(d => d.SupplierOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierO__Suppl__5812160E");
        });

        modelBuilder.Entity<TypeOfFood>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__TypeOfFo__516F03953CBF2F21");

            entity.ToTable("TypeOfFood");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
