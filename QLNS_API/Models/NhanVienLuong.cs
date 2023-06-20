using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NhanVienLuong
    {
        public int MaLuong { get; set; }
        public double MucLuong { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
