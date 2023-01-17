using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
   public class DocumentListViewModel:NoteTemplateViewModel
    {

        public string DocumentId { get; set; }
        public string TemplateId { get; set; }
        public string Template { get; set; }
        public string TemplateOwner { get; set; }
        public string ProjectNo { get; set; }
        public string Revision { get; set; }
        public string Discipline { get; set; }
        public string StageStatus { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentNameLabel { get; set; }
        public string DocumentDescriptionLabel { get; set; }
        public string NtsNoLabelName { get; set; }
        public bool? HideDescription { get; set; }
        public string NoteNo { get; set; }
        public string FolderPath { get; set; }
        public string ParentId { get; set; }
        public string WorkSpaceId { get; set; }
        //public string[] UdfList { get; set; }
        public List<DynamicUdfViewModel> UdfList { get; set; }
        public string UdfNameList { get; set; }
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

        public string UdfCode1 { get; set; }
        public string UdfCode2 { get; set; }
        public string UdfCode3 { get; set; }
        public string UdfCode4 { get; set; }
        public string UdfCode5 { get; set; }
        public string UdfCode6 { get; set; }
        public string UdfCode7 { get; set; }
        public string UdfCode8 { get; set; }
        public string UdfCode9 { get; set; }
        public string UdfCode10 { get; set; }
        public string UdfCode11 { get; set; }
        public string UdfCode12 { get; set; }
        public string UdfCode13 { get; set; }
        public string UdfCode14 { get; set; }
        public string UdfCode15 { get; set; }
        public string UdfCode16 { get; set; }
        public string UdfCode17 { get; set; }
        public string UdfCode18 { get; set; }
        public string UdfCode19 { get; set; }
        public string UdfCode20 { get; set; }
        public string UdfCode21 { get; set; }
        public string UdfCode22 { get; set; }
        public string UdfCode23 { get; set; }
        public string UdfCode24 { get; set; }
        public string UdfCode25 { get; set; }
        public string UdfCode26 { get; set; }
        public string UdfCode27 { get; set; }
        public string UdfCode28 { get; set; }
        public string UdfCode29 { get; set; }
        public string UdfCode30 { get; set; }
        public string UdfCode31 { get; set; }
        public string UdfCode32 { get; set; }
        public string UdfCode33 { get; set; }
        public string UdfCode34 { get; set; }
        public string UdfCode35 { get; set; }
        public string UdfCode36 { get; set; }
        public string UdfCode37 { get; set; }
        public string UdfCode38 { get; set; }
        public string UdfCode39 { get; set; }
        public string UdfCode40 { get; set; }
        public string UdfCode41 { get; set; }
        public string UdfCode42 { get; set; }
        public string UdfCode43 { get; set; }
        public string UdfCode44 { get; set; }
        public string UdfCode45 { get; set; }
        public string UdfCode46 { get; set; }
        public string UdfCode47 { get; set; }
        public string UdfCode48 { get; set; }
        public string UdfCode49 { get; set; }
        public string UdfCode50 { get; set; }
        public bool? IsOverDue { get; set; }
        public string IssueCode { get; set; }
        public string TransmittalNo { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? FromDate { get; set; }
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ToDate { get; set; }
        public string RevisionCode { get; set; }
        public string DisciplineCode { get; set; }
        public double? PendingDays { get; set; }
        public string Vendor { get; set; }
        public int DocCount { get; set; }
        public int? TotalCount { get; set; }
        public IList<ColumnMetadataViewModel> SelectedTableRows { get; set; }
    }

    public class DynamicUdfViewModel
    {
        public string FieldName { get; set; }
        public string LabelDisplayName { get; set; }
        public string PartialViewName { get; set; }
        public string DataSourceControllerName { get; set; }
        public string DataSourceActionName { get; set; }
        public string DataSourceHtmlAttributesString { get; set; }
        public int SequenceNo { get; set; }
    }
}
