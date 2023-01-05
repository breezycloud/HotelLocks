using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProRFL.UI.Data
{
    public class GuestCard
    {
        public int CardNo { get; set; }
        public string? LockNo { get; set; }
        public DateTime BDate { get; set; }
        public DateTime EDate { get; set; }
    }
}
