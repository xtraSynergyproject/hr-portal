using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class IIPCameraViewModel : NoteTemplateViewModel
    {
        public string City { get; set; }
        public string Location { get; set; }
        public string PoliceStation { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string IpAddress { get; set; }
        public string SwitchHostName { get; set; }
        public string RtspLink { get; set; }
        public string TypeOfCamera { get; set; }
        public string Make { get; set; }
    }
    public class StreamingViewModel
    {
        public string RTSP_Id { get; set; }
        public string RTSP_Url { get; set; }
        public string RTSP_User { get; set; }
        public string RTSP_Pwd { get; set; }
        public string ServerId { get; set; }
    }
    public class CctvCameraViewModel 
    {
        public string Id { get; set; }
        public string CameraName { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string PoliceStation { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string IpAddress { get; set; }
        public string SwitchHostName { get; set; }
        public string RtspLink { get; set; }
        public string TypeOfCamera { get; set; }
        public string Make { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
    public class CctvCameraArrayViewModel
    {        
        public string[] CameraName { get; set; }
        public string[] City { get; set; }
        public string[] Location { get; set; }
        public string[] PoliceStation { get; set; }
        public string[] Longitude { get; set; }
        public string[] Latitude { get; set; }
        public string[] IpAddress { get; set; }
        public string[] SwitchHostName { get; set; }
        public string[] RtspLink { get; set; }
        public string[] TypeOfCamera { get; set; }
        public string[] Make { get; set; }       
    }
    public class WatchlistViewModel
    {
        public bool isAdvance { get; set; }
        public bool plainSearch { get; set; }
        public SocialMediaDatefilters dateFilterType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
