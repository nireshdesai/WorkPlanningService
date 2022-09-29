using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkPlanningService.Models
{
    public class WorkPlanning
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string DateOfWorking { get; set; }
        [Range(1, 3)]
        public int ShiftId { get; set; }
    }
}