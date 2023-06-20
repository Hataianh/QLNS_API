using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class ChucNang
    {
        public ChucNang()
        {
            NhanVienChucNangs = new HashSet<NhanVienChucNang>();
        }

        public int MaChucNang { get; set; }
        public string TenChucNang { get; set; }

        public virtual ICollection<NhanVienChucNang> NhanVienChucNangs { get; set; }
    }
}
