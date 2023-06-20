using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NhanVienBoPhan
    {
        public int Id { get; set; }
        public int MaNhanVien { get; set; }
        public int MaBoPhan { get; set; }
        public DateTime? NgayBatDauBp { get; set; }
        public DateTime? NgayKetThucBp { get; set; }

        public virtual BoPhan MaBoPhanNavigation { get; set; }
        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
