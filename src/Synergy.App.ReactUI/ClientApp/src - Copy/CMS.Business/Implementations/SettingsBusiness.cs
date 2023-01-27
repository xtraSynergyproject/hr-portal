using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class SettingsBusiness : BusinessBase<DocumentTypeViewModel, DocumentType>, ISettingsBusiness
    {
        public SettingsBusiness(IRepositoryBase<DocumentTypeViewModel, DocumentType> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async Task<List<TreeViewViewModel>> GetDocumentTypeTreeList(string id)
        {
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                list.Add(new TreeViewViewModel
                {
                    id = DocumentTypeEnum.Root.ToString(),
                    Name = "DocumentType",
                    DisplayName = "Document Type",
                    ParentId = null,
                    hasChildren = true,
                    IconCss = "",
                    children = true,
                    text = "Document Type",
                    parent = "#",
                    a_attr = new { data_id = DocumentTypeEnum.Root.ToString(), data_name = "Document Type", data_type = "ROOT" }
                });
            }
            else if (id == DocumentTypeEnum.Root.ToString())
            {
                list.Add(new TreeViewViewModel
                {
                    id = DocumentTypeEnum.CompositionRoot.ToString(),
                    Name = "Composition",
                    DisplayName = "Composition",
                    ParentId = null,
                    hasChildren = true,
                    IconCss = "",
                    children = true,
                    text = "Composition",
                    parent = id,
                    a_attr = new { data_id = DocumentTypeEnum.CompositionRoot.ToString(), data_name = "Composition", data_type = "CHILD" }
                });
            }
            return list;
        }
    }
}
