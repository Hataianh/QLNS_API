using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class BaoHiem
    {
        public BaoHiem()
        {
            NhanVienBaoHiems = new HashSet<NhanVienBaoHiem>();
        }

        public int MaBaoHiem { get; set; }
        public string LoaiBaoHiem { get; set; }
        public double MucDong { get; set; }

        public virtual ICollection<NhanVienBaoHiem> NhanVienBaoHiems { get; set; }
    }
}
