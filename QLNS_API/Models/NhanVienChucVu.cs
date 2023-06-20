using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NhanVienChucVu
    {
        public int Id { get; set; }
        public int MaNhanVien { get; set; }
        public int MaChucVu { get; set; }
        public DateTime? NgayBatDauCv { get; set; }
        public DateTime? NgayKetThucCv { get; set; }

        public virtual ChucVu MaChucVuNavigation { get; set; }
        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
