using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class RptDataHeaderViewModel : BaseViewModel
    {
        public virtual int Id { get; set; }

        public virtual int RptGenerateId { get; set; }

        public virtual int RptSectionId { get; set; }

        public virtual string StandardComment { get; set; }

        public virtual string UserComment { get; set; }

        public virtual System.DateTime EffectiveFromDate { get; set; }

        public virtual Nullable<System.DateTime> EffectiveToDate { get; set; }

    }
}
