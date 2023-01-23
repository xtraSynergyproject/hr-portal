using Microsoft.Extensions.Configuration;
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
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Synergy.App.Common
{
    public class Helper
    {
        public static Dictionary<string, string> QueryStringToDictionary(string value)
        {
            var dict = new Dictionary<string, string>();
            if (value.IsNullOrEmpty())
            {
                return dict;
            }
            var items = value.Split('&');
            foreach (var item in items)
            {
                var splt = item.Split('=');
                if (splt.Length == 2)
                {
                    dict.Add(splt[0], splt[1]);
                }
                else if (splt.Length > 2)
                {
                    var str = "";
                    var i = 0;
                    foreach (var item1 in splt)
                    {
                        if (i > 0)
                        {
                            str += item1 + "=";
                        }
                        i++;
                    }
                    str = str.TrimEnd('=');
                    dict.Add(splt[0], str);
                }
            }
            return dict;

        }

        public static Dictionary<string, bool> QueryStringToBooleanDictionary(string value)
        {
            var dict = new Dictionary<string, bool>();
            if (value.IsNullOrEmpty())
            {
                return dict;
            }
            var items = value.Split('&');
            foreach (var item in items)
            {
                var splt = item.Split('=');
                if (splt.Length == 2)
                {
                    dict.Add(splt[0], Convert.ToBoolean(splt[1]));
                }
                else if (splt.Length > 2)
                {
                    var str = "";
                    var i = 0;
                    foreach (var item1 in splt)
                    {
                        if (i > 0)
                        {
                            str += item1 + "=";
                        }
                        i++;
                    }
                    str = str.TrimEnd('=');
                    dict.Add(splt[0], Convert.ToBoolean(str));
                }
            }
            return dict;

        }
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

        public static string GetViewableUserColumns
        {
            get
            {
                return "'Name','Email','JobTitle','PhotoId','Mobile'";
            }
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
        public static string GetLanguageCode(string languageName)
        {
            switch (languageName)
            {
                case ApplicationConstant.Language.English:
                    return ApplicationConstant.LanguageCode.English;
                case ApplicationConstant.Language.Arabic:
                    return ApplicationConstant.LanguageCode.Arabic;
                case ApplicationConstant.Language.Hindi:
                    return ApplicationConstant.LanguageCode.Hindi;
                default:
                    return ApplicationConstant.LanguageCode.English;
            }
        }
        public static string GetLanguage(string languageCode)
        {
            switch (languageCode)
            {
                case ApplicationConstant.LanguageCode.English:
                    return ApplicationConstant.Language.English;
                case ApplicationConstant.LanguageCode.Arabic:
                    return ApplicationConstant.Language.Arabic;
                case ApplicationConstant.LanguageCode.Hindi:
                    return ApplicationConstant.Language.Hindi;
                default:
                    return ApplicationConstant.Language.English;
            }
        }
        public static string GenerateAbsoluteUrl(string url, IConfiguration config)
        {
            if (url.IsNotNullAndNotEmpty())
            {
                url = Regex.Replace(url, @"\t|\n|\r", "");
                return string.Concat(ApplicationConstant.AppSettings.ApplicationBaseUrl(config), url);
            }
            return url;
        }
        public static string DynamicValueBind(string text, dynamic notification, dynamic model = null, dynamic recepient = null, dynamic sender = null, dynamic udf = null)
        {
            try
            {
                if (text.IsNotNullAndNotEmpty())
                {
                    Velocity.Init();
                    var velocityContext = new VelocityContext();
                    velocityContext.Put("notification", notification);
                    velocityContext.Put("model", model);
                    velocityContext.Put("recepient", recepient);
                    velocityContext.Put("sender", sender);
                    velocityContext.Put("udf", udf);
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
        public static string DynamicValueBind(string text, dynamic model)
        {
            try
            {
                if (text.IsNotNullAndNotEmpty())
                {
                    Velocity.Init();
                    var velocityContext = new VelocityContext();
                    velocityContext.Put("model", model);
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
        public static T ExecuteBreLogic<T>(string text, dynamic inputData, dynamic masterData)
        {
            try
            {
                if (text.IsNotNullAndNotEmpty())
                {
                    Velocity.Init();
                    var velocityContext = new VelocityContext();

                    velocityContext.Put("Input", inputData);
                    velocityContext.Put("Master", masterData);
                    string template = string.Join(Environment.NewLine, new[] { text });
                    var sb = new StringBuilder();
                    Velocity.Evaluate(velocityContext, new StringWriter(sb), null, new StringReader(template));
                    return (T)Convert.ChangeType(Convert.ToString(sb), typeof(T));
                }
                return default(T);
            }
            catch (Exception)
            {
                throw;
            }
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

        public static bool IsVideoFile(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",.mp4,.mpeg,.wmv,".Contains(ext);
        }
        public static bool IsImageFile(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",.jpg,.jpeg,.png,.gif,.bmp,".Contains(ext);
        }
        public static bool IsPdfFile(string ext)
        {
            return ext.ToLower() == ".pdf";
        }

        public static bool IsPptFile(string ext)
        {
            return ext.ToLower().Contains(".ppt");
        }

        public static bool IsSfdtFile(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return (",.config,.crt,.sql,.js,.css,.dotx,.docm,.dotm,.dot,.rtf,.txt,.xml,.doc,.docx,,").Contains(ext.ToLower());
        }
        public static bool IsExcelFile(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return (",.xls,.csv,.xlsx,.xlsm,").Contains(ext.ToLower());
        }
        public static bool IsAudioFile(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",.mp3,".Contains(ext);
        }
        public static bool Is2JpegSupportable(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",jpeg,jpg,tiff,tif,gif,png,bmp,pdf,xps,ico,wbm,psd,psp,html,htm,txt,rtf,doc,docx,xls,xlsx,xlsm,ppt,pptx,pps,ppsx,vsd,vsdx,cdr,".Contains(ext);
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






        public static string AppVersion
        {
            get { return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion; }
        }

        public static string AppName
        {
            get { return "ERP"; }
        }
        public static string EncryptJavascriptAesCypher(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            //  var encrypted = Convert.FromBase64String(text);
            var decriptedFromJavascript = EncryptTextToAesBytes(text, keybytes, iv);
            return /*string.Format(decriptedFromJavascript)*/ Convert.ToBase64String(decriptedFromJavascript);
        }
        public static string DecryptJavascriptAesCypher(string cipherText)
        {
            if (cipherText.IsNullOrEmpty())
            {
                return cipherText;
            }
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptAesCypherFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        public static string DecryptAesCypherFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        public static byte[] EncryptTextToAesBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }
        public static string Encrypt(string value)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(value);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ApplicationConstant.EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
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
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ApplicationConstant.EncryptionKey, new byte[] { 0x65, 0x76, 0x61, 0x6e, 0x20, 0x76, 0x65, 0x64, 0x76, 0x61, 0x64, 0x65, 0x20 });
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
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ApplicationConstant.EncryptionKey, new byte[] { 0x65, 0x76, 0x61, 0x6e, 0x20, 0x76, 0x65, 0x64, 0x76, 0x61, 0x64, 0x65, 0x20 });
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
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ApplicationConstant.EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
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
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void RunCommandWithShellExecute(string command, int timeoutSeconds)
        {
            try
            {
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
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
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
        public static bool CompareStringArray(string[] array1, string[] array2)
        {
            if (array1 == null && array2 == null)
            {
                return true;
            }
            else if (array1 != null && array2 == null)
            {
                return false;
            }
            else if (array1 == null && array2 != null)
            {
                return false;
            }
            else
            {
                return Enumerable.SequenceEqual(array1, array2);
            }
        }
        public static string OrganizationMapping(string userId, string companyId, string legalEntityId)
        {
            var query = $@"with ""Department"" as(
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""User"" as u on  u.""IsSystemAdmin""=true
            where u.""Id""='{userId}' and d.""IsDeleted""=false
            --and d.""LegalEntityId""='{legalEntityId}'
            union
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""UserEntityPermission"" as uep on d.""Id""=uep.""EntityModelId"" and uep.""EntityModelType""=1
            join public.""User"" as u on u.""Id""=uep.""UserEntityId"" and uep.""UserEntityType""=1
            where u.""Id""='{userId}' and d.""IsDeleted""=false --and d.""LegalEntityId""='{legalEntityId}'
            union
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""LegalEntity"" as le on d.""DeptLegalEntityId""=le.""Id"" 
            join public.""UserEntityPermission"" as uep on le.""Id""=uep.""EntityModelId"" and uep.""EntityModelType""=6
            join public.""User"" as u on u.""Id""=uep.""UserEntityId"" and uep.""UserEntityType""=1
            where u.""Id""='{userId}' and d.""IsDeleted""=false --and d.""LegalEntityId""='{legalEntityId}'
            union
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""UserEntityPermission"" as uep on d.""Id""=uep.""EntityModelId"" and uep.""EntityModelType""=1
            join public.""UserRole"" as ur on ur.""Id""=uep.""UserEntityId"" and uep.""UserEntityType""=2
            join public.""UserRoleUser"" as uru on uru.""UserRoleId""=ur.""Id"" 
            join public.""User"" as u on u.""Id""=uru.""UserId""
            where u.""Id""='{userId}' and d.""IsDeleted""=false --and d.""LegalEntityId""='{legalEntityId}'
            union
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""LegalEntity"" as le on d.""DeptLegalEntityId""=le.""Id"" 
            join public.""UserEntityPermission"" as uep on le.""Id""=uep.""EntityModelId"" and uep.""EntityModelType""=6
            join public.""UserRole"" as ur on ur.""Id""=uep.""UserEntityId"" and uep.""UserEntityType""=2
            join public.""UserRoleUser"" as uru on uru.""UserRoleId""=ur.""Id"" 
            join public.""User"" as u on u.""Id""=uru.""UserId""
            where u.""Id""='{userId}' and d.""IsDeleted""=false --and d.""LegalEntityId""='{legalEntityId}'
            )";
            return query;

        }
        public static string TemplateMapping(string userId, string companyId, string legalEntityId)
        {
            var query = $@"with ""Template"" as(
            select d.""Id"" as ""TemplateId"" FROM public.""Template"" as d
            join public.""User"" as u on  u.""IsSystemAdmin""=true
            where u.""Id""='{userId}' --and d.""LegalEntityId""='{legalEntityId}'
            union
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""UserEntityPermission"" as uep on d.""Id""=uep.""EntityModelId"" and uep.""EntityModelType""=1
            join public.""User"" as u on u.""Id""=uep.""UserEntityId"" and uep.""UserEntityType""=1
            where u.""Id""='{userId}' --and d.""LegalEntityId""='{legalEntityId}'
            union
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""LegalEntity"" as le on d.""DeptLegalEntityId""=le.""Id"" 
            join public.""UserEntityPermission"" as uep on le.""Id""=uep.""EntityModelId"" and uep.""EntityModelType""=6
            join public.""User"" as u on u.""Id""=uep.""UserEntityId"" and uep.""UserEntityType""=1
            where u.""Id""='{userId}' --and d.""LegalEntityId""='{legalEntityId}'
            union
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""UserEntityPermission"" as uep on d.""Id""=uep.""EntityModelId"" and uep.""EntityModelType""=1
            join public.""UserRole"" as ur on ur.""Id""=uep.""UserEntityId"" and uep.""UserEntityType""=2
            join public.""UserRoleUser"" as uru on uru.""UserRoleId""=ur.""Id"" 
            join public.""User"" as u on u.""Id""=uru.""UserId""
            where u.""Id""='{userId}' --and d.""LegalEntityId""='{legalEntityId}'
            union
            select d.""Id"" as ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartment"" as d
            join public.""LegalEntity"" as le on d.""DeptLegalEntityId""=le.""Id"" 
            join public.""UserEntityPermission"" as uep on le.""Id""=uep.""EntityModelId"" and uep.""EntityModelType""=6
            join public.""UserRole"" as ur on ur.""Id""=uep.""UserEntityId"" and uep.""UserEntityType""=2
            join public.""UserRoleUser"" as uru on uru.""UserRoleId""=ur.""Id"" 
            join public.""User"" as u on u.""Id""=uru.""UserId""
            where u.""Id""='{userId}' --and d.""LegalEntityId""='{legalEntityId}'
            )";
            return query;

        }

        public static string GetAnnualLeaveTemplateCode(string legalEntityCode)
        {
            return "ANNUAL_LEAVE";
        }
        public static string GetPayrollExecutionStatusCss(PayrollExecutionStatusEnum? stauts)
        {
            switch (stauts)
            {
                case PayrollExecutionStatusEnum.NotStarted:
                    return "badge badge-default";
                case PayrollExecutionStatusEnum.Submitted:
                    return "badge badge-info";
                case PayrollExecutionStatusEnum.InProgress:
                    return "badge badge-warning";
                case PayrollExecutionStatusEnum.Completed:
                    return "badge badge-success";
                case PayrollExecutionStatusEnum.Error:
                    return "badge badge-danger";
                default:
                    return "label label-default";
            }
        }
        public async static Task<string> HttpRequest(string url, HttpVerb verb, string dataJson, bool disableSsl = false)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                if (disableSsl)
                {
                    httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                }

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    var address = new Uri(url);
                    try
                    {
                        switch (verb)
                        {
                            case HttpVerb.Get:
                                var getResponse = await httpClient.GetStringAsync(address);
                                return getResponse;
                            case HttpVerb.Post:
                                var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
                                var postResponse = await httpClient.PostAsync(address, stringContent);
                                var postResponseString = await postResponse.Content.ReadAsStringAsync();
                                return postResponseString;
                            default:
                                var getResponse2 = await httpClient.GetStringAsync(address);
                                return getResponse2;
                        }
                    }
                    catch (Exception e)
                    {

                        throw;
                    }

                }
            }

        }

        public static Tuple<DateTime, DateTime> GetStartAndEndDateByEnum(InventoryDataFilterEnum filter)
        {
            if (filter == InventoryDataFilterEnum.Today)
            {
                return new Tuple<DateTime, DateTime>(DateTime.Today,
                    DateTime.Today.Date.AddDays(1).AddTicks(-1));
            }
            else if (filter == InventoryDataFilterEnum.Yesterday)
            {
                return new Tuple<DateTime, DateTime>(DateTime.Today.AddDays(-1),
                    DateTime.Today.Date.AddTicks(-1));
            }
            else if (filter == InventoryDataFilterEnum.ThisWeek)
            {
                var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                var diff = DateTime.Today.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek - 1;

                if (diff < 0)
                {
                    diff += 7;
                }

                var firstDayOfTheWeek = DateTime.Today.AddDays(-diff).Date;
                var lastDayOfTheWeek = firstDayOfTheWeek.AddDays(7).AddTicks(-1);
                return new Tuple<DateTime, DateTime>(firstDayOfTheWeek, lastDayOfTheWeek);
            }
            else if (filter == InventoryDataFilterEnum.PreviousWeek)
            {
                var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                var diff = DateTime.Today.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

                if (diff < 0)
                {
                    diff += 7;
                }

                var firstDayOfTheWeek = DateTime.Today.AddDays(-diff).Date.AddDays(-6);
                var lastDayOfTheWeek = firstDayOfTheWeek.AddDays(7).AddTicks(-1);
                return new Tuple<DateTime, DateTime>(firstDayOfTheWeek, lastDayOfTheWeek);

            }
            else if (filter == InventoryDataFilterEnum.ThisMonth)
            {
                var monthFirstDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var monthLastDate = monthFirstDate.AddMonths(1).AddTicks(-1);

                return new Tuple<DateTime, DateTime>(monthFirstDate, monthLastDate);
            }
            else if (filter == InventoryDataFilterEnum.PreviousMonth)
            {
                var monthFirstDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1);
                var monthLastDate = monthFirstDate.AddMonths(1).AddTicks(-1);

                return new Tuple<DateTime, DateTime>(monthFirstDate, monthLastDate);
            }
            else if (filter == InventoryDataFilterEnum.ThisQuarter)
            {
                DateTime datetime = DateTime.Now;
                int currQuarter = (datetime.Month - 1) / 3 + 1;
                DateTime dtFirstDay = new DateTime(datetime.Year, 3 * currQuarter - 2, 1);
                DateTime dtLastDay = new DateTime(datetime.Year, 3 * currQuarter, 1).AddMonths(1).AddTicks(-1);
                return new Tuple<DateTime, DateTime>(dtFirstDay, dtLastDay);
            }
            else if (filter == InventoryDataFilterEnum.PreviousQuarter)
            {
                DateTime datetime = DateTime.Now;
                int currQuarter = (datetime.Month - 1) / 3 + 1;
                DateTime dtFirstDay = new DateTime(datetime.Year, 3 * currQuarter - 2, 1).AddMonths(-3);
                DateTime dtLastDay = new DateTime(datetime.Year, 3 * currQuarter, 1).AddMonths(1).AddTicks(-1).AddMonths(-3);
                return new Tuple<DateTime, DateTime>(dtFirstDay, dtLastDay);
            }
            else if (filter == InventoryDataFilterEnum.ThisYear)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new(year, 1, 1);
                DateTime lastDay = new(year, 12, 31);
                return new Tuple<DateTime, DateTime>(firstDay, lastDay.AddDays(1).AddTicks(-1));
            }
            else if (filter == InventoryDataFilterEnum.PreviousYear)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new(year - 1, 1, 1);
                DateTime lastDay = new(year - 1, 12, 31);
                return new Tuple<DateTime, DateTime>(firstDay, lastDay.AddDays(1).AddTicks(-1));
            }
            return new Tuple<DateTime, DateTime>(DateTime.Today, DateTime.Today);
        }
        public static string ReplaceJsonProperty(string json, string replacePropertyName, string newPropertyAndValue = "")
        {
            var patttern = @$"\s*\""{replacePropertyName}\"" *: *\"".*\""(,|(?=\s+\}}))";
            return Regex.Replace(json, patttern, newPropertyAndValue);
        }
    }
}
