using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class TemplateFieldManageViewModel
    {
        public long TemplateFieldId { get; set; }
        public NtsUserTypeEnum? EditableBy { get; set; }
        public NtsActionEnum? EditableContext { get; set; }
        public NtsUserTypeEnum? ViewableBy { get; set; }
        public NtsActionEnum? ViewableContext { get; set; }
        public long? PermissionCode { get; set; }
    }
}
