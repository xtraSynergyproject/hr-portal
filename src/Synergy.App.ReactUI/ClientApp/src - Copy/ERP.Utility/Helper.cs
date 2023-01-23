using NVelocity;
using NVelocity.App;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ERP.Utility
{
    public class Helper
    {
        public static bool HasPropertyName<T>(string txtToCheckIn, Expression<Func<T>> propertyLambda)
        {
            MemberExpression me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }


            return txtToCheckIn.Contains(string.Concat(",", me.Member.Name, ","));
        }

        public static string GetDomain(Uri url)
        {
            var ret = url.DnsSafeHost;
            if (!url.IsDefaultPort)
            {
                ret = string.Concat(ret, ":", url.Port);
            }
            ret = string.Concat(url.Scheme, @"://", ret);
            return ret;
        }

        public static dynamic ConvertToDynamicObject(IDictionary<String, Object> dictionary)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;

            foreach (var keyValuePair in dictionary)
            {
                expandoObjCollection.Add(keyValuePair);
            }
            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }
        public static List<List<T>> ToChunks<T>(IList<T> source, int chunkSize)
        {
            return source.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
        public static double LeaveDuration(DateTime startDate, DateTime endDate, bool isHalfDay = false)
        {
            if (isHalfDay)
            {
                return 0.5;
            }
            return (endDate - startDate).TotalDays;
        }

        public static T DeserializeFromString<T>(string settings)
        {
            byte[] b = Convert.FromBase64String(settings);
            using (var stream = new MemoryStream(b))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static string SerializeToString<T>(T data)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
                stream.Flush();
                stream.Position = 0;
                return Convert.ToBase64String(stream.ToArray());
            }
        }
        public static double LeaveDurationForHours(double hours, double workingHours = 8)
        {
            return hours / workingHours;
        }

        public static string RandomColor()
        {
            return ColorList[Random.Next(9)];
        }
        private static List<string> ColorList = new List<string> { "#F08963", "#84B779", "#5BA79F", "#7A8F9D", "#4F67B6", "#5CBBF3", "#696969", "#555D50", "#1B4D3E", "#3B7A57" };
        private static Random Random = new Random();
        public static string OrganizationMappingForReports(long userId, long companyId, long legalEntityId)
        {
            var date = DateTime.Now.Date.ToUTCFormat();
            return string.Concat(@"match(user:ADM_User{IsDeleted:0,Status:'Active',CompanyId:", companyId, @",Id:", userId, @"})
            optional match (adminorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            <-[:R_OrganizationRoot]-(adminorgo:HRS_Organization)-[:R_Organization_LegalEntity_OrganizationRoot]->
            (adminorgle:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @",Id:", legalEntityId, @"})
            where user.IsAdmin=true
           
            optional match(user)-[usermapping:R_UserReport_Mapping_OrganizationRoot]->(userorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            optional match path =(userorg)<-[upr:R_OrganizationRoot_ParentOrganizationRoot*0..]
            -(userchildorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"}) 
            WHERE all(rel in upr WHERE rel.EffectiveStartDate<='", date, @"' and rel.EffectiveEndDate>='", date, @"' and 
            (usermapping.ExcludeAllChild is null  or usermapping.ExcludeAllChild=false ) )
            optional match(user)-[userrole:R_User_UserRole]->(role:ADM_UserRole{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            optional match(role)-[roleMapping:R_UserRole_Mapping_OrganizationRoot]->(roleorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            optional match pathr =(roleorg)<-[rpr:R_OrganizationRoot_ParentOrganizationRoot*0..]
            -(rolechildorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"}) 
            WHERE all(rel in rpr WHERE rel.EffectiveStartDate<='", date, @"' and rel.EffectiveEndDate>='", date, @"' and 
            (roleMapping.ExcludeAllChild is null  or roleMapping.ExcludeAllChild=false ) )
            WITH collect(adminorg.Id)+collect(userchildorg.Id)+collect(rolechildorg.Id) as orgs unwind orgs as AllowedOrganizationIds
            with distinct  AllowedOrganizationIds ");
        }

        public static bool IsVideoFile(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",mp4,mpeg,wmv,".Contains(ext);
        }
        public static bool IsImageFile(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",jpg,jpeg,png,gif,bmp,".Contains(ext);
        }
        public static bool IsPdfFile(string ext)
        {
            return ext.ToLower() == "pdf";
        }
        public static bool IsExcelFile(string ext)
        {
            return ext.ToLower().Contains("xls");
        }
        public static bool IsAudioFile(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",mp3,".Contains(ext);
        }
        public static bool Is2JpegSupportable(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",jpeg,jpg,tiff,tif,gif,png,bmp,pdf,xps,ico,wbm,psd,psp,html,htm,txt,rtf,doc,docx,xls,xlsx,xlsm,ppt,pptx,pps,ppsx,vsd,vsdx,cdr,".Contains(ext);
        }
        public static string OrganizationMapping(long userId, long companyId, long legalEntityId)
        {
            var date = DateTime.Now.Date.ToUTCFormat();
            return string.Concat(@"match(user:ADM_User{IsDeleted:0,Status:'Active',CompanyId:", companyId, @",Id:", userId, @"})
            optional match (adminorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            <-[:R_OrganizationRoot]-(adminorgo:HRS_Organization)-[:R_Organization_LegalEntity_OrganizationRoot]->
            (adminorgle:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @",Id:", legalEntityId, @"})
            where user.IsAdmin=true
           
            optional match(user)-[usermapping:R_User_Mapping_OrganizationRoot]->(userorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            optional match path =(userorg)<-[upr:R_OrganizationRoot_ParentOrganizationRoot*0..]
            -(userchildorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"}) 
            WHERE all(rel in upr WHERE rel.EffectiveStartDate<='", date, @"' and rel.EffectiveEndDate>='", date, @"' and 
            (usermapping.ExcludeAllChild is null  or usermapping.ExcludeAllChild=false ) )
            optional match(user)-[userrole:R_User_UserRole]->(role:ADM_UserRole{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            optional match(role)-[roleMapping:R_UserRole_Mapping_OrganizationRoot]->(roleorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            optional match pathr =(roleorg)<-[rpr:R_OrganizationRoot_ParentOrganizationRoot*0..]
            -(rolechildorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"}) 
            WHERE all(rel in rpr WHERE rel.EffectiveStartDate<='", date, @"' and rel.EffectiveEndDate>='", date, @"' and 
            (roleMapping.ExcludeAllChild is null  or roleMapping.ExcludeAllChild=false ) )
            WITH collect(adminorg.Id)+collect(userchildorg.Id)+collect(rolechildorg.Id) as orgs unwind orgs as AllowedOrganizationIds
            with distinct  AllowedOrganizationIds ");


            //optional match(user)-[:R_User_PersonRoot]->(userpersonroot: HRS_PersonRoot{ IsDeleted: 0,Status: 'Active',CompanyId: ", companyId, @"})
            //< -[:R_AssignmentRoot_PersonRoot] - (userassignmentroot: HRS_AssignmentRoot{ IsDeleted: 0,Status: 'Active',CompanyId: ", companyId, @"})
            //< -[:R_AssignmentRoot] - (userassignment: HRS_Assignment{ IsLatest: true,IsDeleted: 0,Status: 'Active',CompanyId: ", companyId, @"})
            //-[:R_Assignment_OrganizationRoot]->(userassignmentor: HRS_OrganizationRoot{ IsDeleted: 0,Status: 'Active',CompanyId: ", companyId, @"})


            //var date = DateTime.Now.Date.ToUTCFormat();
            //return string.Concat(@"match(user:ADM_User{IsDeleted:0,Status:'Active',CompanyId:", companyId, @",Id:", userId, @"})
            //optional match (adminorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            //<-[:R_OrganizationRoot]-(adminorgo:HRS_Organization)-[:R_Organization_LegalEntity_OrganizationRoot]->
            //(adminorgle:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @",Id:", legalEntityId, @"})
            //where user.IsAdmin=true
            //optional match(user)-[:R_User_PersonRoot]->(userpersonroot:HRS_PersonRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            //<-[:R_AssignmentRoot_PersonRoot]-(userassignmentroot:HRS_AssignmentRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            //<-[:R_AssignmentRoot]-(userassignment:HRS_Assignment{IsLatest:true,IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            //-[:R_Assignment_OrganizationRoot]->(userassignmentor:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            //optional match(user)-[usermapping:R_User_Mapping_OrganizationRoot]->(userorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            //optional match path =(userorg)<-[upr:R_OrganizationRoot_ParentOrganizationRoot*0..]
            //-(userchildorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"}) 
            //WHERE all(rel in upr WHERE rel.EffectiveStartDate<='", date, @"' and rel.EffectiveEndDate>='", date, @"' and 
            //(usermapping.ExcludeAllChild is null  or usermapping.ExcludeAllChild=false ) )
            //optional match(user)-[userrole:R_User_UserRole]->(role:ADM_UserRole{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            //optional match(role)-[roleMapping:R_UserRole_Mapping_OrganizationRoot]->(roleorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            //optional match pathr =(roleorg)<-[rpr:R_OrganizationRoot_ParentOrganizationRoot*0..]
            //-(rolechildorg:HRS_OrganizationRoot{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"}) 
            //WHERE all(rel in rpr WHERE rel.EffectiveStartDate<='", date, @"' and rel.EffectiveEndDate>='", date, @"' and 
            //(roleMapping.ExcludeAllChild is null  or roleMapping.ExcludeAllChild=false ) )
            //WITH collect(adminorg.Id)+collect(userchildorg.Id)+collect(rolechildorg.Id)+collect(userassignmentor.Id) as orgs unwind orgs as AllowedOrganizationIds
            //with distinct  AllowedOrganizationIds ");
        }

        public static string GetPayrollExecutionStatusCss(PayrollExecutionStatusEnum? stauts)
        {
            switch (stauts)
            {
                case PayrollExecutionStatusEnum.NotStarted:
                    return "label label-default";
                case PayrollExecutionStatusEnum.Submitted:
                    return "label label-info";
                case PayrollExecutionStatusEnum.InProgress:
                    return "label label-warning";
                case PayrollExecutionStatusEnum.Completed:
                    return "label label-success";
                case PayrollExecutionStatusEnum.Error:
                    return "label label-danger";
                default:
                    return "label label-default";
            }
        }

        public static string TemplateMasterMapping(long userId, long companyId)
        {
            var date = DateTime.Now.Date.ToUTCFormat();
            return string.Concat(@"match(user:ADM_User{IsDeleted:0,Status:'Active',CompanyId:", companyId, @",Id:", userId, @"})
            optional match (adminT:NTS_TemplateMaster{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            where user.IsAdmin=true
            optional match(user)-[:R_User_Mapping_TemplateMaster]->
            (userT:NTS_TemplateMaster{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            optional match(user)-[:R_User_UserRole]->(role:ADM_UserRole{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            optional match(role)-[:R_UserRole_Mapping_TemplateMaster]
            ->(roleT:NTS_TemplateMaster{IsDeleted:0,Status:'Active',CompanyId:", companyId, @"})
            WITH collect(adminT.Id)+collect(userT.Id)+collect(roleT.Id) as templates unwind templates as AllowedTemplateMasterIds
            with distinct  AllowedTemplateMasterIds ");
        }
        public static string PersonDisplayName(string prefix, string alias)
        {
            return PersonFullName(prefix, alias);
        }
        public static string PersonDisplayNameWithSponsorshipNo(string prefix, string alias)
        {
            return string.Concat(prefix, ".FirstName + coalesce(\" \" + ", prefix, ".LastName,\"\") + coalesce(\"-\" + ", prefix, ".PersonNo,\"\") ", alias);
        }
        public static string PersonDisplayNameWithPersonNo(string prefix, string alias)
        {
            return string.Concat(prefix, ".FirstName + coalesce(\" \" + ", prefix, ".LastName,\"\") + coalesce(\"-\" + ", prefix, ".PersonNo,\"\") ", alias);
        }

        public static string PersonFullName(string prefix, string alias)
        {
            return string.Concat(prefix, ".FirstName + coalesce(\" \" + ", prefix, ".MiddleName,\"\") + coalesce(\" \" + ", prefix, ".LastName,\"\") ", alias);
        }
        public static string PersonFullNameWithSponsorshipNo(string prefix, string alias)
        {
            return string.Concat(prefix, ".FirstName + coalesce(\" \" + ", prefix, ".MiddleName,\"\") + coalesce(\" \" + ", prefix, ".LastName,\"\") + coalesce(\"-\" + ", prefix, ".PersonNo,\"\") ", alias);
        }
        public static string UserInfoOrPersonFullNameWithSponsorshipNo(string userInfoPrefix, string personPrefix, string alias)
        {
            return string.Concat(" coalesce(", PersonFullNameWithSponsorshipNo(personPrefix, ""), ",coalesce(case when ", userInfoPrefix, ".Name ='' then null else ", userInfoPrefix, ".Name end+'-','')+", userInfoPrefix, ".PersonNo", ") ", alias);
        }
        public static string LeadFullNameWithMobile(string prefix, string alias)
        {
            return string.Concat(prefix, ".FirstName + coalesce(\" \" + ", prefix, ".LastName,\"\") ", alias);
        }
        public static string PersonFullNameWithPersonNo(string prefix, string alias)
        {
            return string.Concat(prefix, ".FirstName + coalesce(\" \" + ", prefix, ".MiddleName,\"\") + coalesce(\" \" + ", prefix, ".LastName,\"\") + coalesce(\"-\" + ", prefix, ".PersonNo,\"\") ", alias);
        }

        public static string UserDisplayName(string userPrefix, string alias, string personPrefix = "")
        {
            return personPrefix == "" ? UserDisplayName(userPrefix, alias) : PersonDisplayName(personPrefix, alias);
        }

        public static string UserOrPersonDisplayName(string userPrefix, string personPrefix, string alias)
        {
            return string.Concat(" coalesce(", PersonFullName(personPrefix, ""), ",", userPrefix, ".UserName) ", alias);
        }
        public static string UserDisplayName(string userPrefix, string alias)
        {
            return UserNameWithEmail(userPrefix, alias);
        }
        public static string UserNameWithEmail(string prefix, string alias)
        {
            return string.Concat(prefix, ".UserName + coalesce(\"(\" + ", prefix, ".Email+\")\",\"\") ", alias);
        }

        public static string AssigneeNameWithEmail(string prefix, string alias)
        {
            return string.Concat(prefix, ".FirstName + coalesce(\" \" + ", prefix, ".MiddleName,\"\") + coalesce(\" \" + ", prefix, ".LastName,\"\") ", alias);
        }
        public static double GetAnnualLeaveEntitlement(string gradeName)
        {
            double entitlement = 21;
            if (gradeName == "G-CEO" || gradeName == "G-8" || gradeName == "G-9"
                || gradeName == "G-10" || gradeName == "G-11" || gradeName == "G-12")
            {
                entitlement = 30;
            }
            else if (gradeName == "G-5" || gradeName == "G-6" || gradeName == "G-7"
               || gradeName == "G-7" || gradeName == "G-8")
            {
                entitlement = 30;
            }
            return entitlement;
        }
        public static string GetFileType(string fileName)
        {

            var ext = "";
            if (fileName.IsNotNullAndNotEmpty())
            {
                var divs = fileName.Split('.').ToList();
                ext = divs.LastOrDefault();

                return ext;
            }


            return ext;
        }

        //public static Dictionary<string, object> SetReadOnlyWithClass<T>(string txtToCheckIn, Expression<Func<T>> propertyLambda, string className = "")
        //{
        //    var me = propertyLambda.Body as MemberExpression;

        //    if (me == null)
        //    {
        //        throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
        //    }
        //    var ret = new Dictionary<string, object>();
        //    if (className.IsNotNullAndEmpty())
        //    {
        //        ret.Add("class", className);
        //    }

        //    if (!txtToCheckIn.Contains(string.Concat(",", me.Member.Name, ",")))
        //    {
        //        ret.Add("readonly", "readonly");
        //    }

        //    return ret;
        //}

        public static string SetReadOnly<T>(string txtToCheckIn, Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }
            return txtToCheckIn.Contains(string.Concat(",", me.Member.Name, ",")) ? "" : "readonly";
        }

        public static string GetVirtualPath()
        {

            string path = HttpContext.Current.Request.RawUrl;
            path = path.Substring(0, path.IndexOf("?"));
            path = path.Substring(path.LastIndexOf("/") + 1);
            return path;
        }
        public static string GetUrlLocalpath()
        {
            return HttpContext.Current.Request.Url.LocalPath;
        }
        public static string GetQueryString()
        {
            return HttpContext.Current.Request.Url.Query;
        }
        public static string GetQueryString(string url)
        {
            int index = url.IndexOf("?") + 1;
            return url.Substring(index);
        }


        public static Dictionary<string, object> GenerateCypherParameterWithMandatoryValues(long companyId, long isDeleted = 0, StatusEnum status = StatusEnum.Active, params object[] param)
        {
            var prms = new Dictionary<string, object>
            {
                { "CompanyId", companyId },
                { "IsDeleted", isDeleted },
                { "Status", status.ToString() }
            };

            var p = param.ToList();
            if (param.Count() % 2 != 0)
            {
                throw new InvalidDataException("Invalid number of parameters");
            }
            for (int i = 0; i < p.Count(); i = i + 2)
            {
                prms.Add(Convert.ToString(p[i]), p[i + 1]);
            }
            return prms;
        }

        public static Dictionary<string, object> GenerateCypherParameter(params object[] param)
        {
            var prms = new Dictionary<string, object>();
            var p = param.ToList();
            if (param.Count() % 2 != 0)
            {
                throw new InvalidDataException("Invalid number of parameters");
            }
            for (int i = 0; i < p.Count(); i = i + 2)
            {
                prms.Add(Convert.ToString(p[i]), p[i + 1]);
            }
            return prms;
        }


        public static string EncryptedQueryString(string query)
        {
            return String.Concat("?", Constant.QueryStringEncryptionParam, EncryptQS(query));
        }

        public static string GenerateAbsoluteUrl(string url)
        {
            if (url.IsNotNullAndNotEmpty())
            {
                url = Regex.Replace(url, @"\t|\n|\r", "");
                return string.Concat(Constant.AppSettings.ApplicationBaseUrl, url);
            }
            return url;
        }

        public static string DecryptedQueryString(string query)
        {
            return DecryptQS(query.Replace(Constant.QueryStringEncryptionParam, string.Empty));
        }
        public static string AppVersion
        {
            get { return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion; }
        }
        public static string AppVersionWithParam
        {
            get { return String.Concat(Constant.QueryStringVersionParam, AppVersion); }
        }
        public static string AppName
        {
            get { return "ERP"; }
        }

        private readonly static byte[] Salt = Encoding.ASCII.GetBytes(Constant.EncryptionKey.Length.ToString());

        public static string Encrypt(string value)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(value);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Constant.EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    value = Convert.ToBase64String(ms.ToArray());
                }
            }
            return value;
        }
        public static string EncryptQS(string value)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(value);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Constant.EncryptionKey, new byte[] { 0x65, 0x76, 0x61, 0x6e, 0x20, 0x76, 0x65, 0x64, 0x76, 0x61, 0x64, 0x65, 0x20 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    value = Convert.ToBase64String(ms.ToArray());
                }
            }
            return value;
        }
        public static string DecryptQS(string cipherText, bool useHashing = true)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Constant.EncryptionKey, new byte[] { 0x65, 0x76, 0x61, 0x6e, 0x20, 0x76, 0x65, 0x64, 0x76, 0x61, 0x64, 0x65, 0x20 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string Decrypt(string cipherText, bool useHashing = true)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Constant.EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static byte[] ResizeImage(byte[] bytes, int width, int height)
        {

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            var image = byteArrayToImage(bytes);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return imageToByteArray(destImage);
        }


        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public static int ConvertPixelToPoints(int pixel)
        {
            return Convert.ToInt32((pixel * 72.00) / 120.00);
        }
        public static string GetIPAddress()
        {
            try
            {
                return System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            catch (Exception)
            {

                return string.Empty;
            }


        }

        public static string DynamicValueBind(string text, dynamic dynamicModel)
        {
            try
            {
                if (text.IsNotNullAndNotEmpty())
                {
                    Velocity.Init();
                    var velocityContext = new VelocityContext();
                    velocityContext.Put("model", dynamicModel);
                    string template = string.Join(Environment.NewLine, new[] { text });
                    var sb = new StringBuilder();
                    Velocity.Evaluate(velocityContext, new StringWriter(sb), "Template", new StringReader(template));
                    return sb.ToString();
                }
                return text;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string GetDisplayName(dynamic obj, string propertyName)
        {
            var display = obj.GetType().GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true);

            if (display != null && display.Length > 0)
            {
                return display[0].Name;
            }
            else
            {
                return propertyName;
            }

        }
        public static ModuleEnum? GetModuleName(string url)
        {
            if (url.Contains(string.Concat("/", ModuleEnum.Admin.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Admin;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.General.ToString().ToLower(), "/")))
            {
                return ModuleEnum.General;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Hrs.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Hrs;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Leave.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Leave;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Nts.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Nts;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Pay.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Pay;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Pms.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Pms;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Pmt.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Pmt;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Sal.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Sal;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Sps.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Sps;
            }
            else if (url.Contains(string.Concat("/", ModuleEnum.Taa.ToString().ToLower(), "/")))
            {
                return ModuleEnum.Taa;
            }

            return ModuleEnum.General;
        }

        public static void RunCommand(string command, int timeoutSeconds)
        {
            try
            {
                using (var cmd = new System.Diagnostics.Process())
                {

                    var process = new ProcessStartInfo();
                    process.UseShellExecute = false;
                    process.FileName = "cmd.exe";
                    process.Arguments = command;
                    process.WindowStyle = ProcessWindowStyle.Hidden;
                    process.CreateNoWindow = true;
                    //process.RedirectStandardInput = true;
                    cmd.StartInfo = process;
                    cmd.Start();

                    //cmd.StandardInput.WriteLine(command);
                    //cmd.StandardInput.Flush();
                    //cmd.StandardInput.Close();
                    // var result = cmd.StandardOutput.ReadToEnd();
                    cmd.WaitForExit();



                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw;
            }
        }

        public static void RunCommandWithShellExecute(string command, int timeoutSeconds)
        {
            try
            {
                Log.Instance.Info(command);
                using (var cmd = new System.Diagnostics.Process())
                {

                    var process = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        WorkingDirectory = @"C:\Windows\System32",
                        FileName = @"C:\Windows\System32\cmd.exe",
                        Arguments = command,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = false
                    };
                    cmd.StartInfo = process;
                    cmd.Start();
                    cmd.WaitForExit();




                    //process.StartInfo.RedirectStandardOutput = true;
                    //process.StartInfo.RedirectStandardError = true;

                    //process.StartInfo.UseShellExecute = false;
                    //process.StartInfo.CreateNoWindow = true;
                    //process.StartInfo.FileName = "cmd.exe";
                    //process.StartInfo.Arguments = command;
                    //process.OutputDataReceived += Process_OutputDataReceived;
                    //process.ErrorDataReceived += Process_ErrorDataReceived;

                    //process.Start();

                    //process.BeginOutputReadLine();
                    //process.BeginErrorReadLine();

                    //process.WaitForExit();

                    // Log.Instance.Info(output);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }
        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string ByteSizeWithSuffix(Int64? value = 0, int decimalPlaces = 1)
        {
            if (value == null)
            {
                value = 0;
            }
            if (value < 0) { return "-" + ByteSizeWithSuffix(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
        public static bool IsValidDateTime(string dateTime)
        {
            string[] formats = { "yyyy-mm-dd" };
            DateTime parsedDateTime;
            return DateTime.TryParseExact(dateTime, formats, new CultureInfo("en-US"),
                                           DateTimeStyles.None, out parsedDateTime);
        }
        public static string GenerateRandomOTP(int iOTPLength)

        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in characters, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        public static string WordWrap(string text, int width)
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(Environment.NewLine, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }
            return sb.ToString();
        }


        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
    }


}
