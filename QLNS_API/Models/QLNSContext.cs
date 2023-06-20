using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace QLNS_API.Models
{
    public partial class QLNSContext : DbContext
    {
        public QLNSContext()
        {
        }

        public QLNSContext(DbContextOptions<QLNSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BangCong> BangCongs { get; set; }
        public virtual DbSet<BangLuong> BangLuongs { get; set; }
        public virtual DbSet<BaoHiem> BaoHiems { get; set; }
        public virtual DbSet<BoPhan> BoPhans { get; set; }
        public virtual DbSet<ChucNang> ChucNangs { get; set; }
        public virtual DbSet<ChucVu> ChucVus { get; set; }
        public virtual DbSet<HopDong> HopDongs { get; set; }
        public virtual DbSet<KhenThuongKyLuat> KhenThuongKyLuats { get; set; }
        public virtual DbSet<LoaiHopDong> LoaiHopDongs { get; set; }
        public virtual DbSet<NghiPhep> NghiPheps { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<NhanVienBaoHiem> NhanVienBaoHiems { get; set; }
        public virtual DbSet<NhanVienBoPhan> NhanVienBoPhans { get; set; }
        public virtual DbSet<NhanVienChucNang> NhanVienChucNangs { get; set; }
        public virtual DbSet<NhanVienChucVu> NhanVienChucVus { get; set; }
        public virtual DbSet<NhanVienLuong> NhanVienLuongs { get; set; }
        public virtual DbSet<NhanVienPhongBan> NhanVienPhongBans { get; set; }
        public virtual DbSet<NhanVienPhuCap> NhanVienPhuCaps { get; set; }
        public virtual DbSet<PhongBan> PhongBans { get; set; }
        public virtual DbSet<PhuCap> PhuCaps { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<TrinhDo> TrinhDos { get; set; }
        public virtual DbSet<UngLuong> UngLuongs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=TANHH;Database=QLNS;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Vietnamese_CI_AS");

            modelBuilder.Entity<BangCong>(entity =>
            {
                entity.HasKey(e => e.MaBangCong)
                    .HasName("PK__bangcong__569DB9EF96868C8F");

                entity.ToTable("BangCong");

                entity.Property(e => e.GioRa).HasColumnType("datetime");

                entity.Property(e => e.GioVao).HasColumnType("datetime");

                entity.Property(e => e.TrangThaiRa).HasMaxLength(50);

                entity.Property(e => e.TrangThaiVao).HasMaxLength(50);

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.BangCongs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bangcong__manhan__33D4B598");
            });

            modelBuilder.Entity<BangLuong>(entity =>
            {
                entity.HasKey(e => e.MaBangLuong);

                entity.ToTable("BangLuong");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.BangLuongs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BangLuong_NhanVien");
            });

            modelBuilder.Entity<BaoHiem>(entity =>
            {
                entity.HasKey(e => e.MaBaoHiem)
                    .HasName("PK__baohiem__A96F90750187D85A");

                entity.ToTable("BaoHiem");

                entity.Property(e => e.LoaiBaoHiem)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<BoPhan>(entity =>
            {
                entity.HasKey(e => e.MaBoPhan)
                    .HasName("PK__bophan__1CBC1350942E49B2");

                entity.ToTable("BoPhan");

                entity.Property(e => e.TenBoPhan)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.MaPhongBanNavigation)
                    .WithMany(p => p.BoPhans)
                    .HasForeignKey(d => d.MaPhongBan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_bophan_phongban");
            });

            modelBuilder.Entity<ChucNang>(entity =>
            {
                entity.HasKey(e => e.MaChucNang)
                    .HasName("PK_chucnang");

                entity.ToTable("ChucNang");

                entity.Property(e => e.TenChucNang)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ChucVu>(entity =>
            {
                entity.HasKey(e => e.MaChucVu)
                    .HasName("PK__chucvu__38D260689B492371");

                entity.ToTable("ChucVu");

                entity.Property(e => e.TenChucVu)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<HopDong>(entity =>
            {
                entity.HasKey(e => e.MaHopDong)
                    .HasName("PK__hopdong__44476340D1E158F8");

                entity.ToTable("HopDong");

                entity.Property(e => e.NgayKy).HasColumnType("datetime");

                entity.Property(e => e.NoiDung).IsRequired();

                entity.HasOne(d => d.MaLoaiHopDongNavigation)
                    .WithMany(p => p.HopDongs)
                    .HasForeignKey(d => d.MaLoaiHopDong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_hopdong_loaihopdong");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.HopDongs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hopdong__manhanv__30F848ED");
            });

            modelBuilder.Entity<KhenThuongKyLuat>(entity =>
            {
                entity.ToTable("KhenThuongKyLuat");

                entity.Property(e => e.NgayQuyetDinh).HasColumnType("datetime");

                entity.Property(e => e.NoiDung)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.KhenThuongKyLuats)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__khenthuon__manha__398D8EEE");
            });

            modelBuilder.Entity<LoaiHopDong>(entity =>
            {
                entity.HasKey(e => e.MaLoaiHopDong)
                    .HasName("PK_loaihopdong");

                entity.ToTable("LoaiHopDong");

                entity.Property(e => e.TenLoaiHopDong)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NghiPhep>(entity =>
            {
                entity.ToTable("NghiPhep");

                entity.Property(e => e.NgayBatDauNghi).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThucNghi).HasColumnType("datetime");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.NghiPheps)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nghiphep__manhan__36B12243");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNhanVien)
                    .HasName("PK__nhanvien__75A28B520F542DCD");

                entity.ToTable("NhanVien");

                entity.Property(e => e.Cccd)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiaChi)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DienThoai)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GioiTinh)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.HoTen)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.HasOne(d => d.MaTrinhDoNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.MaTrinhDo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nhanvien_trinhdo");
            });

            modelBuilder.Entity<NhanVienBaoHiem>(entity =>
            {
                entity.ToTable("NhanVien_BaoHiem");

                entity.HasOne(d => d.MaBaoHiemNavigation)
                    .WithMany(p => p.NhanVienBaoHiems)
                    .HasForeignKey(d => d.MaBaoHiem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nhanvien_baohiem_baohiem");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.NhanVienBaoHiems)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nhanvien_baohiem_nhanvien");
            });

            modelBuilder.Entity<NhanVienBoPhan>(entity =>
            {
                entity.ToTable("NhanVien_BoPhan");

                entity.Property(e => e.NgayBatDauBp).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThucBp).HasColumnType("datetime");

                entity.HasOne(d => d.MaBoPhanNavigation)
                    .WithMany(p => p.NhanVienBoPhans)
                    .HasForeignKey(d => d.MaBoPhan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_BoPhan_BoPhan");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.NhanVienBoPhans)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_BoPhan_NhanVien");
            });

            modelBuilder.Entity<NhanVienChucNang>(entity =>
            {
                entity.ToTable("NhanVien_ChucNang");

                entity.HasOne(d => d.MaChucNangNavigation)
                    .WithMany(p => p.NhanVienChucNangs)
                    .HasForeignKey(d => d.MaChucNang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nhanvien_chucnang_chucnang");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.NhanVienChucNangs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nhanvien_chucnang_nhanvien");
            });

            modelBuilder.Entity<NhanVienChucVu>(entity =>
            {
                entity.ToTable("NhanVien_ChucVu");

                entity.Property(e => e.NgayBatDauCv).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThucCv).HasColumnType("datetime");

                entity.HasOne(d => d.MaChucVuNavigation)
                    .WithMany(p => p.NhanVienChucVus)
                    .HasForeignKey(d => d.MaChucVu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_ChucVu_ChucVu");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.NhanVienChucVus)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_ChucVu_NhanVien");
            });

            modelBuilder.Entity<NhanVienLuong>(entity =>
            {
                entity.HasKey(e => e.MaLuong);

                entity.ToTable("NhanVien_Luong");

                entity.Property(e => e.NgayBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.NhanVienLuongs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_Luong_NhanVien");
            });

            modelBuilder.Entity<NhanVienPhongBan>(entity =>
            {
                entity.ToTable("NhanVien_PhongBan");

                entity.Property(e => e.NgayBatDauPb).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThucPb).HasColumnType("datetime");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.NhanVienPhongBans)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_PhongBan_NhanVien");

                entity.HasOne(d => d.MaPhongBanNavigation)
                    .WithMany(p => p.NhanVienPhongBans)
                    .HasForeignKey(d => d.MaPhongBan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_PhongBan_PhongBan");
            });

            modelBuilder.Entity<NhanVienPhuCap>(entity =>
            {
                entity.ToTable("NhanVien_PhuCap");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.NhanVienPhuCaps)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nhanvien___manha__3E52440B");

                entity.HasOne(d => d.MaPhuCapNavigation)
                    .WithMany(p => p.NhanVienPhuCaps)
                    .HasForeignKey(d => d.MaPhuCap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nhanvien___maphu__3F466844");
            });

            modelBuilder.Entity<PhongBan>(entity =>
            {
                entity.HasKey(e => e.MaPhongBan)
                    .HasName("PK__phongban__5F252C13BF243DEF");

                entity.ToTable("PhongBan");

                entity.Property(e => e.TenPhongBan)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<PhuCap>(entity =>
            {
                entity.HasKey(e => e.MaPhuCap)
                    .HasName("PK__phucap__BB4AA22568A3E16E");

                entity.ToTable("PhuCap");

                entity.Property(e => e.TenPhuCap)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.MaTaiKhoan);

                entity.ToTable("TaiKhoan");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.TaiKhoans)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaiKhoan_NhanVien");
            });

            modelBuilder.Entity<TrinhDo>(entity =>
            {
                entity.HasKey(e => e.MaTrinhDo)
                    .HasName("PK__trinhdo__5C22CF1A788A6399");

                entity.ToTable("TrinhDo");

                entity.Property(e => e.TenTrinhDo)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UngLuong>(entity =>
            {
                entity.ToTable("UngLuong");

                entity.Property(e => e.Ngay).HasColumnType("datetime");

                entity.Property(e => e.NoiDung).HasMaxLength(500);

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.UngLuongs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ungluong__manhan__4222D4EF");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
