using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class BoPhan
    {
        public BoPhan()
        {
            NhanVienBoPhans = new HashSet<NhanVienBoPhan>();
        }

        public int MaBoPhan { get; set; }
        public string TenBoPhan { get; set; }
        public int MaPhongBan { get; set; }

        public virtual PhongBan MaPhongBanNavigation { get; set; }
        public virtual ICollection<NhanVienBoPhan> NhanVienBoPhans { get; set; }
    }
}
