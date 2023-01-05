using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProRFL.UI.Data
{
    public class Room
    {
        [Required(ErrorMessage="Name is required")]
        public string? Name { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomName { get; set; }
        [Required(ErrorMessage = "Lock No is required")]
        public string? LockNo { get; set; }
        [Required(ErrorMessage = "Card No is required")]
        public int? CardNo { get; set; }
        public string? DepartureDate { get; set; }
        public DateTime? Date => string.IsNullOrEmpty(DepartureDate) ? new DateTime(DateTime.Now.Ticks) : DateTime.Parse(DepartureDate!);
    }
}
