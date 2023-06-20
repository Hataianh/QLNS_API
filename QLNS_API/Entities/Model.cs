using QLNS_API.Models;
using System;
using System.Collections.Generic;

namespace QLNS_API.Entities
{
    public class BangCongModel
    {
        public BangCong bangcong { get; set; }
        public int MaBangCong { get; set; }
        public DateTime? GioVao { get; set; }
        public DateTime? GioRa { get; set; }
        public int MaNhanVien { get; set; }
        public string TrangThaiVao { get; set; }
        public string TrangThaiRa { get; set; }
        public NhanVien nhanvien { get; set; }
    }
    public class BaoHiemModel
    {
        public BaoHiem baohiem { get; set; }
        public NhanVienBaoHiem nhanvienBaohiem { get; set; }
    }
    public class BoPhanModel
    {
        public BoPhan bophan { get; set; }
        public NhanVien nhanvien { get; set; }
    }
    public class ChucNangModel
    {
        public ChucNang chucnang { get; set; }
        public NhanVien nhanvien { get; set; }
    }
    public class ChucVuModel
    {
        public ChucVu chucvu { get; set; }
        public NhanVien nhanvien { get; set; }
    }
    public class HopDongModel
    {
        public HopDong hopdong { get; set; }
    }
    public class KhenThuongKyLuatModel
    {
        public KhenThuongKyLuat khenthuongkyluat { get; set; }
        public NhanVien nhanvien { get; set; }
        public int[] MaNhanVien { get; set; }
    }
    public class LoaiHopDongModel
    {
        public LoaiHopDong loaihopdong { get; set; }
        public HopDong hopdong { get; set; }
    }
    public class NghiPhepModel
    {
        public NghiPhep nghiphep { get; set; }
    }
    public class BangLuongModel
    {
        public BangLuong bangluong { get; set; }
        public NhanVien nhanvien { get; set; }
    }
    public class NhanVienModel
    {
        public int MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Cccd { get; set; }
        public string DiaChi { get; set; }
        public string HinhAnh { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? XacMinhEmail { get; set; }
        public int MaPhongBan { get; set; }
        public DateTime? NgayKetThucPb { get; set; }
        public int MaBoPhan { get; set; }
        public DateTime? NgayKetThucBp { get; set; }
        public int MaChucVu { get; set; }
        public DateTime? NgayKetThucCv { get; set; }
        public int MaTrinhDo { get; set; }
        public int TrangThai { get; set; }
        public string TenPhongBan { get; set; }
        public string TenBoPhan { get; set; }
        public string TenChucVu { get; set; }
        public string TenTrinhDo { get; set; }
        public double MucLuong { get; set; }
        public double? KTKLSoTien { get; set; }
        public double PCSoTien { get; set; }
        public string LoaiBaoHiem { get; set; }
        public double? ULSoTien { get; set; }
        public int? MaPhuCap { get; set; }
        public int? MaBaoHiem { get; set; }
        public int? SoKTKL { get; set; }
        public int? UngLuongID { get; set; }
        public string Token { get; set; }
        public string RolePB { get; set; }
        public string RoleBP { get; set; }
        public string RoleCV { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public NhanVien nhanvien { get; set; }
        public NhanVien NhanVien { get; internal set; }
        public BangCong bangcong { get; set; }
        public HopDong hopdong { get; set; }
        public KhenThuongKyLuat khenthuongkyluat { get; set; }
        public NghiPhep nghiphep { get; set; }
        public UngLuong ungluong { get; set; }
        public BaoHiem baohiem { get; set; }
        public BaoHiemModel BaoHiemModel { get; set; }
        public PhuCap phucap { get; set; }
        public PhuCapModel PhuCapModel { get; set; }
        public ChucNang chucnang { get; set; }
        public PhongBan phongban { get; set; }
        public BoPhan bophan { get; set; }
        public ChucVu chucvu { get; set; }
        public TaiKhoan taikhoan { get; set; }
        public List<NhanVienBaoHiem> nhanvienBaohiem { get; set; }
        public List<NhanVienPhuCap> nhanvienPhucap { get; set; }
        public List<NhanVienChucNang> nhanvienChucnang { get; set; }
        public List<NhanVienLuong> nhanvienluong { get; set; }
        public List<BaoHiem> baoHiem { get; set; }
        public List<PhuCap> phuCap { get; set; }
        public List<KhenThuongKyLuat> khenthuongKyluat { get; set; }
        public List<UngLuong> ungLuong { get; set; }
        public NhanVienPhongBan nhanvienPhongban { get; set; }
        public NhanVienBoPhan nhanvienBophan { get; set; }
        public NhanVienChucVu nhanvienChucvu { get; set; }
        public NhanVienLuong nhanvienLuong { get; set; }
    }
    public class NhanVienBaoHiemModel
    {
        public NhanVien nhanvien { get; set; }
        public NhanVienBaoHiem nhanvienBaohiem { get; set; }
    }
    public class NhanVienPhuCapModel
    {
        public NhanVien nhanvien { get; set; }
        public NhanVienPhuCap nhanvienPhucap { get; set; }
        public PhuCap phucap { get; set; }
    }
    public class NhanVienChucNangModel
    {
        public NhanVien nhanvien { get; set; }
        public NhanVienChucNang nhanvienChucnang { get; set; }
    }
    public class NhanVienPhongBanModel
    {
        public NhanVien nhanvien { get; set; }
        public NhanVienPhongBan nhanvienPhongban { get; set; }
    }
    public class NhanVienLuongModel
    {
        public NhanVien nhanvien { get; set; }
        public NhanVienLuong nhanvienLuong { get; set; }
    }
    public class NhanVienBoPhanModel
    {
        public NhanVien nhanvien { get; set; }
        public NhanVienBoPhan nhanvienBophan { get; set; }
    }
    public class NhanVienChucVuModel
    {
        public NhanVien nhanvien { get; set; }
        public NhanVienChucVu nhanvienChucvu { get; set; }
    }
    public class PhongBanModel
    {
        public PhongBan phongban { get; set; }
        public BoPhan bophan { get; set; }
        public NhanVien nhanvien { get; set; }
    }
    public class PhuCapModel
    {
        public PhuCap phucap { get; set; }
        public NhanVienPhuCap nhanvienPhucap { get; set; }
    }
    public class TrinhDoModel
    {
        public TrinhDo trinhdo { get; set; }
        public NhanVien nhanvien { get; set; }
    }
    public class UngLuongModel
    {
        public UngLuong ungluong { get; set; }
    }
    public class TaiKhoanModel
    {
        public TaiKhoan taikhoan { get; set; }
        public NhanVien nhanvien { get; set; }
    }
}
