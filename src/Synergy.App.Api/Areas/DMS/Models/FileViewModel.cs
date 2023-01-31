using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.DMS.Models
{
    public class FileNoteViewModel
    {

        public string Name { get; set; }
        public int? Level { get; set; }
        public long? FolderId { get; set; }

        public string Id { get; set; }

        public string ParentId { get; set; }

        public string UniqueId { get; set; }
        // public string UniqueIdReference { get; set; }
        public string ParentUniqueId { get; set; }

        public long? ToWorkspaceId { get; set; }
        public string selectedFiles { get; set; }
        public string ParentName { get; set; }
        public long? SharedByUserId { get; set; }
        public string SharedByUserName { get; set; }
        public long? PreviousParentId { get; set; }
        public bool? IsAllowSubFolders { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsFolderFileArchived { get; set; }
        //public List<DocumentViewModel> DocumentsList { get; set; }
        public long? FileId { get; set; }

        private string _Color;
       
        public bool? HasChildren { get; set; }
    }
}
