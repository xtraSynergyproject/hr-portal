using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
namespace CMS.UI.ViewModel
{
    public class FileViewModel: File
    {        
        public byte[] ContentByte { get; set; }
        public byte[] SnapshotContentByte { get; set; }
        public string CreatedDateDisplay
        {
            get
            {               
                    var d =Humanizer.DateHumanizeExtensions.Humanize(CreatedDate);
                    return d;   
            }
        }
        public string Size
        {
            get
            {                
                var d = Humanizer.ByteSizeExtensions.Bytes(ContentLength);
                return d.ToString();
            }
        }
        public string MediaNewType
        {
            get
            {

                if (FileName != null)
                {
                    //if (CloudDocumentUrl != null && CloudDocumentUrl.Contains("http"))
                    //{
                    //    return "CLOUD";
                    //}
                    //if (System.Web.MimeMapping.GetMimeMapping(FileName).StartsWith("image/"))
                    //{
                    //    return "PHOTO";
                    //}
                    if (FileName.Contains(".png",StringComparison.InvariantCultureIgnoreCase) || FileName.Contains(".jpg", StringComparison.InvariantCultureIgnoreCase)
                        || FileName.Contains(".jpeg", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return "PHOTO";
                    }
                    else if (FileName.Contains(".pdf") || FileName.Contains(".PDF"))
                    {
                        return "PDF";
                    }
                    else
                    {
                        return "VIDEO";
                    }
                }
                return "MESSAGE";
            }
        }

        //public string StreamVideoPath
        //{
        //    get { return ApplicationConstant.AppSettings.ApplicationBaseUrl() + "api/video"; }
        //}
    }
}
