using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using ActiveQueryBuilder.Web.Server.Services;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class AQBController : ApplicationController
    {


        private string instanceId = "BootstrapTheming";

        private readonly IQueryBuilderService _aqbs;
        private readonly IConfiguration _config;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IColumnMetadataBusiness _columnMetadataBusiness;

        // Use IQueryBuilderService to get access to the server-side instances of Active Query Builder objects. 
        // See the registration of this service in the Startup.cs.
        public AQBController(IQueryBuilderService aqbs, IConfiguration config
            , ITableMetadataBusiness tableMetadataBusiness,
            IColumnMetadataBusiness columnMetadataBusiness)
        {
            _aqbs = aqbs;
            _config = config;
            _tableMetadataBusiness = tableMetadataBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
        }
        public IActionResult Index(string tableMetadataId, LayoutModeEnum lo = LayoutModeEnum.Main, string cbm = null)
        {
            var qb = _aqbs.GetOrCreate(instanceId, q => q.SyntaxProvider = new GenericSyntaxProvider());

            return View(qb);
        }

        public async Task LoadMetadata()
        {
            var queryBuilder1 = _aqbs.Get(instanceId);

            ResetQueryBuilderMetadata(queryBuilder1);
            queryBuilder1.SyntaxProvider = new PostgreSQLSyntaxProvider();
            // prevent QueryBuilder to request metadata
            queryBuilder1.MetadataLoadingOptions.OfflineMode = true;

            queryBuilder1.MetadataProvider = null;

            MetadataContainer metadataContainer = queryBuilder1.MetadataContainer;
            metadataContainer.BeginUpdate();

            try
            {
                metadataContainer.Clear();
                var tableMetadataList = await _tableMetadataBusiness.GetList();
                var columnMetadataList = await _columnMetadataBusiness.GetList();
                var schemas = tableMetadataList.Select(x => x.Schema).Distinct().ToList();
                foreach (var item in schemas)
                {
                    var schemaDbo = metadataContainer.AddSchema(item);
                    var tables = tableMetadataList.Where(x => x.TableType == TableTypeEnum.Table && x.Schema == item).ToList();
                    foreach (var table in tables)
                    {
                        var t = schemaDbo.AddTable(table.Name);
                        var columns = columnMetadataList.Where(x => x.TableMetadataId == table.Id).ToList();
                        foreach (var column in columns)
                        {
                            t.AddField(column.Alias);
                            if (column.IsForeignKey && !column.IsVirtualForeignKey)
                            {
                                var foreignKey = t.AddForeignKey(column.ForeignKeyConstraintName);

                                using (var referencedName = new MetadataQualifiedName())
                                {
                                    referencedName.Add(column.ForeignKeyTableName);
                                    referencedName.Add(column.ForeignKeyTableSchemaName);

                                    foreignKey.ReferencedObjectName = referencedName;
                                }

                                foreignKey.Fields.Add(column.Name);
                                foreignKey.ReferencedFields.Add(column.ForeignKeyColumnName);
                            }
                        }
                    }
                    var views = tableMetadataList.Where(x => x.TableType == TableTypeEnum.View && x.Schema == item).ToList();
                    foreach (var table in views)
                    {
                        var t = schemaDbo.AddView(table.Name);
                        var columns = columnMetadataList.Where(x => x.TableMetadataId == table.Id).ToList();
                        foreach (var column in columns)
                        {
                            t.AddField(column.Alias);
                            if (column.IsForeignKey && !column.IsVirtualForeignKey)
                            {
                                var foreignKey = t.AddForeignKey(column.ForeignKeyConstraintName);

                                using (var referencedName = new MetadataQualifiedName())
                                {
                                    referencedName.Add(column.ForeignKeyTableName);
                                    referencedName.Add(column.ForeignKeyTableSchemaName);

                                    foreignKey.ReferencedObjectName = referencedName;
                                }

                                foreignKey.Fields.Add(column.Name);
                                foreignKey.ReferencedFields.Add(column.ForeignKeyColumnName);
                            }
                        }
                    }
                }
               
                //MetadataNamespace schemaDbo = metadataContainer.AddSchema("public");

                //MetadataObject test = schemaDbo.AddView("View");
                //MetadataField f = test.AddField("OrderId");
                //f.FieldType = System.Data.DbType.Int32;
                //// fields

                //test.AddField("CustomerId");

                //// prepare metadata for table "Orders"
                //MetadataObject orders = schemaDbo.AddTable("Orders");
                //// fields
                //orders.AddField("OrderId");
                //orders.AddField("CustomerId");

                //// prepare metadata for table "Order Details"
                //MetadataObject orderDetails = schemaDbo.AddTable("Order Details");
                //// fields
                //orderDetails.AddField("OrderId");
                //orderDetails.AddField("ProductId");
                //// foreign keys
                //MetadataForeignKey foreignKey = orderDetails.AddForeignKey("OrderDetailsToOrders");

                //using (MetadataQualifiedName referencedName = new MetadataQualifiedName())
                //{
                //    referencedName.Add("Orders");
                //    referencedName.Add("dbo");

                //    foreignKey.ReferencedObjectName = referencedName;
                //}

                //foreignKey.Fields.Add("OrderId");
                //foreignKey.ReferencedFields.Add("OrderId");
            }
            finally
            {
                metadataContainer.EndUpdate();
            }

            queryBuilder1.MetadataStructure.Refresh();
        }
        private void ResetQueryBuilderMetadata(QueryBuilder queryBuilder1)
        {
            queryBuilder1.MetadataProvider = null;
            queryBuilder1.ClearMetadata();
            // queryBuilder1.MetadataContainer.ItemMetadataLoading -= way2ItemMetadataLoading;
        }
        public async Task<ActionResult> GetViewIdNameList() 
        {
            var model = await _tableMetadataBusiness.GetList(x => x.TableType == TableTypeEnum.View);
            if (model!=null) 
            {
                var list = model.Select(e => new IdNameViewModel()
                {
                    Id=e.Id,
                    Name=e.Name,
                   
                }).ToList();
                return Json(list);
            }
            return Json(new List<IdNameViewModel>()) ;
        }
        public async Task<ActionResult> GetViewQueryById(string Id)
        {
            var model = await _tableMetadataBusiness.GetSingleById(Id);
            if (model != null)
            {
               
                return Json(new { success=true,query=model.Query});
            }
            return Json(new { success = false });
        }
        public async Task<IActionResult> ManageAQB(string query,string id)
        {
            var model = new TableMetadataViewModel();
           // model = await _tableMetadataBusiness.GetSingleById(id);
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _tableMetadataBusiness.GetSingleById(id);
                model.OldName = model.Name;
                model.OldSchema = model.Schema;
                model.Schema = model.Schema == "public" ? "gen":model.Schema ;
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                
                model.Query = query;
                model.TableType = TableTypeEnum.View;
                model.DataAction = DataActionEnum.Create;
                model.CreateTable = false;
            }            
             
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageAQB(TableMetadataViewModel model)
        {
            model.Query = model.Query.Replace("From ", " From ", StringComparison.InvariantCultureIgnoreCase);
            model.Query = model.Query.Replace("Where ", " Where ", StringComparison.InvariantCultureIgnoreCase);
            if (model.DataAction == DataActionEnum.Create)
            {
                model.DisplayName = model.Name;
                model.Alias = model.Name;
                model.TableType = TableTypeEnum.View;
                model.CreateTable = false;
                model.Schema = model.Schema=="gen"?"public": model.Schema;
                model.ColumnMetadatas = new List<ColumnMetadataViewModel>();
                var result = await _tableMetadataBusiness.Create(model);
                if (result.IsSuccess)
                {
                    // fetch the columns from view and create column metadata
                   var columnList= await _tableMetadataBusiness.GetViewColumnByTableName(model.Schema, model.DisplayName);
                   columnList.ForEach(x => x.TableMetadataId = result.Item.Id);
                    // var data1 = await _tableMetadataBusiness.GetSingleById(result.Item.Id);
                    // data1.ColumnMetadatas = columnList;
                    //var result1 = await _tableMetadataBusiness.Edit(data1);
                    foreach (var col in columnList)
                    {
                        var result2 = await _columnMetadataBusiness.Create(col);
                    }
                        
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
            }
            else
            {
                var data1 = await _tableMetadataBusiness.GetSingleById(model.Id);
                data1.Name = model.Name;               
                data1.DisplayName = model.Name;
                data1.OldName = model.OldName;
                data1.OldSchema = model.OldSchema;
                data1.Schema = model.Schema == "gen" ? "public" : model.Schema;
                data1.Alias = model.Name;
                data1.Query = model.Query;
                var existingcolumns = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == model.Id);
                data1.ColumnMetadatas = existingcolumns;
                var result = await _tableMetadataBusiness.Edit(data1);
                if (result.IsSuccess)
                {
                    List<string> newlistids = new List<string>();
                   
                    var NewcolumnList = await _tableMetadataBusiness.GetViewColumnByTableName(data1.Schema, data1.DisplayName);
                    foreach (var component in NewcolumnList)
                    {
                        newlistids.Add(component.Name);
                    }
                    var diff = existingcolumns.Where(item => !newlistids.Contains(item.Name));
                    if (diff.Any())
                    {
                        foreach (var item in diff)
                        {
                            await _columnMetadataBusiness.Delete(item.Id);
                        }
                    }                  
        
                    foreach (var newCol in NewcolumnList)
                    {
                        //and if column name is same and datatype is different edit them
                        if (existingcolumns.Any(x => x.Name == newCol.Name && x.DataType != newCol.DataType))
                        {
                            var rec = existingcolumns.Where(x => x.Name == newCol.Name && x.DataType != newCol.DataType).FirstOrDefault();
                            var col = await _columnMetadataBusiness.GetSingleById(rec.Id);
                            col.DataType = newCol.DataType;
                            var result2 = await _columnMetadataBusiness.Edit(col);
                        }
                        else if (existingcolumns.Any(x => x.Name == newCol.Name && x.DataType == newCol.DataType))
                        { 
                            // Do Nothing
                        }
                        else
                        {
                            // fetch new columns and create in column metadata 
                            newCol.TableMetadataId = result.Item.Id;
                            var result2 = await _columnMetadataBusiness.Create(newCol);
                        }
                    }   
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
            }

        }
        //[HttpPost]
        //public async Task<IActionResult> ManageAQB(TableMetadataViewModel model)
        //{
        //    model.Query=model.Query.Replace("From ", " From ",StringComparison.InvariantCultureIgnoreCase);
        //    model.Query = model.Query.Replace("Where ", " Where ", StringComparison.InvariantCultureIgnoreCase);
        //    if (model.DataAction == DataActionEnum.Create)
        //    {
        //        model.DisplayName = model.Name;
        //        model.Alias = model.Name;
        //        model.TableType = TableTypeEnum.View;
        //        model.CreateTable = false;
        //        var Q1 = model.Query.Split(new string[] { "From" }, StringSplitOptions.None);
        //        var Q2 = Q1[0].Split(',');
        //        if (Q2[0] != "Select *")
        //        {
        //            Q2[0] = Q2[0].Replace("Select", "");
        //            List<ColumnMetadataViewModel> list = new List<ColumnMetadataViewModel>();
        //            foreach (var data in Q2)
        //            {                       
        //                var Q3 = data.Split(new string[] { " As " }, StringSplitOptions.None);
        //                    if (Q3[0].Contains("Count", StringComparison.InvariantCultureIgnoreCase))
        //                    {
        //                    ColumnMetadataViewModel column = new ColumnMetadataViewModel();                           

        //                        var alias = Q3[1];
        //                        alias = Regex.Replace(Q3[1], @"[^0-9a-zA-Z]+", "");
        //                        column.Alias = alias.Trim();
        //                        column.Name = alias.Trim();
        //                        column.LabelName = alias.Trim();
        //                        column.DataType = DataColumnTypeEnum.Long;                               
        //                        column.DataAction = model.DataAction;
        //                        list.Add(column);                            
        //                }
        //                else if (Q3[0].Contains("avg",StringComparison.InvariantCultureIgnoreCase) ||
        //                    Q3[0].Contains("max", StringComparison.InvariantCultureIgnoreCase) ||
        //                    Q3[0].Contains("min", StringComparison.InvariantCultureIgnoreCase) ||
        //                    Q3[0].Contains("sum", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    ColumnMetadataViewModel column = new ColumnMetadataViewModel();

        //                    var alias = Q3[1];
        //                    alias = Regex.Replace(Q3[1], @"[^0-9a-zA-Z]+", "");
        //                    column.Alias = alias.Trim();
        //                    column.Name = alias.Trim();
        //                    column.LabelName = alias.Trim();
        //                    column.DataType = DataColumnTypeEnum.Double;
        //                    //column.UdfUIType = columnMetadata.UdfUIType;
        //                    column.DataAction = model.DataAction;
        //                    list.Add(column);
        //                }
        //                else if (Q3[0].Contains("."))
        //                    {
        //                        var Q4 = Q3[0].Split('.');
        //                    if (Q4.Length<3) 
        //                    {
        //                        Q1[1] = Q1[1].Replace("left ", " ", StringComparison.InvariantCultureIgnoreCase);
        //                        Q1[1] = Q1[1].Replace("right ", " ", StringComparison.InvariantCultureIgnoreCase); 
        //                        Q1[1] = Q1[1].Replace("inner ", " ", StringComparison.InvariantCultureIgnoreCase);
        //                        //Q1[1] = Q1[1].Replace("Join ", "join", StringComparison.InvariantCultureIgnoreCase);
        //                        var tst = Regex.Split(Q1[1], "join", RegexOptions.IgnoreCase);// Q1[1].Split(new string[] { " join " }, StringSplitOptions.None);
        //                        foreach (var rec in tst) 
        //                        {
        //                            var l1 = Regex.Split(rec, "As", RegexOptions.IgnoreCase);
        //                            if (l1.Length==2) 
        //                            {
        //                                if (rec.Contains(Q4[0].Trim(), StringComparison.InvariantCultureIgnoreCase))
        //                                {
        //                                    var l2 = Regex.Split(rec, " As ", RegexOptions.IgnoreCase);// rec.Split(new string[] { " As " }, StringSplitOptions.None);
        //                                    var Q5 = l2[0].Split(".");
        //                                    Array.Resize(ref Q5, Q5.Length + 1);
        //                                    Array.Resize(ref Q4, Q4.Length + 1);
        //                                    Q5[Q5.GetUpperBound(0)] = Regex.Replace(Q4[1], @"[^0-9a-zA-Z]+", "");
        //                                    Q4 = Q5;
        //                                }
        //                            }                                   
        //                        }
        //                    }
        //                      // Q4[2] = Q4[2].Split(new string[] { " As " }, StringSplitOptions.None)[0];
        //                        ColumnMetadataViewModel column = new ColumnMetadataViewModel();
        //                        var columnMetadata = await _tableMetadataBusiness.GetColumnByTableName(Q4[0], Q4[1], Q4[2]);
        //                        if (columnMetadata != null)
        //                        {
        //                            var alias = Q3[1];
        //                            alias = Regex.Replace(Q3[1], @"[^0-9a-zA-Z]+", "");
        //                            column.Alias = alias.Trim();
        //                            column.Name = alias.Trim();
        //                            column.LabelName = alias.Trim();
        //                            column.DataType = columnMetadata.DataType;
        //                            column.UdfUIType = columnMetadata.UdfUIType;
        //                            column.DataAction = model.DataAction;
        //                            list.Add(column);
        //                        }
        //                    }
        //                    //else
        //                    //{
        //                    //   // Console.WriteLine(data1);
        //                    //}
        //            }
        //            model.ColumnMetadatas = list;
        //        }
        //        var result = await _tableMetadataBusiness.Create(model);
        //        if (result.IsSuccess)
        //        {

        //            return Json(new { success = true });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, error = result.Messages.ToHtmlError() });
        //        }
        //    }
        //    else  
        //    {               
        //        var data1 = await _tableMetadataBusiness.GetSingleById(model.Id);
        //        data1.Name = model.Name;
        //        data1.Schema = model.Schema;
        //        data1.DisplayName = model.Name;
        //        data1.OldName = model.OldName;
        //        data1.OldSchema = model.OldSchema;              
        //        data1.Alias = model.Name;
        //        data1.Query = model.Query;
        //        var Q1 = model.Query.Split(new string[] { "From" }, StringSplitOptions.None);
        //        var Q2 = Q1[0].Split(',');
        //        if (Q2[0] != "Select *")
        //        {
        //            Q2[0] = Q2[0].Replace("Select", "");
        //            List<ColumnMetadataViewModel> list = new List<ColumnMetadataViewModel>();
        //            foreach (var data in Q2)
        //            {
        //                var Q3 = data.Split(new string[] { " As " }, StringSplitOptions.None);
        //                // var Q3 = data.Split('.');
        //                // Q3[2] = Q3[2].Split(new string[] { " As " }, StringSplitOptions.None)[0];
        //                if (Q3[0].Contains("Count", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    ColumnMetadataViewModel column = new ColumnMetadataViewModel();

        //                    var alias = Q3[1];
        //                    alias = Regex.Replace(Q3[1], @"[^0-9a-zA-Z]+", "");
        //                    column.Alias = alias.Trim();
        //                    column.Name = alias.Trim();
        //                    column.LabelName = alias.Trim();
        //                    column.DataType = DataColumnTypeEnum.Long;
        //                    column.DataAction = model.DataAction;
        //                    list.Add(column);
        //                }
        //                else if (Q3[0].Contains("avg", StringComparison.InvariantCultureIgnoreCase) ||
        //                    Q3[0].Contains("max", StringComparison.InvariantCultureIgnoreCase) ||
        //                    Q3[0].Contains("min", StringComparison.InvariantCultureIgnoreCase) ||
        //                    Q3[0].Contains("sum", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    ColumnMetadataViewModel column = new ColumnMetadataViewModel();

        //                    var alias = Q3[1];
        //                    alias = Regex.Replace(Q3[1], @"[^0-9a-zA-Z]+", "");
        //                    column.Alias = alias.Trim();
        //                    column.Name = alias.Trim();
        //                    column.LabelName = alias.Trim();
        //                    column.DataType = DataColumnTypeEnum.Double;
        //                    //column.UdfUIType = columnMetadata.UdfUIType;
        //                    column.DataAction = model.DataAction;
        //                    list.Add(column);
        //                }
        //                else if (Q3[0].Contains("."))
        //                {
        //                    var Q4 = Q3[0].Split('.');
        //                    if (Q4.Length < 3)
        //                    {
        //                        Q1[1] = Q1[1].Replace("left ", " ", StringComparison.InvariantCultureIgnoreCase);
        //                        Q1[1] = Q1[1].Replace("right ", " ", StringComparison.InvariantCultureIgnoreCase);
        //                        Q1[1] = Q1[1].Replace("inner ", " ", StringComparison.InvariantCultureIgnoreCase);
        //                        //Q1[1] = Q1[1].Replace("Join ", "join", StringComparison.InvariantCultureIgnoreCase);
        //                        var tst = Regex.Split(Q1[1], "join", RegexOptions.IgnoreCase);// Q1[1].Split(new string[] { " join " }, StringSplitOptions.None);
        //                        foreach (var rec in tst)
        //                        {
        //                            var l1 = Regex.Split(rec, "As", RegexOptions.IgnoreCase);
        //                            if (l1.Length == 2)
        //                            {
        //                                if (rec.Contains(Q4[0].Trim(), StringComparison.InvariantCultureIgnoreCase))
        //                                {
        //                                    var l2 = Regex.Split(rec, " As ", RegexOptions.IgnoreCase);// rec.Split(new string[] { " As " }, StringSplitOptions.None);
        //                                    var Q5 = l2[0].Split(".");
        //                                    Array.Resize(ref Q5, Q5.Length + 1);
        //                                    Array.Resize(ref Q4, Q4.Length + 1);
        //                                    Q5[Q5.GetUpperBound(0)] = Regex.Replace(Q4[1], @"[^0-9a-zA-Z]+", "");
        //                                    Q4 = Q5;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    // Q4[2] = Q4[2].Split(new string[] { " As " }, StringSplitOptions.None)[0];
        //                    ColumnMetadataViewModel column = new ColumnMetadataViewModel();
        //                    var columnMetadata = await _tableMetadataBusiness.GetColumnByTableName(Q4[0], Q4[1], Q4[2]);
        //                    if (columnMetadata != null)
        //                    {
        //                        var alias = Q3[1];
        //                        alias = Regex.Replace(Q3[1], @"[^0-9a-zA-Z]+", "");
        //                        column.Alias = alias.Trim();
        //                        column.Name = alias.Trim();
        //                        column.LabelName = alias.Trim();
        //                        column.DataType = columnMetadata.DataType;
        //                        column.UdfUIType = columnMetadata.UdfUIType;
        //                        column.DataAction = model.DataAction;
        //                        list.Add(column);
        //                    }
        //                }
        //            }
        //            data1.ColumnMetadatas = list;
        //        }
        //        List<string> newlistids = new List<string>();
        //        var existingcolumns = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == model.Id);
        //        foreach (var component in data1.ColumnMetadatas)
        //        {
        //            newlistids.Add(component.Id);
        //        }
        //        var diff = existingcolumns.Where(item => !newlistids.Contains(item.Id));
        //        if (diff.Any())
        //        {
        //            foreach (var item in diff)
        //            {
        //                await _columnMetadataBusiness.Delete(item.Id);
        //            }
        //        }
        //        var result = await _tableMetadataBusiness.Edit(data1);
        //        if (result.IsSuccess)
        //        {

        //            return Json(new { success = true });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, error = result.Messages.ToHtmlError() });
        //        }
        //    }

        //}
    }
}
