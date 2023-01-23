using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace CMS.Data.Model
{
    public class BreResult : DataModelBase
    {
        public string BusinessRuleNodeId { get; set; }
        public BreExecuteMethodTypeEnum? BreExecuteMethodType { get; set; }
        public string MethodName { get; set; }
        public string MethodNamespace { get; set; }
        public string MethodParamJson { get; set; }
        public string CustomMethodScript { get; set; }
        public bool? ReturnWithMessage { get; set; }
        public bool? ReturnIfMethodReturns { get; set; }
        public bool? MethodReturnValue { get; set; }
        public string[] Message { get; set; }
    }
}
