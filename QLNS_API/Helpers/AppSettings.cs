using QLNS_API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QLNS_API.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public User Users { get; set; }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
