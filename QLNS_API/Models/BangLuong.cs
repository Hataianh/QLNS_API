using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class BangLuong
    {
        public int MaBangLuong { get; set; }
        public int MaNhanVien { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public double Luong { get; set; }
        public int TongNgayCong { get; set; }
        public double TongPhuCap { get; set; }
        public double TongKtkl { get; set; }
        public double TongBaoHiem { get; set; }
        public double TongUngLuong { get; set; }
        public double LuongThucLinh { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
