using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.PJM.Models
{
    public class LoadBookModel:NoteViewModel
    {
        public List<LoadBookModel> ChildNotes { get; set; }

        public int ChildNotesCount { get; set; }
    }
}
