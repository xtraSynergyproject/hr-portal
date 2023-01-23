﻿using CMS.Common;
using CMS.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class EGovUserViewModel
    {

        public string userId { get; set; }
        public string name { get; set; }
        public string aadharNo { get; set; }
        public string houseNo { get; set; }
        public string street { get; set; }
        public string wardName { get; set; }
        public string talukZone { get; set; }
        public string areaVillage { get; set; }
        public string postOffice { get; set; }
        public string districtCity { get; set; }
        public string stateName { get; set; }
        public string pincode { get; set; }
        public string nationality { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string emailId { get; set; }
        public string contactNumber { get; set; }
        public string departmentId { get; set; }
        public string[] role { get; set; }
        public string staffId { get; set; }
        public string bloodGroup { get; set; }
        public string password { get; set; }


    }
}
