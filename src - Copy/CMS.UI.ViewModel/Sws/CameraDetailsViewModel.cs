using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class CameraDetailsViewModel
    {
        public string SrNo { get; set; }
        public string CameraName { get; set; }
        public string LocationName { get; set; }
        public string PoliceStation { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string IpAddress { get; set; }
        public string RtspLink { get; set; }
        public string TypeOfCamera { get; set; }
        public string Make { get; set; }      
        

    }
    
    public class CameraDetailsArrayViewModel
    {
        public string[] SrNo { get; set; }
        public string[] CameraName { get; set; }
        public string[] LocationName { get; set; }
        public string[] PoliceStation { get; set; }
        public string[] Longitude { get; set; }
        public string[] Latitude { get; set; }
        public string[] IpAddress { get; set; }
        public string[] RtspLink { get; set; }
        public string[] TypeOfCamera { get; set; }
        public string[] Make { get; set; }      
        

    }
    public class CameraDetails2ViewModel
    {
        public string SrNo { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string PoliceStation { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string SwitchHostName { get; set; }
        public string IpAddress { get; set; }
        public string RtspLink { get; set; }       
        


    }

    public class NewsFeedsViewModel
    {
        public string name { get; set; }
        public string _index { get; set; }
        public string author { get; set; }
        public string message { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string published { get; set; }


    }
    public class NewsFeedsArrayViewModel
    {
        public string[] name { get; set; }
        public string[] _index { get; set; }
        public string[] author { get; set; }
        public string[] message { get; set; }
        public string[] title { get; set; }
        public string[] link { get; set; }
        public string[] published { get; set; }


    }
    public class TrackChartViewModel
    {
        public string Name { get; set; }
        public string Keyword { get; set; }
        public string Count { get; set; }
    }

    public class DateFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class DocumentSearchArrayViewModel
    {
        public string[] notesubject { get; set; }
        public string[] notedescription { get; set; }
        public string[] noteno { get; set; }
        public string[] filename { get; set; }  

    }
}
