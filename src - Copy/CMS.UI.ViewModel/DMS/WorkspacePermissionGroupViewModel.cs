using CMS.Common;
using CMS.Data.Model;
using Microsoft.AspNetCore.Http;
using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class WorkspacePermissionGroupViewModel : NoteTemplateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Users { get; set; }
        public long? RemoveUserId { get; set; }
        public int UsersCount { get; set; }
        public ICollection<string> UsersList { get; set; }


    }
  
    
}
