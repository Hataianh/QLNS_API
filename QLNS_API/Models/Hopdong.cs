using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class HopDong
    {
        public int MaHopDong { get; set; }
        public int MaLoaiHopDong { get; set; }
        public DateTime? NgayKy { get; set; }
        public string NoiDung { get; set; }
        public int LanKy { get; set; }
        public int MaNhanVien { get; set; }

        public virtual LoaiHopDong MaLoaiHopDongNavigation { get; set; }
        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
