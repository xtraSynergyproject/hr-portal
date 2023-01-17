using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class RemoteWorkResultViewModel
    {

        public DateTime MonitorIntervalFrom { get; set; }
        public DateTime MonitorIntervalTo { get; set; }
        public DateTime CapturedDate { get; set; }
        public int MouseClicks { get; set; }
        public int KeyboardClicks { get; set; }
        public string ActiveScreenBase64 { get; set; }
        public string ClientUserId { get; set; }
        public ClientUserTypeEnum ClientUserType { get; set; }
    }
    [Serializable]
    public class RemoteWorkResultListViewModel
    {
        public string ClientUserId { get; set; }
        public ClientUserTypeEnum ClientUserType { get; set; }
        public DateTime ClientDate { get; set; }
        public List<string> ResultItems { get; set; }
    }
}
