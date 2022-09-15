using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkPlanningService.Models
{
    public class WorkPlanning
    {
        public int EmployeeId { get; set; }
        public string DateOfWorking { get; set; }
        public int ShiftId { get; set; }
    }
}