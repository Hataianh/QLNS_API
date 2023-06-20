using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class LoaiHopDong
    {
        public LoaiHopDong()
        {
            HopDongs = new HashSet<HopDong>();
        }

        public int MaLoaiHopDong { get; set; }
        public string TenLoaiHopDong { get; set; }
        public string MauHopDong { get; set; }

        public virtual ICollection<HopDong> HopDongs { get; set; }
    }
}
