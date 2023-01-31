using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class DocumentTagViewModel
    {
        public long Id { get; set; }
        public string TagCategory { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public long DocumentId { get; set; }
        public IList<DocumentTagViewModel> Tags { get; set; }
        public string TagsCollection { get; set; }
        public DataOperation? Operation { get; set; }
        public long? ParentTagCategoryId { get; set; }
        public long? ParentWorkSpaceId { get; set; }
        public bool? HasChildren { get; set; }
        public TagTypeEnum? TagType { get; set; }
        public long? TagSourceId { get; set; }
        public string CreatedBy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? CreatedDate { get; set; }
        public long? VersionNo { get; set; }
        public long? RelationId { get; set; }
        public bool? RelationIsDelete { get; set; }
        public bool? IsSystemAdmin { get; set; }
        public bool? IsSystemTagCategory { get; set; }

    }
    public class TagCategoryViewModel:ViewModelBase
    {
        [Display(Name = "Is System Tag Category")]
        public bool? IsSystemTagCategory { get; set; }
        public long? TagCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }

        public long DocumentId { get; set; }
        public IList<DocumentTagViewModel> Tags { get; set; }
        public string TagsCollection { get; set; }
        
        public long[] ParentTagCategoryId { get; set; }
        public long? WorkSpaceId { get; set; }
        public bool? HasChildren { get; set; }
        public TagTypeEnum? TagType { get; set; }
        public long? TagSourceId { get; set; }
        public int TagCount { get; set; }
        public long? VersionNo { get; set; }
        public long? RelationId { get; set; }
        public bool? RelationIsDelete { get; set; }
    }
}
