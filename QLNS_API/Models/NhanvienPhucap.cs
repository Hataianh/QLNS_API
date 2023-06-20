using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NhanVienPhuCap
    {
        public int Id { get; set; }
        public int MaNhanVien { get; set; }
        public int MaPhuCap { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
        public virtual PhuCap MaPhuCapNavigation { get; set; }
    }
}
