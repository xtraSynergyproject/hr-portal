using ERP.Data.GraphModel;
using ERP.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace ERP.UI.ViewModel
{
    [Serializable]
    public class ViewModelBase 
    {

        [Display(Name = "Id", ResourceType = typeof(ERP.Translation.General))]
        public virtual long Id { get; set; }
        [Display(Name = "Status", ResourceType = typeof(ERP.Translation.General))]
        public virtual StatusEnum Status { get; set; }
        public virtual long IsDeleted { get; set; }
        public virtual long VelocityId { get; set; }
        public virtual long VelocityObjectId { get; set; }

        [Display(Name = "Is Active?")]
        public virtual bool? IsActive { get; set; }

        public LayoutModeEnum? LayoutMode { get; set; }
        public string PageLayout { get; set; }
        //public ModuleEnum? ModuleName { get; set; }
        public long BlockId { get; set; }
        public string LoggedInUserName
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToString(HttpContext.Current.Session[Constant.SessionVariable.UserName]);
                }
                return "System";
            }
        }
        public long LoggedInUserId
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null)
                {
                    return Constant.WindowsServiceUserId;
                }
                return Convert.ToInt64(HttpContext.Current.Session[Constant.SessionVariable.UserId]);
            }
        }
        public long? LoggedInAsByUserId { get; set; }
        public string ReturnUrl { get; set; }
        public string RequestSource { get; set; }
        public virtual string SubmitButtonText
        {
            get
            {
                if (Operation == DataOperation.Delete)
                {
                    return ERP.Translation.General.Delete;// "Delete";
                }
                else
                {
                    return ERP.Translation.General.Save;// "Save";
                }
            }
        }


        public long? LogId { get; set; }



        private long _createdBy;
        private DateTime _createdDate;
        private DateTime _lastUpdatedDate;
        private long _lastUpdatedBy;
        private long? _companyId;
        public virtual long? CompanyId
        {
            get
            {
                if (_companyId == null)
                {
                    if (HttpContext.Current == null || HttpContext.Current.Session == null)
                    {
                        return Constant.DefaultCompanyId;
                    }
                    return Convert.ToString(HttpContext.Current.Session[Constant.SessionVariable.CompanyId]).ToSafeLong();
                }
                else
                {
                    return _companyId;
                }
            }
            set
            {
                _companyId = value;
            }
        }


        public long CreatedBy
        {
            get
            {
                if (_createdBy == 0 && Operation != null && Operation != DataOperation.Read)
                {
                    return LoggedInUserId;
                }
                else
                {
                    return _createdBy;
                }
            }
            set
            {
                _createdBy = value;
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public virtual DateTime CreatedDate
        {
            get
            {
                if (_createdDate == Constant.SystemMinDate && Operation != null && Operation != DataOperation.Read)
                {
                    return DateTime.Now.ApplicationNow();
                }
                else
                {
                    return _createdDate;
                }
            }
            set
            {
                _createdDate = value;
            }
        }

        public long LastUpdatedBy
        {
            get
            {
                if (_lastUpdatedBy == 0 && Operation != null && Operation != DataOperation.Read)
                {
                    return LoggedInUserId;
                }
                else
                {
                    return _lastUpdatedBy;
                }
            }
            set
            {
                _lastUpdatedBy = value;
            }
        }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime LastUpdatedDate
        {
            get
            {
                if (_lastUpdatedDate == Constant.SystemMinDate && Operation != null && Operation != DataOperation.Read)
                {
                    return DateTime.Now.ApplicationNow();
                }
                else
                {
                    return _lastUpdatedDate;
                }


            }
            set
            {
                _lastUpdatedDate = value;
            }
        }

        private SourceSystemOwnerEnum? _SourceSystemOwner;
        public SourceSystemOwnerEnum? SourceSystemOwner
        {
            get
            {
                if (_SourceSystemOwner == null)
                {
                    return SourceSystemOwnerEnum.SYSTEM;
                }
                else
                {
                    return _SourceSystemOwner;
                }


            }
            set
            {
                _SourceSystemOwner = value;
            }
        }
        public string SourceSystemId { get; set; }
        public DataOperation? Operation { get; set; }

        //public string[] VisibilityFields { get; set; }
        //public string[] ModifyFields { get; set; }

        public string VisibilityPermission { get; set; }
        //public string VisibilityPermission
        //{
        //    get
        //    {
        //        if(_VisibilityPermission.IsNotNullAndNotEmpty())
        //        {
        //            return _VisibilityPermission;
        //        }
        //        if(VisibilityFields!=null)
        //        _VisibilityPermission = string.Concat(",", string.Join(",", VisibilityFields), ",");
        //        return _VisibilityPermission;
        //    }
        //    set
        //    {
        //        _VisibilityPermission = value;
        //    }
        //}
        public string ModifyPermission { get; set; }
        //public string ModifyPermission
        //{
        //    get
        //    {
        //        if (_ModifyPermission.IsNotNullAndNotEmpty())
        //        {
        //            return _ModifyPermission;
        //        }
        //        if (ModifyFields != null)
        //            _ModifyPermission = string.Concat(",", string.Join(",", ModifyFields), ",");
        //        return _ModifyPermission;
        //    }
        //    set
        //    {
        //        _ModifyPermission = value;
        //    }
        //}
        //public long? VersionNo { get; set; }
        // public bool IsCreatingNewVersion { get; set; }
        public long? VersionedByUserId { get; set; }
        public DateTime? VersionedDate { get; set; }
        public bool IsValidated { get; set; }
        public virtual long? NodeId { get; set; }
        
        public string LoggedInUserLegalEntityCode
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToString(HttpContext.Current.Session[Constant.SessionVariable.LegalEntityCode]);
                }
                return "CAYAN_KSA";
            }
        }
        //public bool IsAdmin
        //{
        //    get
        //    {
                 
        //            return Convert.ToBoolean(HttpContext.Current.Session[Constant.SessionVariable.IsAdmin]);
                 
        //    }
        //}
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        private long loggedInGenUserId; // field

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public long LoggedInGenUserId
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null)
                {
                    return loggedInGenUserId != 0 ? loggedInGenUserId : Constant.WindowsServiceUserId;
                }
                return Convert.ToInt64(HttpContext.Current.Session[Constant.SessionVariable.UserId]);
            }
            set { loggedInGenUserId = value; }
        }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
