using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeManage.Models
{
    public class Goal
    {
        public int GoalId { get; set; }

        [Required]
        public string GoalName { get; set; }

        [Required]
        public string GoalDescription { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual List<Tasks> Tasks { get; set; }
    }
}