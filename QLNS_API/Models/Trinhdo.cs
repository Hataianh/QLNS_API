using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class TrinhDo
    {
        public TrinhDo()
        {
            NhanViens = new HashSet<NhanVien>();
        }

        public int MaTrinhDo { get; set; }
        public string TenTrinhDo { get; set; }

        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
