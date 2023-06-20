using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class UngLuong
    {
        public int Id { get; set; }
        public DateTime Ngay { get; set; }
        public string NoiDung { get; set; }
        public double SoTien { get; set; }
        public int? TrangThai { get; set; }
        public int MaNhanVien { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
