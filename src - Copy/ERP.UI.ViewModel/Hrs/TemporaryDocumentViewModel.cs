using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.Web;

namespace ERP.UI.ViewModel
{
    public class TemporaryDocumentViewModel : BaseViewModel
    {
        public virtual int Id { get; set; }

        public virtual string FileExtension { get; set; }

        public virtual string FileName { get; set; }

        public virtual string ContentType { get; set; }

        public virtual Nullable<long> ContentLength { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string FileContentBase64String { get; set; }
    }
}
