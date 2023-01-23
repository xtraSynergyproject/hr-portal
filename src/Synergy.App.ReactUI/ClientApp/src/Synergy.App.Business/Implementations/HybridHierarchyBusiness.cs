using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.IO;
using SpreadsheetLight;
using Synergy.App.Common.Utilities;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Synergy.App.Business
{
    public class HybridHierarchyBusiness : BusinessBase<HybridHierarchyViewModel, HybridHierarchy>, IHybridHierarchyBusiness
    {
        IRepositoryQueryBase<HybridHierarchyViewModel> _repoQuery;
        IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        IRepositoryQueryBase<UserHierarchyPermissionViewModel> _userHierarchyPermission;
        IUserContext _userContext;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public HybridHierarchyBusiness(IRepositoryBase<HybridHierarchyViewModel, HybridHierarchy> repo, IMapper autoMapper
            , IRepositoryQueryBase<HybridHierarchyViewModel> repoQuery, IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<UserHierarchyPermissionViewModel> userHierarchyPermission, IUserContext userContext, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
            _queryRepo1 = queryRepo1;
            _userHierarchyPermission = userHierarchyPermission;
            _userContext = userContext;
            _cmsQueryBusiness = cmsQueryBusiness;

        }
        public async override Task<CommandResult<HybridHierarchyViewModel>> Create(HybridHierarchyViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model,autoCommit);
            if (result.IsSuccess)
            {
                if (model.HierarchyPath == null || !model.HierarchyPath.Any())
                {
                    var hrchyPath = await GetHierarchyPath(result.Item.Id);
                    result.Item.HierarchyPath = hrchyPath.ToArray();
                    await UpdateHierarchyPath(result.Item);
                }
            }

            return CommandResult<HybridHierarchyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async override Task<CommandResult<HybridHierarchyViewModel>> Edit(HybridHierarchyViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);

            return CommandResult<HybridHierarchyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<IList<TaskViewModel>> GetBHServiceData(bool showAllService)
        {

            var result = await _cmsQueryBusiness.GetBHServiceData(showAllService);
            return result;
        }
        public async Task<IList<TaskViewModel>> GetBHTaskData()
        {
            var result = await _cmsQueryBusiness.GetBHTaskData();
            return result;
        }
        public async Task<List<BusinessHierarchyPermissionViewModel>> GetBusinessHierarchyPermissionData(string groupCode)
        {
            var list = await _cmsQueryBusiness.GetBusinessHierarchyPermissionData(groupCode);
            if (list.IsNotNull())
            {
                foreach (var item in list)
                {
                    if (item.UserId.IsNotNullAndNotEmpty())
                    {
                        var users = item.UserId.Trim('[', ']');
                        users = users.Replace("\"", "\'");
                       var data = await _cmsQueryBusiness.GetBusinessHierarchyPermissionData2(users);
                        if (data.IsNotNullAndNotEmpty())
                        {
                            item.UserName = data;
                        }
                    }
                }
            }
            return list;
        }

        public async Task<bool> DeleteBusinessHierarchyPermission(BusinessHierarchyPermissionViewModel model)
        {
            var id = model.Id;
            var noteId = model.NtsNoteId;
             await _cmsQueryBusiness.DeleteBusinessHierarchyPermissionData(id, noteId);
            return true;
        }
        public async Task<List<string>> GetHierarchyPath(string hierarchyItemId)
        {
             var list = await _cmsQueryBusiness.GetHierarchyPathData(hierarchyItemId);
            var items = list.Select(x => x.ParentId).ToList();
            items.Reverse();
            return items;

        }
        public async Task<List<HybridHierarchyViewModel>> GetHierarchyParentDetails(string hierarchyItemId)
        {
            var list = await _cmsQueryBusiness.GetHierarchyParentDetailsData(hierarchyItemId);
            return list;

        }
        public async Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyChildList(string parentId, int level, int levelupto, bool enableAOR, string bulkRequestId, bool includeParent)
        {
            var userId = _repo.UserContext.UserId;
            var isAdmin = _repo.UserContext.IsSystemAdmin;
            var list = await _cmsQueryBusiness.GetBusinessHierarchyChildListData(parentId,level,levelupto,enableAOR,bulkRequestId,includeParent, userId, isAdmin);
          return list;
        }

        public async Task RemoveFromBusinessHierarchy(string id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<HybridHierarchyViewModel>> GetHourReportProjectData()
        {
            string query = "";
            //            string query = @$"select s.""servicesubject"" as projectname,s.""id"" as projectid,t.""tasksubject"" as taskname,t.""taskno"" as taskno,lov.""name"" as taskstatusname,
            //tu.""id"" as assigneeid,s.""startdate"" as taskstartdate,t.""tasksla"" as sla,t.""id""  as taskid,
            //s.""duedate"" as taskduedate,tu.""name"" as assigneename
            //                            from public.""ntstasktimeentry""
            //        };
            var queryData = await _repoQuery.ExecuteQueryList<HybridHierarchyViewModel>(query, null);
            return queryData;
        }
        public async Task<MemoryStream> DownloadHybridHierarchy()
        {

            var ms = new MemoryStream();
            using (var sl = new SLDocument())
            {
                sl.AddWorksheet("Business Hierarchy");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);


                sl.SetColumnWidth("A", 20);
                sl.SetColumnWidth("B", 20);
                sl.SetColumnWidth("C", 20);
                sl.SetColumnWidth("D", 20);
                sl.SetColumnWidth("E", 20);
                sl.SetColumnWidth("F", 20);
                sl.SetColumnWidth("G", 20);
                sl.SetColumnWidth("H", 20);
                sl.SetColumnWidth("I", 20);
                sl.SetColumnWidth("J", 20);
                sl.SetColumnWidth("K", 20);
                sl.SetColumnWidth("L", 20);
                sl.SetColumnWidth("M", 20);
                sl.SetColumnWidth("N", 20);
                sl.SetColumnWidth("O", 20);
                sl.SetColumnWidth("P", 20);
                sl.SetColumnWidth("R", 20);

                sl.MergeWorksheetCells("A1", "B1");
                sl.SetCellValue("A1", "");
                sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                sl.MergeWorksheetCells("I1", "J1");
                sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                sl.MergeWorksheetCells("A2", "H3");
                sl.SetCellValue("A2", "Business Hierarchy : " + DateTime.Today.ToDefaultDateFormat());
                sl.SetCellStyle("A2", "H3", ExcelHelper.GetReportHeadingStyle(sl));

                int row = 5;

                sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                sl.SetCellValue(string.Concat("A", row), "OrgLevel1");
                sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                sl.SetCellValue(string.Concat("B", row), "OrgLevel2");
                sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                sl.SetCellValue(string.Concat("C", row), "OrgLevel3");
                sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                sl.SetCellValue(string.Concat("D", row), "OrgLevel4");
                sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                sl.SetCellValue(string.Concat("E", row), "Brand");
                sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                sl.SetCellValue(string.Concat("F", row), "Market");
                sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                sl.SetCellValue(string.Concat("G", row), "Province");
                sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                sl.SetCellValue(string.Concat("H", row), "Department");
                sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                sl.SetCellValue(string.Concat("I", row), "CareerLevel");
                sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                sl.SetCellValue(string.Concat("J", row), "Job");
                sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                sl.SetCellValue(string.Concat("K", row), "Position");
                sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                sl.SetCellValue(string.Concat("L", row), "Employee");
                sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                var model = await PrepareHybridHierarchyExcel();

                //model.Add(new BusinessHierarchyExcelViewModel { OrgLevel1 = "Level1", OrgLevel2 = "Level1", OrgLevel3 = "Level1", OrgLevel4 = "Level1", Brand = "Level1", Market = "Level1", Province = "Level1", CareerLevel = "Level1", Department = "Level1", Job = "Level1", Employee = "Level1" });
                //model.Add(new BusinessHierarchyExcelViewModel { OrgLevel1 = "Level2", OrgLevel2 = "Level2", OrgLevel3 = "Level2", OrgLevel4 = "Level2", Brand = "Level2", Market = "Level2", Province = "Level2", CareerLevel = "Level2", Department = "Level1", Job = "Level1", Employee = "Level2" });
                //model.Add(new BusinessHierarchyExcelViewModel { OrgLevel1 = "Level3", OrgLevel2 = "Level3", OrgLevel3 = "Level3", OrgLevel4 = "Level3", Brand = "Level3", Market = "Level3", Province = "Level3", CareerLevel = "Level3", Department = "Level3", Job = "Level3", Employee = "Level3" });
                //model.Add(new BusinessHierarchyExcelViewModel { OrgLevel1 = "Level4", OrgLevel2 = "Level4", OrgLevel3 = "Level4", OrgLevel4 = "Level4", Brand = "Level4", Market = "Level4", Province = "Level4", CareerLevel = "Level4", Department = "Level4", Job = "Level4", Employee = "Level4" });

                //        var model = new List<BusinessHierarchyExcelViewModel>
                //{
                //    new  {OrgLevel1 = "Level1", OrgLevel2 = "Level1",OrgLevel3 ="Level1",OrgLevel4="Level1",Brand="Level1",Market="Level1",Province="Level1",CareerLevel="Level1",Department="Level1",Job="Level1",Employee="Level1"},
                //    new  {OrgLevel1 = "Level2", OrgLevel2 = "Level2",OrgLevel3 ="Level2",OrgLevel4="Level2",Brand="Level2",Market="Level2",Province="Level2",CareerLevel="Level2",Department="Level1",Job="Level1",Employee="Level2"},
                //    new  {OrgLevel1 = "Level3", OrgLevel2 = "Level3",OrgLevel3 ="Level3",OrgLevel4="Level3",Brand="Level3",Market="Level3",Province="Level3",CareerLevel="Level3",Department="Level3",Job="Level3",Employee="Level3"},
                //    new  {OrgLevel1 = "Level4", OrgLevel2 = "Level4",OrgLevel3 ="Level4",OrgLevel4="Level4",Brand="Level4",Market="Level4",Province="Level4",CareerLevel="Level4",Department="Level4",Job="Level4",Employee="Level4"}
                //};

                row++;
                //var projectList = await GetHourReportProjectData();
                foreach (var modelData in model)
                {
                    if (modelData.IsNotNull())
                    {
                        sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                        sl.SetCellValue(string.Concat("A", row), modelData.OrgLevel1.IsNotNull() ? modelData.OrgLevel1 : "");
                        sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                        sl.SetCellValue(string.Concat("B", row), modelData.OrgLevel2.IsNotNull() ? modelData.OrgLevel2 : "");
                        sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                        sl.SetCellValue(string.Concat("C", row), modelData.OrgLevel3.IsNotNull() ? modelData.OrgLevel3 : "");
                        sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                        sl.SetCellValue(string.Concat("D", row), modelData.OrgLevel4.IsNotNull() ? modelData.OrgLevel4 : "");
                        sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                        sl.SetCellValue(string.Concat("E", row), modelData.Brand.IsNotNull() ? modelData.Brand : "");
                        sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                        sl.SetCellValue(string.Concat("F", row), modelData.Market.IsNotNull() ? modelData.Market : "");
                        sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                        sl.SetCellValue(string.Concat("G", row), modelData.Province.IsNotNull() ? modelData.Province : "");
                        sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                        sl.SetCellValue(string.Concat("H", row), modelData.Department.IsNotNull() ? modelData.Department : "");
                        sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                        sl.SetCellValue(string.Concat("I", row), modelData.CareerLevel.IsNotNull() ? modelData.CareerLevel : "");
                        sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                        sl.SetCellValue(string.Concat("J", row), modelData.Job.IsNotNull() ? modelData.Job : "");
                        sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                        sl.SetCellValue(string.Concat("K", row), modelData.Position.IsNotNull() ? modelData.Position : "");
                        sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                        sl.SetCellValue(string.Concat("L", row), modelData.Employee.IsNotNull() ? modelData.Employee : "");
                        sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        row++;
                        //        //    string query = @$"SELECT ""Id"" as Id FROM public.""HybridHierarchy"" where ""IsDeleted""=false";
                        //        //var name = await _repoQuery.ExecuteQueryList<HybridHierarchyViewModel>(query, null);

                        //        //var list = name;
                        //        //return list;
                    }
                }
                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }


        public async Task<MemoryStream> DownloadAORdata(List<BusinessHierarchyAORViewModel> aorList)
        {
            List<string> usersList = aorList.Select(x => x.UserName).Distinct().ToList();
            List<string> boxList = aorList.Select(x => x.ReferenceName).Distinct().ToList();

            var ms = new MemoryStream();
            using (var sl = new SLDocument())
            {
                //box-row; users-columns ----- 2nd worksheet


                // first worksheet
                sl.SelectWorksheet("Sheet1");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);
                var cellStyle = sl.CreateStyle();
                cellStyle.Fill.SetPatternBackgroundColor(SLThemeColorIndexValues.Dark1Color);
                cellStyle.Fill.SetPatternType(PatternValues.Solid);

                var row = 1;
                var column = 1;
                foreach (var user in usersList)
                {
                    column = 1;
                    if (column == 1)
                    {

                        sl.SetCellValue(row + 1, column, user);
                        sl.AutoFitColumn(column);
                    }
                    foreach (var box in boxList)
                    {
                        if (row == 1)
                        {
                            sl.SetCellValue(row, column + 1, box);
                        }

                        var exist = aorList.Exists(x => x.UserName == user && x.ReferenceName == box);
                        if (exist)
                        {
                            sl.SetCellValue(row + 1, column + 1, "YES");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetApprovedStyle(sl));

                        }
                        else
                        {
                            sl.SetCellValue(row + 1, column + 1, "NO");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetRejectedStyle(sl));
                        }
                        column++;
                    }

                    row++;

                }


                sl.AddWorksheet("Sheet2");
                // second worksheet
                sl.SelectWorksheet("Sheet2");

                row = 1;
                column = 1;
                foreach (var box in boxList)
                {
                    column = 1;
                    if (column == 1)
                    {
                        sl.SetCellValue(row + 1, column, box);
                    }
                    foreach (var user in usersList)
                    {
                        if (row == 1)
                        {
                            sl.SetCellValue(row, column + 1, user);
                        }
                        var exist = aorList.Exists(x => x.UserName == user && x.ReferenceName == box);
                        if (exist)
                        {
                            sl.SetCellValue(row + 1, column + 1, "YES");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetApprovedStyle(sl));

                        }
                        else
                        {
                            sl.SetCellValue(row + 1, column + 1, "NO");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetRejectedStyle(sl));
                        }
                        column++;
                    }

                    row++;

                }
                sl.SelectWorksheet("Sheet1");
                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }
        public async Task<MemoryStream> DownloadBusinessPartnerMappingExcel(List<BusinessHierarchyAORViewModel> mappingList)
        {
            List<string> usersList = mappingList.Select(x => x.BusinessPartnerName).Distinct().ToList();
            List<string> boxList = mappingList.Select(x => x.DepartmentName).Distinct().ToList();

            var ms = new MemoryStream();
            using (var sl = new SLDocument())
            {
                //box-row; users-columns ----- 2nd worksheet


                // first worksheet
                sl.SelectWorksheet("Sheet1");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);
                var cellStyle = sl.CreateStyle();
                cellStyle.Fill.SetPatternBackgroundColor(SLThemeColorIndexValues.Dark1Color);
                cellStyle.Fill.SetPatternType(PatternValues.Solid);

                var row = 1;
                var column = 1;
                foreach (var user in usersList)
                {
                    column = 1;
                    if (column == 1)
                    {

                        sl.SetCellValue(row + 1, column, user);
                        sl.AutoFitColumn(column);
                    }
                    foreach (var box in boxList)
                    {
                        if (row == 1)
                        {
                            sl.SetCellValue(row, column + 1, box);
                        }

                        var exist = mappingList.Exists(x => x.BusinessPartnerName == user && x.DepartmentName == box);
                        if (exist)
                        {
                            sl.SetCellValue(row + 1, column + 1, "YES");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetApprovedStyle(sl));

                        }
                        else
                        {
                            sl.SetCellValue(row + 1, column + 1, "NO");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetRejectedStyle(sl));
                        }
                        column++;
                    }

                    row++;

                }


                sl.AddWorksheet("Sheet2");
                // second worksheet
                sl.SelectWorksheet("Sheet2");

                row = 1;
                column = 1;
                foreach (var box in boxList)
                {
                    column = 1;
                    if (column == 1)
                    {
                        sl.SetCellValue(row + 1, column, box);
                    }
                    foreach (var user in usersList)
                    {
                        if (row == 1)
                        {
                            sl.SetCellValue(row, column + 1, user);
                        }
                        var exist = mappingList.Exists(x => x.BusinessPartnerName == user && x.DepartmentName == box);
                        if (exist)
                        {
                            sl.SetCellValue(row + 1, column + 1, "YES");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetApprovedStyle(sl));

                        }
                        else
                        {
                            sl.SetCellValue(row + 1, column + 1, "NO");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetRejectedStyle(sl));
                        }
                        column++;
                    }

                    row++;

                }
                sl.SelectWorksheet("Sheet1");
                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }
        private async Task<List<BusinessHierarchyExcelViewModel>> PrepareHybridHierarchyExcel()
        {
            var result = new List<BusinessHierarchyExcelViewModel>();
            var data = await GetBusinessHierarchyChildList("-1", 0, 1000, false, null, true);
            var parentIds = data.Select(x => x.ParentId).Distinct().ToList();
            var leafNodes = data.Where(x => !parentIds.Contains(x.Id)).OrderBy(x => x.SequenceOrder).ToList();
            foreach (var item in leafNodes)
            {
                AddItemsToHybridHierarchyExcel(item, data, result);
            }
            return result;
        }

        private void AddItemsToHybridHierarchyExcel(HybridHierarchyViewModel item, List<HybridHierarchyViewModel> data, List<BusinessHierarchyExcelViewModel> result)
        {
            var model = new BusinessHierarchyExcelViewModel();
            while (item != null)
            {
                switch (item.ReferenceType)
                {
                    case "LEVEL1":
                        model.OrgLevel1 = item.Name;
                        break;
                    case "LEVEL2":
                        model.OrgLevel2 = item.Name;
                        break;
                    case "LEVEL3":
                        model.OrgLevel3 = item.Name;
                        break;
                    case "LEVEL4":
                        model.OrgLevel4 = item.Name;
                        break;
                    case "BRAND":
                        model.Brand = item.Name;
                        break;
                    case "MARKET":
                        model.Market = item.Name;
                        break;
                    case "PROVINCE":
                        model.Province = item.Name;
                        break;
                    case "DEPARTMENT":
                        model.Department = item.Name;
                        break;
                    case "CAREER_LEVEL":
                        model.CareerLevel = item.Name;
                        break;
                    case "JOB":
                        model.Job = item.Name;
                        break;
                    case "POSITION":
                        model.Position = item.Name;
                        break;
                    case "EMPLOYEE":
                        model.Employee = item.Name;
                        break;
                    default:
                        break;
                }
                item = data.FirstOrDefault(x => x.Id == item.ParentId);
            }
            if (model.OrgLevel2.IsNullOrEmpty())
            {
                model.OrgLevel2 = model.OrgLevel1;
            }
            if (model.OrgLevel3.IsNullOrEmpty())
            {
                model.OrgLevel3 = model.OrgLevel2;
            }
            if (model.OrgLevel4.IsNullOrEmpty())
            {
                model.OrgLevel4 = model.OrgLevel3;
            }
            result.Add(model);
        }

        public async Task UpdateHierarchyPath(HybridHierarchyViewModel hybridmodel)
        {
            var values = string.Join("\", \"", hybridmodel.HierarchyPath);
            values = string.Concat("{\"", values, "\"}");
            await _cmsQueryBusiness.UpdateHierarchyPathData(hybridmodel, values);
        }
        

        public async Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyList(string referenceType = null, string searckKey = null, bool bindPath = false)
        {
            var list = await _cmsQueryBusiness.GetBusinessHierarchyListData(referenceType, searckKey,bindPath);

            return list;
        }
        public async Task<List<UserHierarchyPermissionViewModel>> GetBusinessHierarchyRootPermission(string PermissionId = null,string UserId = null)
        {
            var list = await _cmsQueryBusiness.GetBusinessHierarchyRootPermissionData(PermissionId, UserId);
            return list;
        }

        public async Task MoveItemToNewParent(string cureNodeId, string newParentId)
        {
            var hierarchy = await GetSingleById(cureNodeId);
            hierarchy.ParentId = newParentId;
            await Edit(hierarchy);
            var path = await GetHierarchyPath(cureNodeId);
            hierarchy.HierarchyPath = path.ToArray();

            await UpdateHierarchyPath(hierarchy);

            var childlist = await GetBusinessHierarchyChildList(cureNodeId, 0, 1000, false, "", false);

            foreach (var item in childlist)
            {
                path = await GetHierarchyPath(item.Id);
                item.HierarchyPath = path.ToArray();
                await UpdateHierarchyPath(item);
            }


        }

        public async Task<List<BusinessHierarchyAORViewModel>> GetAllAORBusinessHierarchyList()
        {
           var list = await _cmsQueryBusiness.GetAllAORBusinessHierarchyListData();
            return list;
            
        }

    }
}
