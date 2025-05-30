﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManage.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsComplete { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
