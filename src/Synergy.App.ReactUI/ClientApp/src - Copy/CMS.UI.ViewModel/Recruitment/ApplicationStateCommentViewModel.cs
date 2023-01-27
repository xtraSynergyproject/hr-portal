using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class ApplicationStateCommentViewModel : ApplicationStateComment
    {
       public string CommentedBy { get; set; }
       public string TemplateCode { get; set; }
       public string ApplicationStateCode { get; set; }
    }
}
