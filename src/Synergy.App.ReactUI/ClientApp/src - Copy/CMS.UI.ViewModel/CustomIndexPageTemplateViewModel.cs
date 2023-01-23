using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class CustomIndexPageTemplateViewModel : CustomIndexPageTemplate
    {
        public string RowData { get; set; }
        public string TableMetadataId { get; set; }
        public string ModuleCodes { get; set; }
        public PageViewModel Page { get; set; }
        public IList<CustomIndexPageColumnViewModel> SelectedTableRows { get; set; }

        public bool ShowAllOwnersService { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public string TemplateCodes { get; set; }
        public string CategoryCodes { get; set; }
        public bool IsDisableCreate { get; set; }
        public IList<customIndexPageTemplateViewModel> PaymentDueList { get; set; }
        public IList<customIndexPageTemplateViewModel> ServiceList { get; set; }
       
        public string TemplateSelectionPopupDefaultTitle
        {
            get
            {
                if (TemplateSelectionPopupTitle.IsNullOrEmpty())
                {
                    return "Select Service Type";
                }
                else
                {
                    return TemplateSelectionPopupTitle;
                }
            }
        }
        public string CreateButtonDefaultText
        {
            get
            {
                if (CreateButtonText.IsNullOrEmpty())
                {
                    switch (NtsType)
                    {
                        case NtsTypeEnum.Note:
                            return "Create New Note";
                        case NtsTypeEnum.Task:
                            return "Create New Task";
                        case NtsTypeEnum.Service:
                            return "Create New Service";
                        default:
                            return "Create New Item";
                    }
                }
                else
                {
                    return CreateButtonText;
                }
            }
        }
    }
    public class customIndexPageTemplateViewModel
    {
        public string ServiceName { get; set; }
        public long DueAmount { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        public string BillDate { get; set; }
        public int? Count { get; set; }
        public int? InProgressCount { get; set; }
        public int? CompletedCount { get; set; }
        public int? RejectedCount { get; set; }
        public string TemplateCode { get; set; }
    }
}
