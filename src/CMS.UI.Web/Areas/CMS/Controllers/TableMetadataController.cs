using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using Synergy.App.DataModel;
using Synergy.App.Business;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.CMS.Views.Controllers
{
    [Area("Cms")]
    public class TableMetadataController : ApplicationController
    {
        private readonly IModuleBusiness _moduleBusiness;
        private readonly ITableMetadataBusiness _tableMetaDataBusiness;
        private readonly IColumnMetadataBusiness _columnMetadataBusiness;
        public TableMetadataController(IModuleBusiness moduleBusiness, ITableMetadataBusiness tableMetaDataBusiness
            , IColumnMetadataBusiness columnMetadataBusiness)
        {
            _moduleBusiness = moduleBusiness;
            _tableMetaDataBusiness = tableMetaDataBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult GetTableMetaDataList()
        {
            var model = new List<TableMetadataViewModel>();
            var t1 = new TableMetadataViewModel { TableType = TableTypeEnum.Table, Type= TableTypeEnum.Table.ToString(), Name = "Table1", ModuleName = "Module1", Schema = "Schema1" };
            model.Add(t1);
            var t2 = new TableMetadataViewModel { TableType = TableTypeEnum.Table, Type = TableTypeEnum.Table.ToString(), Name = "Table2", ModuleName = "Module2", Schema = "Schema2" };
            model.Add(t2);
            var t3 = new TableMetadataViewModel { TableType = TableTypeEnum.View, Type = TableTypeEnum.View.ToString(), Name = "View1", ModuleName = "Module1", Schema = "Schema2" };
            model.Add(t3);
            var t4 = new TableMetadataViewModel { TableType = TableTypeEnum.View, Type = TableTypeEnum.View.ToString(), Name = "View2", ModuleName = "Module2", Schema = "Schema2" };
            model.Add(t4);
            return Json(model);
        }
        public IActionResult CreateTableMetadata()
        {
            var model = new TableMetadataViewModel();
            model.DataAction = DataActionEnum.Create;
            model.TableType= TableTypeEnum.Table;
            return View("ManageTableMetadata", model);
        }

        public async Task<IActionResult> EditTableMetadata(string id)
        {
            var model = await _tableMetaDataBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("ManageTableMetadata", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageTableMetadata(TableMetadataViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _tableMetaDataBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _tableMetaDataBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("ManageTableMetadata", model);
        }

        public IActionResult CreateColumnMetadata(string tableMetadataId)
        {
            var model = new ColumnMetadataViewModel();
            model.DataAction = DataActionEnum.Create;
            model.TableMetadataId = tableMetadataId;
            return View("ManageColumnMetadata", model);
        }

        public async Task<IActionResult> EditColumnMetadata(string id)
        {
            var model = await _columnMetadataBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("ManageColumnMetadata", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageColumnMetadata(ColumnMetadataViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _columnMetadataBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _columnMetadataBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("ManageColumnMetadata", model);
        }

        public ActionResult ReadTableMetadata()
        {
            var model = _tableMetaDataBusiness.GetList();
            var data = model.Result;
            return Json(data);
        }
        public ActionResult ReadColumnMetadata(string tableMetadataId)
        {
            var model =_columnMetadataBusiness.GetList(x=>x.TableMetadataId== tableMetadataId);
            var data = model.Result;
            return Json(data);
        }
        public ActionResult ReadTableColumnMetadatas([DataSourceRequest] DataSourceRequest request)
        {
            var model = new List<ColumnMetadataViewModel>();
            var dsResult = model.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public async Task<IActionResult> TableColumn(string tableMetadataId)
        {
            var model = new ColumnMetadataViewModel();
            if (tableMetadataId.IsNotNullAndNotEmpty())
            {
                var tablemodel = await _tableMetaDataBusiness.GetSingleById(tableMetadataId);
                model.TableMetadataId = tableMetadataId;
                model.TableMetadataName = tablemodel.DisplayName;
            }
            
            return View(model);
        }

        public async Task<IActionResult> DeleteTableColumn(string Id)
        {
            await _columnMetadataBusiness.Delete(Id);
            return Json(true);
        }

        public IActionResult TableViewPage()
        {
            return View();
        }
        //public IActionResult ManageTable()
        //{
        //    var model = new TableMetadataViewModel();
        //    model.TableType = TableTypeEnum.Table;
        //    return View(model);
        //}
        
        public IActionResult CreateTable()
        {
            var model = new TableMetadataViewModel();
            model.TableType = TableTypeEnum.Table;
            return View("ManageTable", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageTable(TableMetadataViewModel model)
        {
            if (model.ColumnMetaDetails != null)
            {
                var columnData = JsonConvert.DeserializeObject<List<ColumnMetadataViewModel>>(model.ColumnMetaDetails);
                var cols = new List<ColumnMetadataViewModel>();
                foreach (var item in columnData)
                {
                    item.DataType = (DataColumnTypeEnum)Enum.Parse(typeof(DataColumnTypeEnum),item.DataTypestr);
                    cols.Add(item);
                }
                model.ColumnMetadatas = cols;
                var result=await _tableMetaDataBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                    //ViewBag.Success = true;

                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }
            //var err = new Dictionary<string, string>();
            //err.Add("Error","Test Error messafe");
            //ModelState.AddModelErrors(err);
            return View("ManageTable", model);
        }
        public IActionResult ManageView()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetModuleList()
        {
            var model = new List<IdNameViewModel>();
            //model.Add(new IdNameViewModel() { Id = "M0001", Name = "ModuleName1" });
            //model.Add(new IdNameViewModel() { Id = "M0002", Name = "ModuleName2" });
            //model.Add(new IdNameViewModel() { Id = "M0003", Name = "ModuleName3" });
            var data = await _moduleBusiness.GetList();
            model = data.Select(x=> new IdNameViewModel {Id=x.Id,Name=x.Name }).ToList();
            return Json(model);
        }
        [HttpGet]
        public ActionResult GetSchemaList()
        {
            var model = new List<IdNameViewModel>();
            model.Add(new IdNameViewModel() { Id = ApplicationConstant.Database.Schema.Cms, Name = ApplicationConstant.Database.Schema.Cms });
            model.Add(new IdNameViewModel() { Id = ApplicationConstant.Database.Schema._Public, Name = ApplicationConstant.Database.Schema._Public });
            return Json(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetForeignKeyTableList()
        {
            //var data = new List<TableMetadataViewModel>();
            //data.Add(new TableMetadataViewModel(){ Id = "T0001", Name = "Tabel1"});
            //data.Add(new TableMetadataViewModel(){Id = "T0002",Name = "Tabel2"});
            //data.Add(new TableMetadataViewModel() { Id = "T0003", Name = "Tabel3" });

            var data = await _tableMetaDataBusiness.GetList();
            var model = data.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return Json(model);
        }
        public async Task<ActionResult> GetForeignKeyColumnList(string tableId)
        {
            //var data = new List<ColumnMetadataViewModel>();
            //data.Add(new ColumnMetadataViewModel() { Id = "C00011", Name = "Column11", TableMetadataId= "T0001" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00012", Name = "Column12", TableMetadataId = "T0001" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00021", Name = "Column21", TableMetadataId = "T0002" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00022", Name = "Column22", TableMetadataId = "T0002" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00031", Name = "Column31", TableMetadataId = "T0003" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00032", Name = "Column32", TableMetadataId = "T0003" });
            
            var data = await _columnMetadataBusiness.GetList();
            var model = data.Where(x=>x.TableMetadataId==tableId).Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return Json(model);
        }

        public async Task<ActionResult> GetForeignKeyDisplayColumnList(string tableId)
        {
            //var data = new List<ColumnMetadataViewModel>();
            //data.Add(new ColumnMetadataViewModel() { Id = "DC00011", Name = "DisplayColumn11", TableMetadataId= "T0001" });
            //data.Add(new ColumnMetadataViewModel() { Id = "DC00012", Name = "DisplayColumn12", TableMetadataId = "T0001" });
            //data.Add(new ColumnMetadataViewModel() { Id = "DC00021", Name = "DisplayColumn21", TableMetadataId = "T0002" });
            //data.Add(new ColumnMetadataViewModel() { Id = "DC00022", Name = "DisplayColumn22", TableMetadataId = "T0002" });
            //data.Add(new ColumnMetadataViewModel() { Id = "DC00031", Name = "DisplayColumn31", TableMetadataId = "T0003" });
            //data.Add(new ColumnMetadataViewModel() { Id = "DC00032", Name = "DisplayColumn32", TableMetadataId = "T0003" });
            
            var data = await _columnMetadataBusiness.GetList();
            var model = data.Where(x => x.TableMetadataId == tableId).Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return Json(model);
        }
        public async Task<ActionResult> GetForeignKeyTableNameById(string tableId)
        {
            //var model = new List<IdNameViewModel>();
            //model.Add(new IdNameViewModel() { Id = "T0001", Name = "Tabel1" });
            //model.Add(new IdNameViewModel() { Id = "T0002", Name = "Tabel2" });
            //model.Add(new IdNameViewModel() { Id = "T0003", Name = "Tabel3" });

            var model = await _tableMetaDataBusiness.GetList();
            var tablename = model.Where(x => x.Id == tableId).Select(x => x.Name).FirstOrDefault();
            return Json(tablename);
        }
        public async Task<ActionResult> GetForeignKeyColumnNameById(string columnId)
        {
            //var data = new List<ColumnMetadataViewModel>();
            //data.Add(new ColumnMetadataViewModel() { Id = "C00011", Name = "Column11", TableMetadataId = "T0001" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00012", Name = "Column12", TableMetadataId = "T0001" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00021", Name = "Column21", TableMetadataId = "T0002" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00022", Name = "Column22", TableMetadataId = "T0002" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00031", Name = "Column31", TableMetadataId = "T0003" });
            //data.Add(new ColumnMetadataViewModel() { Id = "C00032", Name = "Column32", TableMetadataId = "T0003" });

            var data = await _columnMetadataBusiness.GetList();
            var columnname = data.Where(x => x.Id == columnId).Select(x => x.Name).FirstOrDefault();
            return Json(columnname);
        }
        public ActionResult GetForeignKeyDisplayColumnNameById(string discolumnId)
        {
            var data = new List<ColumnMetadataViewModel>();
            data.Add(new ColumnMetadataViewModel() { Id = "DC00011", Name = "DisplayColumn11", TableMetadataId = "T0001" });
            data.Add(new ColumnMetadataViewModel() { Id = "DC00012", Name = "DisplayColumn12", TableMetadataId = "T0001" });
            data.Add(new ColumnMetadataViewModel() { Id = "DC00021", Name = "DisplayColumn21", TableMetadataId = "T0002" });
            data.Add(new ColumnMetadataViewModel() { Id = "DC00022", Name = "DisplayColumn22", TableMetadataId = "T0002" });
            data.Add(new ColumnMetadataViewModel() { Id = "DC00031", Name = "DisplayColumn31", TableMetadataId = "T0003" });
            data.Add(new ColumnMetadataViewModel() { Id = "DC00032", Name = "DisplayColumn32", TableMetadataId = "T0003" });
            var discolumnname = data.Where(x => x.Id == discolumnId).Select(x => x.Name).FirstOrDefault();
            return Json(discolumnname);

        }
        
    }
}
