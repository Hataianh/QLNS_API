using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class NhanVienChucNang
    {
        public int Id { get; set; }
        public int MaNhanVien { get; set; }
        public int MaChucNang { get; set; }

        public virtual ChucNang MaChucNangNavigation { get; set; }
        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
