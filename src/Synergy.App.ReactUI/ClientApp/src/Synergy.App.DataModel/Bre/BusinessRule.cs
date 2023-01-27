using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Synergy.App.DataModel
{   
    public class BusinessRule : DataModelBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string DiagramData { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }
       
        [ForeignKey("ListOfValue")]
        public string ActionId { get; set; }
        //public LOV Action { get; set; }
        public BusinessLogicExecutionTypeEnum BusinessLogicExecutionType { get; set; }
    }
}
