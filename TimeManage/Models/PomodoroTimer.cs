using System;
using System.ComponentModel.DataAnnotations;

namespace TimeManage.Models
{
    public class PomodoroTimer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public int Minutes { get; set; } = 25;
        public int Seconds { get; set; } = 0;

        public int PreviousMinutes { get; set; } = 25;
        public int PreviousSeconds { get; set; } = 0;

        public DateTime? TimerEnd { get; set; }
        public bool IsRunning { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}