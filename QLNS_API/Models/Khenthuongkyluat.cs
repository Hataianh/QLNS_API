using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class KhenThuongKyLuat
    {
        public int Id { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayQuyetDinh { get; set; }
        public double? SoTien { get; set; }
        public int Loai { get; set; }
        public int MaNhanVien { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
