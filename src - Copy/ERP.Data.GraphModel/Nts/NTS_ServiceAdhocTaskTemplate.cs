using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.Model
{

    public partial class NTS_ServiceAdhocTaskTemplate : NTSBase
    {
        public ulong ServiceAdhocTaskTemplateId { get; set; }
        public ulong ServiceTemplateId { get; set; }
        [NonSerialized]
        private NTS_Template _ServiceTemplate;
        public NTS_Template ServiceTemplate
        {
            get
            {
                if (_ServiceTemplate == null && Session != null && Session.InTransaction)
                {
                    _ServiceTemplate = Session.AllObjects<NTS_Template>(false).FirstOrDefault(x => x.TemplateId == ServiceTemplateId && x.IsActive());
                }
                return _ServiceTemplate;
            }
        }
        public ulong TaskTemplateId { get; set; }
        [NonSerialized]
        private NTS_Template _TaskTemplate;
        public NTS_Template TaskTemplate
        {
            get
            {
                if (_TaskTemplate == null && Session != null && Session.InTransaction)
                {
                    _TaskTemplate = Session.AllObjects<NTS_Template>(false).FirstOrDefault(x => x.TemplateId == TaskTemplateId && x.IsActive());
                }
                return _TaskTemplate;
            }
        }
        

    }
}
