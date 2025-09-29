using System.Collections.Generic;
using TimeManage.Models;

namespace TimeManage.Models
{
    public class TimeTable
    {
        public int Id { get; set; }
        public string Name { get; set; } = "My Schedule";
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    }
}