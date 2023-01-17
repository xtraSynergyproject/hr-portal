using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class ApplicationError : DataModelBase
    {
        public int HttpCode { get; set; }
        public string Exception { get; set; }
        public string ErrorMessage { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public bool IsAjaxCall { get; set; }
    }

}
