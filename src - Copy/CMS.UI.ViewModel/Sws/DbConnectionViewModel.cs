using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class DbConnectionViewModel : NoteTemplateViewModel
    {
        public string databaseType { get; set; }
        [Required]
        public string hostName { get; set; }
        [Required]
        public string port { get; set; }
        [Required]
        public string maintenanceDatabase { get; set; }
        [Required]
        public string username { get; set; }       
        public string role { get; set; }       
        public string service { get; set; }
        [Required]
        public string password { get; set; }       

    }
}
