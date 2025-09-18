using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeManage.Models;

namespace TimeManage.ViewModels
{
    public class GoalTaskViewModel
    {
        public virtual Goal goal { get; set; }
        public virtual List<Tasks> Tasks { get; set; }

        public int selectedTaskId { get; set; }
    }
}