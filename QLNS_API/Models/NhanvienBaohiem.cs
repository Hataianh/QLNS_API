using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NhanVienBaoHiem
    {
        public int Id { get; set; }
        public int MaNhanVien { get; set; }
        public int MaBaoHiem { get; set; }

        public virtual BaoHiem MaBaoHiemNavigation { get; set; }
        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
