using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class DependentViewModel
    {       
        public string DependentId { get; set; }
        public string PersonId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; } 
        public string IqamahIdNationalityId { get; set; }        
        public string RelationshipTypeName { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public string DateOfBirth { get; set; }
        public string NoteId { get; set; }
        public string UserRole { get; set; }

    }
}
