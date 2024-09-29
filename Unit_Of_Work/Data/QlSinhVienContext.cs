using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Unit_Of_Work.Data;

public partial class QlSinhVienContext : DbContext
{
    public QlSinhVienContext()
    {
    }

    public QlSinhVienContext(DbContextOptions<QlSinhVienContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LopHoc> LopHocs { get; set; }

    public virtual DbSet<SinhVien> SinhViens { get; set; }

/*    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=QL_SinhVien;User ID=sa;Password=123;Trust Server Certificate=True");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LopHoc>(entity =>
        {
            entity.HasKey(e => e.MaLh);

            entity.ToTable("LopHoc");

            entity.Property(e => e.MaLh)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TenLh)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(e => e.MaSv);

            entity.ToTable("SinhVien");

            entity.Property(e => e.MaSv).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaLh)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.NgaySinh).HasMaxLength(50);

            entity.HasOne(d => d.MaLhNavigation).WithMany(p => p.SinhViens)
                .HasForeignKey(d => d.MaLh)
                .HasConstraintName("FK_SinhVien_LopHoc");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
