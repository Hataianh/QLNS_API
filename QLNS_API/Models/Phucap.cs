using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class PhuCap
    {
        public PhuCap()
        {
            NhanVienPhuCaps = new HashSet<NhanVienPhuCap>();
        }

        public int MaPhuCap { get; set; }
        public string TenPhuCap { get; set; }
        public double SoTien { get; set; }

        public virtual ICollection<NhanVienPhuCap> NhanVienPhuCaps { get; set; }
    }
}
