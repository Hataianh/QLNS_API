using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NghiPhep
    {
        public int Id { get; set; }
        public DateTime NgayBatDauNghi { get; set; }
        public DateTime NgayKetThucNghi { get; set; }
        public int MaNhanVien { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
