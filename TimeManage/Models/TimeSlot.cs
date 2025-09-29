using System;

namespace TimeManage.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public int TimeTableId { get; set; }
        public virtual TimeTable TimeTable { get; set; }
        public string Color { get; set; }
        public string Txtcolor { get; set; }
    }
}