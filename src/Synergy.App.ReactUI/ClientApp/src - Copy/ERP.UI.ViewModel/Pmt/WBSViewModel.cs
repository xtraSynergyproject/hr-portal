using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class WBSViewModel : ViewModelBase
    {

        public string ItemNo { get; set; }
        public string WbsNo { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ResourceName { get; set; }
        public int? WorkHours { get; set; }
        public long? Percentage { get; set; }
        public NtsPriorityEnum? Priority { get; set; }
        public long? ProjectId { get; set; }
        public long? ParentId { get; set; }
        public WBSItemTypeEnum? ItemSource { get; set; }
        public List<long> Users { get; set; }
        public NtsTypeEnum? NTSType { get; set; }
        public long? AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public decimal? SequenceNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PlanStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? PlanEndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ActualStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ActualEndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ForcastStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ForcastEndDate { get; set; }
        public long? NTSItemId { get; set; }
        public long? TemplateMasterId { get; set; }
        public long? WbsTemplateId { get; set; }
        public NtsActionEnum? TemplateAction { get; set; }//set runtime
        public TimeSpan? SLA { get; set; }
        public string DefaultView { get; set; }

        public string Udf1 { get; set; }
        public string Udf2 { get; set; }
        public string Udf3 { get; set; }
        public string Udf4 { get; set; }
        public string Udf5 { get; set; }
        public string Udf6 { get; set; }
        public string Udf7 { get; set; }
        public string Udf8 { get; set; }
        public string Udf9 { get; set; }
        public string Udf10 { get; set; }
        public string Udf11 { get; set; }
        public string Udf12 { get; set; }
        public string Udf13 { get; set; }
        public string Udf14 { get; set; }
        public string Udf15 { get; set; }
        public string Udf16 { get; set; }
        public string Udf17 { get; set; }
        public string Udf18 { get; set; }
        public string Udf19 { get; set; }
        public string Udf20 { get; set; }
        public string Udf21 { get; set; }
        public string Udf22 { get; set; }
        public string Udf23 { get; set; }
        public string Udf24 { get; set; }
        public string Udf25 { get; set; }
        public string Udf26 { get; set; }
        public string Udf27 { get; set; }
        public string Udf28 { get; set; }
        public string Udf29 { get; set; }
        public string Udf30 { get; set; }
        public string Udf31 { get; set; }
        public string Udf32 { get; set; }
        public string Udf33 { get; set; }
        public string Udf34 { get; set; }
        public string Udf35 { get; set; }
        public string Udf36 { get; set; }
        public string Udf37 { get; set; }
        public string Udf38 { get; set; }
        public string Udf39 { get; set; }
        public string Udf40 { get; set; }
        public string Udf41 { get; set; }
        public string Udf42 { get; set; }
        public string Udf43 { get; set; }
        public string Udf44 { get; set; }
        public string Udf45 { get; set; }
        public string Udf46 { get; set; }
        public string Udf47 { get; set; }
        public string Udf48 { get; set; }
        public string Udf49 { get; set; }
        public string Udf50 { get; set; }
        public string Udf51 { get; set; }
        public string Udf52 { get; set; }
        public string Udf53 { get; set; }
        public string Udf54 { get; set; }
        public string Udf55 { get; set; }
        public string Udf56 { get; set; }
        public string Udf57 { get; set; }
        public string Udf58 { get; set; }
        public string Udf59 { get; set; }
        public string Udf60 { get; set; }
        public string Udf61 { get; set; }
        public string Udf62 { get; set; }
        public string Udf63 { get; set; }
        public string Udf64 { get; set; }
        public string Udf65 { get; set; }
        public string Udf66 { get; set; }
        public string Udf67 { get; set; }
        public string Udf68 { get; set; }
        public string Udf69 { get; set; }
        public string Udf70 { get; set; }
        public string Udf71 { get; set; }
        public string Udf72 { get; set; }
        public string Udf73 { get; set; }
        public string Udf74 { get; set; }
        public string Udf75 { get; set; }
        public string Udf76 { get; set; }
        public string Udf77 { get; set; }
        public string Udf78 { get; set; }
        public string Udf79 { get; set; }
        public string Udf80 { get; set; }
        public string Udf81 { get; set; }
        public string Udf82 { get; set; }
        public string Udf83 { get; set; }
        public string Udf84 { get; set; }
        public string Udf85 { get; set; }
        public string Udf86 { get; set; }
        public string Udf87 { get; set; }
        public string Udf88 { get; set; }
        public string Udf89 { get; set; }
        public string Udf90 { get; set; }
        public string Udf91 { get; set; }
        public string Udf92 { get; set; }
        public string Udf93 { get; set; }
        public string Udf94 { get; set; }
        public string Udf95 { get; set; }
        public string Udf96 { get; set; }
        public string Udf97 { get; set; }
        public string Udf98 { get; set; }
        public string Udf99 { get; set; }
        public string Udf100 { get; set; }
        public long? UserId { get; set; }
        public long? TaskGroupId { get; set; }
        public string TaskGroup { get; set; }

    }
    public class WBSItemFieldViewModel
    {
        public string ItemField { get; set; }
        public string ItemFieldLable { get; set; }
        public string ItemFieldPartialView { get; set; }
        public string ItemFieldGrouplable { get; set; }
        public int ItemFieldSequence { get; set; }
    }
}
