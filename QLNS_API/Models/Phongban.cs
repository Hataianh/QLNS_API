using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class PhongBan
    {
        public PhongBan()
        {
            BoPhans = new HashSet<BoPhan>();
            NhanVienPhongBans = new HashSet<NhanVienPhongBan>();
        }

        public int MaPhongBan { get; set; }
        public string TenPhongBan { get; set; }

        public virtual ICollection<BoPhan> BoPhans { get; set; }
        public virtual ICollection<NhanVienPhongBan> NhanVienPhongBans { get; set; }
    }
}
