﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class UserHierarchyViewModel : UserHierarchy
    {
        [Display(Name = "Hierarchy Name")]
        public string HierarchyId { get; set; }

        public string UserNameWithEmail { get; set; }
        public string SponsorName { get; set; }


        // [Required]
        [Display(Name = "Parent User")]
        public string ParentUserName { get; set; }


        [Display(Name = "Hierarchy Admin")]
        public long? AdminUserId { get; set; }
        [Display(Name = "Hierarchy Admin")]
        public string AdminUserName { get; set; }

        public List<IdNameViewModel> Colleagues { get; set; }
        public long? RelationshipId { get; set; }
   
        public virtual string EmployeeNo { get; set; }
        [Display(Name = "Employee Name")]
        public virtual string EmployeeName { get; set; }
        public virtual string Email { get; set; }
        public virtual string IqamahNo { get; set; }
        public virtual string DisplayName { get; set; }
        [Display(Name = "Department Name")]
        public virtual string OrganizationName { get; set; }
        public virtual string JobName { get; set; }
        public virtual string GradeName { get; set; }
        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }
        public string UserName { get; set; }
        public string HierarchyName { get; set; }
        public string Level1ApproverOption1UserId { get; set; }
        public string Level1ApproverOption2UserId { get; set; }
        public string Level1ApproverOption3UserId { get; set; }
           
        public string Level2ApproverOption1UserId { get; set; }
        public string Level2ApproverOption2UserId { get; set; }
        public string Level2ApproverOption3UserId { get; set; }
              
        public string Level3ApproverOption1UserId { get; set; }
        public string Level3ApproverOption2UserId { get; set; }
        public string Level3ApproverOption3UserId { get; set; }
              
        public string Level4ApproverOption1UserId { get; set; }
        public string Level4ApproverOption2UserId { get; set; }
        public string Level4ApproverOption3UserId { get; set; }
             
        public string Level5ApproverOption1UserId { get; set; }
        public string Level5ApproverOption2UserId { get; set; }
        public string Level5ApproverOption3UserId { get; set; }             
        public string Level1ApproverOption1UserName { get; set; }
        public string Level1ApproverOption2UserName { get; set; }
        public string Level1ApproverOption3UserName { get; set; }
              
        public string Level2ApproverOption1UserName { get; set; }
        public string Level2ApproverOption2UserName { get; set; }
        public string Level2ApproverOption3UserName { get; set; }
             
        public string Level3ApproverOption1UserName { get; set; }
        public string Level3ApproverOption2UserName { get; set; }
        public string Level3ApproverOption3UserName { get; set; }
           
        public string Level4ApproverOption1UserName { get; set; }
        public string Level4ApproverOption2UserName { get; set; }
        public string Level4ApproverOption3UserName { get; set; }
             
        public string Level5ApproverOption1UserName { get; set; }
        public string Level5ApproverOption2UserName { get; set; }
        public string Level5ApproverOption3UserName { get; set; }
        [Display(Name = "Level1 Name")]
        public string Level1Name { get; set; }
        [Display(Name = "Level2 Name")]
        public string Level2Name { get; set; }
        [Display(Name = "Level3 Name")]
        public string Level3Name { get; set; }
        [Display(Name = "Level4 Name")]
        public string Level4Name { get; set; }
        [Display(Name = "Level5 Name")]
        public string Level5Name { get; set; }      
        [Display(Name = "User")]
        public string UserIds { get; set; }
        public string UserLevels { get; set; }
        public bool Select { get; set; }
        public string Type { get; set; }
    }
}