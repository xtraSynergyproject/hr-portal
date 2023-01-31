using AutoMapper;
using SpreadsheetLight;
using Synergy.App.Common;
using Synergy.App.Common.Utilities;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class StepTaskEscalationDataBusiness : BusinessBase<StepTaskEscalationDataViewModel, StepTaskEscalationData>, IStepTaskEscalationDataBusiness
    {
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public StepTaskEscalationDataBusiness(IRepositoryBase<StepTaskEscalationDataViewModel, StepTaskEscalationData> repo, IMapper autoMapper
          , ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<StepTaskEscalationDataViewModel>> Create(StepTaskEscalationDataViewModel model, bool autoCommit = true)
        {           
            var result = await base.Create(model,autoCommit);
            return CommandResult<StepTaskEscalationDataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<StepTaskEscalationDataViewModel>> Edit(StepTaskEscalationDataViewModel model, bool autoCommit = true)
        {
          
            var result = await base.Edit(model,autoCommit);
            return CommandResult<StepTaskEscalationDataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<StepTaskEscalationDataViewModel>> GetPortalTaskListWithEscalationData(string portal, string escalatUser)
        {
            var list = await _cmsQueryBusiness.GetPortalTaskListWithEscalationData(portal, escalatUser);
            return list;
        }

        public async Task<List<StepTaskEscalationDataViewModel>> GetMyTasksEscalatedDataList(string portal, string assigneeUser)
        {
            var list = await _cmsQueryBusiness.MyTasksEscalatedDataList(portal, assigneeUser);
            return list;
        }

        public async Task<List<StepTaskEscalationDataViewModel>> AllEscalatedTasks(string portalids)
        {
            var list = await _cmsQueryBusiness.AllEscalatedTasks(portalids);
            return list;
        }

        public async Task<MemoryStream> GetExcelForTemplateData(string portalids)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>();


            reportList.Add("Template Data");
            var data = await AllEscalatedTasks(portalids);
            using (var sl = new SLDocument())
            {



                sl.AddWorksheet("AllEscalatedTasks");
                SLStyle style = sl.CreateStyle();
                style.Font.FontSize = 12;
                style.Font.Bold = true;
                sl.SetRowStyle(1, style);
                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);
                int row = 2;
                sl.SetCellValue("A1", "Name");
                sl.SetCellValue("B1", "Category");
                sl.SetCellValue("C1", "Service No");
                sl.SetCellValue("D1", "Service Name");
                sl.SetCellValue("E1", "Requested By");
                sl.SetCellValue("F1", "Service Status");
                sl.SetCellValue("G1", "Service Created Date");
                sl.SetCellValue("H1", "TaskNo");
                sl.SetCellValue("I1", "Task Subject");
                sl.SetCellValue("J1", "Task Assignee");
                sl.SetCellValue("K1", "Task Status");
                sl.SetCellValue("L1", "Task Start Date");
                sl.SetCellValue("M1", "Task Due Date");
                sl.SetCellValue("N1", "Escalated Date");
                sl.SetCellValue("O1", "Escalated To User Name");
                sl.SetCellValue("P1", "Escalation Comment");
                foreach (var item in data)
                {
                    sl.SetCellValue(string.Concat('A',row), item.TemplateName);
                    sl.SetCellValue(string.Concat('B',row), item.CategoryName);
                    sl.SetCellValue(string.Concat('C',row), item.ServiceNo);
                    sl.SetCellValue(string.Concat('D',row), item.ServiceName);
                    sl.SetCellValue(string.Concat('E',row), item.RequestedBy);
                    sl.SetCellValue(string.Concat('F',row), item.ServiceStatus);
                    sl.SetCellValue(string.Concat('G', row), Convert.ToDateTime(item.ServiceCreatedDate).ToDD_YYYY_MM_DD());
                    sl.SetCellValue(string.Concat('H',row), item.TaskNo);
                    sl.SetCellValue(string.Concat('I',row), item.TaskSubject);
                    sl.SetCellValue(string.Concat('J',row), item.TaskAssignee);
                    sl.SetCellValue(string.Concat('K',row), item.TaskStatus);
                    sl.SetCellValue(string.Concat('L',row), Convert.ToDateTime(item.StartDate).ToDD_YYYY_MM_DD());
                    sl.SetCellValue(string.Concat('M',row), Convert.ToDateTime(item.DueDate).ToDD_YYYY_MM_DD());
                    sl.SetCellValue(string.Concat('N',row), Convert.ToDateTime(item.EscalatedDate).ToDD_YYYY_MM_DD());
                    sl.SetCellValue(string.Concat('O',row), item.EscalatedToUserName);
                    sl.SetCellValue(string.Concat('P', row), item.EscalationComment);
                    row++;
                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }
            ms.Position = 0;
            return ms;
        }

    }
}
