using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            BangCongs = new HashSet<BangCong>();
            BangLuongs = new HashSet<BangLuong>();
            HopDongs = new HashSet<HopDong>();
            KhenThuongKyLuats = new HashSet<KhenThuongKyLuat>();
            NghiPheps = new HashSet<NghiPhep>();
            NhanVienBaoHiems = new HashSet<NhanVienBaoHiem>();
            NhanVienBoPhans = new HashSet<NhanVienBoPhan>();
            NhanVienChucNangs = new HashSet<NhanVienChucNang>();
            NhanVienChucVus = new HashSet<NhanVienChucVu>();
            NhanVienLuongs = new HashSet<NhanVienLuong>();
            NhanVienPhongBans = new HashSet<NhanVienPhongBan>();
            NhanVienPhuCaps = new HashSet<NhanVienPhuCap>();
            TaiKhoans = new HashSet<TaiKhoan>();
            UngLuongs = new HashSet<UngLuong>();
        }

        public int MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Cccd { get; set; }
        public string DiaChi { get; set; }
        public string HinhAnh { get; set; }
        public string DienThoai { get; set; }
        public int MaTrinhDo { get; set; }
        public int TrangThai { get; set; }

        public virtual TrinhDo MaTrinhDoNavigation { get; set; }
        public virtual ICollection<BangCong> BangCongs { get; set; }
        public virtual ICollection<BangLuong> BangLuongs { get; set; }
        public virtual ICollection<HopDong> HopDongs { get; set; }
        public virtual ICollection<KhenThuongKyLuat> KhenThuongKyLuats { get; set; }
        public virtual ICollection<NghiPhep> NghiPheps { get; set; }
        public virtual ICollection<NhanVienBaoHiem> NhanVienBaoHiems { get; set; }
        public virtual ICollection<NhanVienBoPhan> NhanVienBoPhans { get; set; }
        public virtual ICollection<NhanVienChucNang> NhanVienChucNangs { get; set; }
        public virtual ICollection<NhanVienChucVu> NhanVienChucVus { get; set; }
        public virtual ICollection<NhanVienLuong> NhanVienLuongs { get; set; }
        public virtual ICollection<NhanVienPhongBan> NhanVienPhongBans { get; set; }
        public virtual ICollection<NhanVienPhuCap> NhanVienPhuCaps { get; set; }
        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; }
        public virtual ICollection<UngLuong> UngLuongs { get; set; }
    }
}
