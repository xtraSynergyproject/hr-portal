using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class KanbanBoardViewModel : NoteTemplateViewModel
    {
        public string boards { get; set; } 
        public KanbanTemplateEnum kanbanTemplate { get; set; } 
    }
}
