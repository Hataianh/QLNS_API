using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class BangCong
    {
        public int MaBangCong { get; set; }
        public DateTime? GioVao { get; set; }
        public DateTime? GioRa { get; set; }
        public int MaNhanVien { get; set; }
        public string TrangThaiVao { get; set; }
        public string TrangThaiRa { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
