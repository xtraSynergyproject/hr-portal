using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class ApplicationDocument : DataModelBase
    {
        public string DocumentName { get; set; }
        public string Code { get; set; }
        public string DocumentId { get; set; }
    }

}
