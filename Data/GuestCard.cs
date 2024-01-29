﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProRFL.UI.Data
{
    public class GuestCard
    {
        [Key]
        public Guid  MyProperty { get; set; }
        public int CardNo { get; set; }
        public string? LockNo { get; set; }
        public int dai { get; set; }
        public DateTime BDate { get; set; }
        public DateTime EDate { get; set; }
    }
}
