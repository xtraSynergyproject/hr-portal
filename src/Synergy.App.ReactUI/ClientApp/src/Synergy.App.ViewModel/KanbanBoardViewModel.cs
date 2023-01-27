using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class KanbanBoardViewModel : NoteTemplateViewModel
    {
        public string boards { get; set; } 
        public KanbanTemplateEnum kanbanTemplate { get; set; } 
    }
}
