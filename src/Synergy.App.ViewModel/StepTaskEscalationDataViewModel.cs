using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class StepTaskEscalationDataViewModel : StepTaskEscalationData
    {
      public string ServiceNo { get; set; }
      public string ServiceName { get; set; }
      public string RequestedBy { get; set; }
      public string TemplateName { get; set; }
      public string TemplateCode { get; set; }
      public string CategoryName { get; set; }
      public string TaskTemplateCode { get; set; }
      public string TaskNo { get; set; }
      public string TaskStatus { get; set; }
      public string ServiceStatus { get; set; }
      public string TaskId { get; set; }
      public string TaskSubject { get; set; }
      public string TaskAssignee { get; set; }
      public string EscalatedToUserName { get; set; }
      public DateTime StartDate { get; set; }
      public DateTime DueDate { get; set; }
      public DateTime ServiceCreatedDate { get; set; }
    }
}
