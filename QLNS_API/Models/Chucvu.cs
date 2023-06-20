using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class ChucVu
    {
        public ChucVu()
        {
            NhanVienChucVus = new HashSet<NhanVienChucVu>();
        }

        public int MaChucVu { get; set; }
        public string TenChucVu { get; set; }

        public virtual ICollection<NhanVienChucVu> NhanVienChucVus { get; set; }
    }
}
