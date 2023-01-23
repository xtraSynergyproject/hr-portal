﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class EmployeeServiceViewModel
    {

        [Display(Name = "Service No.")]
        public string ServiceNo { get; set; }
        public string ServiceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ServiceOwner { get; set; }
        public string LeaveTypeCode { get; set; }
        public string TemplateAction { get; set; }
    }
}