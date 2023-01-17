using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class ComponentViewModel : Component
    {
        public string CssClass
        {
            get
            {
                switch (ComponentType)
                {
                    case Common.ProcessDesignComponentTypeEnum.Start:
                        return "comp-start";
                    case Common.ProcessDesignComponentTypeEnum.Stop:
                        return "comp-stop";
                    case Common.ProcessDesignComponentTypeEnum.Email:
                        return "comp-email";
                    case Common.ProcessDesignComponentTypeEnum.StepTask:
                        return "comp-step-task";
                    case Common.ProcessDesignComponentTypeEnum.ProcessDesign:
                        return "comp-process-design";
                    case Common.ProcessDesignComponentTypeEnum.DecisionScript:
                        return "comp-decision-script";
                    case Common.ProcessDesignComponentTypeEnum.ExecutionScript:
                        return "comp-execution-script";
                    case Common.ProcessDesignComponentTypeEnum.True:
                        return "comp-true";
                    case Common.ProcessDesignComponentTypeEnum.False:
                        return "comp-false";
                    default:
                        return "comp-none";
                }
            }
        }
        public ProcessDesignComponentTypeEnum ComponentsType
        {
            get
            {
                switch (ComponentTypeNo)
                {
                    case"2":
                        return ProcessDesignComponentTypeEnum.Stop;
                    case "3":
                        return ProcessDesignComponentTypeEnum.Email;
                    case "4":
                        return ProcessDesignComponentTypeEnum.StepTask;
                    case "5":
                        return ProcessDesignComponentTypeEnum.ProcessDesign;
                    case "6":
                        return ProcessDesignComponentTypeEnum.DecisionScript;
                    case "7":
                        return ProcessDesignComponentTypeEnum.ExecutionScript;
                    case "8":
                        return ProcessDesignComponentTypeEnum.True;
                    case "9":
                        return ProcessDesignComponentTypeEnum.False;
                    default:
                        return ProcessDesignComponentTypeEnum.Start;
                }
            }
        }
        public string ComponentTypeNo {
            get;set;
        }
        public string Properties { get; set; }
        public string[] Parents { get; set; }
        public long Level { get; set; }
        public string ParentId { get; set; }
        public string NewId { get; set; }
        public string TemplateId { get; set; }
        public string DecisionScriptComponentId { get; set; }
        public string Script { get; set; }
        public BusinessRuleLogicTypeEnum? BusinessRuleLogicType { get; set; }

    }
}
