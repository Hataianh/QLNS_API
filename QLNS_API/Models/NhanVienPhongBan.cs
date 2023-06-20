using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NhanVienPhongBan
    {
        public int Id { get; set; }
        public int MaNhanVien { get; set; }
        public int MaPhongBan { get; set; }
        public DateTime? NgayBatDauPb { get; set; }
        public DateTime? NgayKetThucPb { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
        public virtual PhongBan MaPhongBanNavigation { get; set; }
    }
}
