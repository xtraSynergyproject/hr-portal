
using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ProjectTaskViewModel
    {
        public string Id { get; set; }
        public string ServiceNo { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public string ProjectStatus { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string UserName { get; set; }
        public string ProjectId { get; set; }
        public string ServiceId { get; set; }
        public string Type { get; set; }
        public bool lazy { get; set; }
        public string title { get; set; }
        public string key { get; set; }
        public string ProjectStatusCode { get; set; }
    }
}
