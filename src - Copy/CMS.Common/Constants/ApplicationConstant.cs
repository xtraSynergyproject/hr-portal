using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common
{

    public static class ApplicationConstant
    {

        public const string AppName = "SYNERGY CMS";
        public const string WindowsServiceUserId = "45bba746-3309-49b7-9c03-b5793369d73c";
        public static class Messages
        {
            public const string DeleteConfirmation = "Are you sure you want to delete the selected item? You cannot undo this action if you proceed. Please confirm";
        }
        public static class DateAndTime
        {
            public static DateTime MinDate = new DateTime(1900, 1, 1);
            public static DateTime MaxDate = new DateTime(9999, 12, 31);
            public const string DefaultJqueryDateFormat = "dd.MM.yyyy";
            public const string DefaultJqueryDateFormatOnly = "yyyy-MM-dd";
            public const string DefaultJqueryDateFormatForMoment = "DD.MM.YYYY";
            public const string DefaultJqueryDateTimeFormat = "dd.MM.yyyy HH:mm";
            public const string DefaultDateFormat = "{0:dd.MM.yyyy}";
            public const string DefaultDateTimeFormatOnly = "dd MMM yyyy HH:mm";
            public const string SequenceNumberFormat = "{0:dd.MM.yyyy}";
            public const string DefaultDateTimeFormat = "{0:dd.MM.yyyy HH:mm}";
            public const string YYYY_MM_DD = "{0:yyyy/MM/dd}";
            public const string YYYY_MM_DD_HH_MM = "{0:yyyy/MM/dd HH:mm}";
            public const string LongDateTimeFormat = "{0:dd MMM yyyy HH:mm:ss}";
        }
        public static class Database
        {
            public const string TableSpace = "pg_default";
            public const string DefaultCollation = "cms_collation_ci";
            public const string DateFormat = "{0:yyyy-MM-dd HH:mm:ss.ff}";
            public const string JQueryDateFormat = "yyyy-MM-dd HH:mm:ss";
            public static class Schema
            {
                public const string Dbo = "dbo";
                public const string Cms = "cms";
                public const string _Public = "public";
                public const string Log = "log";

            }
            public static class Owner
            {
                public const string Postgres = "postgres";
            }
        }


        public static class Roles
        {
            public const string User = "User";
            public const string Admin = "Admin";
            public const string SystemAdmin = "SystemAdmin";
            public const string All = "All";
        }
        public static class Session
        {
            public const string UserId = "UserId";
            public const string UserName = "UserName";
            public const string JobTitle = "JobTitle";
            public const string UserEmail = "UserEmail";
            public const string CompanyId = "CompanyId";
        }
        public class AppSettings
        {
            public static string UploadPath(IConfiguration config)
            {
                return $"{Convert.ToString(config["UploadPath"]).TrimEnd('/')}/";

            }
            public static string ApplicationBaseUrl(IConfiguration config)
            {
                return $"{Convert.ToString(config["ApplicationBaseUrl"]).TrimEnd('/')}/";

            }
            public static string EsdbConnectionUrl(IConfiguration config)
            {
                return $"{Convert.ToString(config["EsdbConnectionUrl"]).TrimEnd('/')}/";

            }
            public static string LogstashConfigPath(IConfiguration config)
            {
                return $"{Convert.ToString(config["LogstashConfigPath"])}";

            }
            public static string WebApiUrl(IConfiguration config)
            {
                return $"{Convert.ToString(config["WebApiUrl"]).TrimEnd('/')}/";

            }
        }
        public class Language
        {
            public const string English = "English";
            public const string Arabic = "Arabic";
        }
        public class LanguageCode
        {
            public const string English = "en-US";
            public const string Arabic = "ar-SA";
        }

        public static string PlaceHolder_SelectOption = "--Select--";
        public static string PlaceHolder_AllOption = "--All--";
        public static string EncryptionKey = "d5Pc2gsc91sZ5AJG3mSHqFWi";

        public const int ApplicationStartYear = 2019;
        public const double MaxFileSizeAllowed = 2147483648;
        public class BusinessAnalytics
        {
            public const string ReadTwitterDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadTwitterDataQuery2 = @"{""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""hashtags"", ""text"", ""location"" ],""query"":""#SEARCHWHERE#""}}}},""highlight"":{""fields"":{""hashtags"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""text"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""location"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string ReadTwitterDataQuery3 = @"{""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""hashtags"", ""text"", ""location"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},""highlight"":{""fields"":{""hashtags"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""text"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""location"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string ReadTwitterDataQuery4 = @"
                        {""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""hashtags"", ""text"", ""location""],
                                ""default_operator"": ""and""
                            }
                          },
                    ""highlight"":{""fields"":{""hashtags"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""text"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""location"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";


             public const string ReadFacebookDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
             public const string ReadFacebookDataQuery2 = @"{""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""pagename"", ""post_message"" ],""query"":""#SEARCHWHERE#""}}}},""highlight"":{""fields"":{""pagename"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""post_message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
             public const string ReadFacebookDataQuery3 = @"{""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""pagename"", ""post_message"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},""highlight"":{""fields"":{""pagename"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""post_message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
             public const string ReadFacebookDataQuery4 = @"{""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""post_message"", ""pagename""],
                                ""default_operator"": ""and""
                            }
                          },
                        ""highlight"":{""fields"":{""pagename"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""post_message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";


            public const string ReadInstagramDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadInstagramDataQuery2 = @"{
                    ""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""hashtags"", ""caption"" ],""query"":""#SEARCHWHERE#""}}}},
                    ""highlight"":{""fields"":{""hashtags"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""caption"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
                                 
            public const string ReadInstagramDataQuery3 = @"{
                    ""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""hashtags"", ""caption"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},
                    ""highlight"":{""fields"":{""hashtags"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""caption"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string ReadInstagramDataQuery4 = @"{
                    ""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""hashtags"", ""caption""],
                                ""default_operator"": ""and""
                            }
                          },
                    ""highlight"":{""fields"":{""hashtags"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""caption"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";

            public const string ReadYoutubeDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadYoutubeDataQuery2 = @"{
""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""title"", ""description"" ],""query"":""#SEARCHWHERE#""}}}},
""highlight"":{""fields"":{""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""description"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string ReadYoutubeDataQuery3 = @"{
""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""title"", ""description"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},
""highlight"":{""fields"":{""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""description"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string ReadYoutubeDataQuery4 = @"{
                    ""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""title"", ""description""],
                                ""default_operator"": ""and""
                            }
                          },
                    ""highlight"":{""fields"":{""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""description"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";

            public const string ReadWhatsappDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadWhatsappDataQuery2 = @"{""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""user"", ""messages"" ],""query"":""#SEARCHWHERE#""}}}},""highlight"":{""fields"":{""user"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""messages"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string ReadWhatsappDataQuery3 = @"{""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""user"", ""messages"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},""highlight"":{""fields"":{""user"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""messages"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string ReadWhatsappDataQuery4 = @"{""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""user"", ""messages""],
                                ""default_operator"": ""and""
                            }
                          },
						  ""highlight"":{""fields"":{""user"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""messages"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";


            public const string ReadTwitterAdvanceDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadTwitterAdvanceDataQuery2 = @"{""match"":{""text"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}}";
            public const string ReadTwitterAdvanceDataQuery3 = @"{""match"":{""text"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}},";
            public const string ReadTwitterAdvanceDataQuery4 = @"{""query"":{""bool"":{""should"":[#SEARCHWHERE#]}},""highlight"":{""fields"":{""text"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";

            

            public const string ReadFacebookAdvanceDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadFacebookAdvanceDataQuery2 = @"{""match"":{""post_message"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}}";
            public const string ReadFacebookAdvanceDataQuery3 = @"{""match"":{""post_message"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}},";
            public const string ReadFacebookAdvanceDataQuery4 = @"{""query"":{""bool"":{""should"":[#SEARCHWHERE#]}},""highlight"":{""fields"":{""post_message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";

            

            public const string ReadInstagramAdvanceDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadInstagramAdvanceDataQuery2 = @"{""match"":{""image_links"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}}";
            public const string ReadInstagramAdvanceDataQuery3 = @"{""match"":{""image_links"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}},";
            public const string ReadInstagramAdvanceDataQuery4 = @"{""query"":{""bool"":{""should"":[#SEARCHWHERE#]}},""highlight"":{""fields"":{""image_links"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";


            public const string ReadYoutubeAdvanceDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadYoutubeAdvanceDataQuery2 = @"{""match"":{""description"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}}";
            public const string ReadYoutubeAdvanceDataQuery3 = @"{""match"":{""description"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}},";
            public const string ReadYoutubeAdvanceDataQuery4 = @"{""query"":{""bool"":{""should"":[#SEARCHWHERE#]}},""highlight"":{""fields"":{""description"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";

            public const string ReadWhatsappAdvanceDataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadWhatsappAdvanceDataQuery2 = @"{""match"":{""messages"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}}";
            public const string ReadWhatsappAdvanceDataQuery3 = @"{""match"":{""messages"": {""query"": ""#SEARCHWHERE#"",""operator"": ""#OPERATORWHERE#""}}},";
            public const string ReadWhatsappAdvanceDataQuery4 = @"{""query"":{""bool"":{""should"":[#SEARCHWHERE#]}},""highlight"":{""fields"":{""messages"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";

            public const string ReadDial100DataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadDial100DataQuery2 = @"{""size"":100 ,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""unityp"", ""unit_status"", ""unique_id"", ""track_personnel"", ""tycod"", ""sub_tycod""
, ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"", ""ag_id"" ],""query"":""#SEARCHWHERE#""}}}},""highlight"":{""fields"":{
""unityp"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unit_status"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unique_id"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""track_personnel"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""tycod"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""sub_tycod"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""station"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""location"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""eid"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""dgroup"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ag_id"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";
            public const string ReadDial100DataQuery3 = @"{""size"":100 ,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""unityp"", ""unit_status"", ""unique_id"", ""track_personnel"", ""tycod"", ""sub_tycod""
, ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"", ""ag_id"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},""highlight"":{""fields"":{
""unityp"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unit_status"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unique_id"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""track_personnel"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""tycod"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""sub_tycod"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""station"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""location"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""eid"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""dgroup"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ag_id"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";
            public const string ReadDial100DataQuery4 = @"{""size"":100 ,""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""unityp"", ""unit_status"", ""unique_id"", ""track_personnel"", ""tycod"", ""sub_tycod""
, ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"", ""ag_id""],
                                ""default_operator"": ""and""
                            }
                          },
""highlight"":{""fields"":{
""unityp"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unit_status"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unique_id"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""track_personnel"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""tycod"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""sub_tycod"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""station"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""location"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""eid"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""dgroup"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ag_id"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";

            public const string ReadCCTVCamera1DataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadCCTVCamera1DataQuery2 = @"{""size"":100 ,""query"":{""bool"":{""must"":[{""multi_match"":{""fields"":[ ""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
, ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"" ],""query"":""#SEARCHWHERE#""}}],{""range"": {""@timestamp"": {
                            ""gte"":""07/07/2021"",   
                            ""lte"":""08/07/2021"",
                            ""format"": ""yyyy-MM-dd HH:mm:ss.fffffffK""
            }
                    }
                }]}
        },""highlight"":{""fields"":{
""srNo"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""cameraName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""locationName "":{ ""pre_tags"" : ["" < em > ""], ""post_tags"" : ["" </ em > ""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""policeStation "":{ ""pre_tags"" : ["" < em > ""], ""post_tags"" : ["" </ em > ""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude "":{ ""pre_tags"" : ["" < em > ""], ""post_tags"" : ["" </ em > ""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude "":{ ""pre_tags"" : ["" < em > ""], ""post_tags"" : ["" </ em > ""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ipAddress "":{ ""pre_tags"" : ["" < em > ""], ""post_tags"" : ["" </ em > ""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""rtspLink "":{ ""pre_tags"" : ["" < em > ""], ""post_tags"" : ["" </ em > ""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""typeOfCamera "":{ ""pre_tags"" : ["" < em > ""], ""post_tags"" : ["" </ em > ""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""make "":{ ""pre_tags"" : ["" < em > ""], ""post_tags"" : ["" </ em > ""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";
            public const string ReadCCTVCamera1DataQuery3 = @"{""size"":100 ,""query"":{""bool"":{""must"":[{""multi_match"":{""fields"":[ ""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
, ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"" ],""query"":""#SEARCHWHERE#""}}]}},""highlight"":{""fields"":{
""srNo"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""cameraName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""locationName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""policeStation"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ipAddress"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""rtspLink"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""typeOfCamera"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""make"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";
            public const string ReadCCTVCamera1DataQuery4 = @"{""size"":100 ,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
, ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},""highlight"":{""fields"":{
""srNo"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""cameraName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""locationName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""policeStation"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ipAddress"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""rtspLink"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""typeOfCamera"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""make"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";
            public const string ReadCCTVCamera1DataQuery5 = @"{""size"":100 ,""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
, ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"" ],
                                ""default_operator"": ""and""
                            }
                          },
						  
""highlight"":{""fields"":{
""srNo"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""cameraName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""locationName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""policeStation"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ipAddress"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""rtspLink"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""typeOfCamera"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""make"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";


            public const string ReadCCTVCamera2DataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadCCTVCamera2DataQuery2 = @"{""size"":100 ,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
, ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"" ],""query"":""#SEARCHWHERE#""}}}},""highlight"":{""fields"":{
""srNo"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""cameraName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""locationName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""policeStation"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ipAddress"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""rtspLink"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""typeOfCamera"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""make"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";
            public const string ReadCCTVCamera2DataQuery3 = @"{""size"":100 ,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
, ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},""highlight"":{""fields"":{
""srNo"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""cameraName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""locationName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""policeStation"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ipAddress"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""rtspLink"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""typeOfCamera"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""make"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";
            public const string ReadCCTVCamera2DataQuery4 = @"{""size"":100 ,""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
, ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"" ],
                                ""default_operator"": ""and""
                            }
                          },
						  
""highlight"":{""fields"":{
""srNo"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""cameraName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""locationName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""policeStation"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ipAddress"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""rtspLink"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""typeOfCamera"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""make"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";



            public const string ReadCamera1DataQuery1 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";
            public const string ReadCamera1DataQuery2 = @"{""size"":1000 ,""query"": {""match_all"": { } } }";






            public const string GetSocialMediaChartDataQuery1 = @"{""query"": {
                        ""simple_query_string"" : {
                            ""query"": ""#SEARCHWHERE#"",
                            ""fields"": [""post_message"", ""pagename""],
                            ""default_operator"": ""and""
                        }
                        },
                    ""highlight"":{""fields"":{""pagename"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""post_message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string GetSocialMediaChartDataQuery2 = @"
                        {""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""hashtags"", ""text"", ""location""],
                                ""default_operator"": ""and""
                            }
                          },
                    ""highlight"":{""fields"":{""hashtags"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""text"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""location"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string GetSocialMediaChartDataQuery3 = @"{
                        ""query"": {
                                ""simple_query_string"" : {
                                    ""query"": ""#SEARCHWHERE#"",
                                    ""fields"": [""title"", ""description""],
                                    ""default_operator"": ""and""
                                }
                                },
                        ""highlight"":{""fields"":{""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""description"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
            public const string GetSocialMediaChartDataQuery4 = @"{""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""user"", ""messages""],
                                ""default_operator"": ""and""
                            }
                          },
						  ""highlight"":{""fields"":{""user"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""messages"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";


            public const string ReadTimesOfIndiaNewsFeedDataQuery1 = @"{""size"": 100,""query"": {""match_all"": { } },""sort"": [{""published"": {""order"": ""desc""}}] }";
            public const string ReadTimesOfIndiaNewsFeedDataQuery2 = @"{""size"": 100,""query"":{""bool"":{""must"":[{""range"": {""published"": {""gte"":""#STARTDATE#"",
                        ""lte"":""#ENDDATE#"",""format"": ""yyyy-MM-dd'T'HH:mm:ss""}}}]}},""sort"": [{""published"": {""order"": ""desc""}}]}";
            public const string ReadTimesOfIndiaNewsFeedDataQuery3 = @"{""size"": 100,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"" ],
""query"":""#SEARCHWHERE#""}}}},""highlight"":{""fields"":{
""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}

}},""sort"": [{""published"": {""order"": ""desc""}}]}";
            public const string ReadTimesOfIndiaNewsFeedDataQuery4 = @"{""size"": 100,""query"":{""bool"":{""must"":[{""multi_match"":{""fields"":[ ""message"", ""title"" ],
                            ""query"":""#SEARCHWHERE#""}}, {""range"": { ""published"": {""gte"":""#STARTDATE#"",   ""lte"":""#ENDDATE#"",""format"": ""yyyy-MM-dd'T'HH:mm:ss""}
                                      }}}]}},""highlight"":{""fields"":{
                            ""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
                            ""message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}

                            }},""sort"": [{""published"": {""order"": ""desc""}}]}";
            public const string ReadTimesOfIndiaNewsFeedDataQuery5 = @"{""size"": 100,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"" ],
""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},""highlight"":{""fields"":{
""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}

}},""sort"": [{""published"": {""order"": ""desc""}}]}";

            public const string ReadTimesOfIndiaNewsFeedDataQuery6 = @"{""size"": 100,""query"":{""bool"":{""must"":[{""multi_match"":{""fields"":[ ""message"", ""title"" ],
                                ""query"":""#SEARCHWHERE#""}},{""range"": {
                                            ""published"": {
                                            ""gte"":""#STARTDATE#"",   
                                            ""lte"":""#ENDDATE#"",
                                            ""format"": ""yyyy-MM-dd'T'HH:mm:ss""
                                            }
                                          }}]}},""highlight"":{""fields"":{
                                ""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
                                ""message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}

                                }},""sort"": [{""published"": {""order"": ""desc""}}]}";
            public const string ReadTimesOfIndiaNewsFeedDataQuery7 = @"{""size"": 100,""query"": {
                            ""simple_query_string"" : {
                                ""query"": ""#SEARCHWHERE#"",
                                ""fields"": [""message"", ""title"" ],
                                ""default_operator"": ""and""
                            }
                          },
						  
""highlight"":{""fields"":{
""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}},""sort"": [{""published"": {""order"": ""desc""}}]}";

            public const string LocationDashboardDataQuery1 = @"{""size"": 1000,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"",""pagename"", ""post_message"",
                        ""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",
                        ""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",
                        ""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
                        , ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",
                        ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""  ],
                        ""query"":""#SEARCHWHERE#""}}}}}";
            public const string LocationDashboardDataQuery2 = @"{}
{""query"":{""multi_match"":{""fields"":[""message"", ""title"",""pagename"", ""post_message"",""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""],""query"":""#SEARCHWHERE#""}}}
";


            public const string PoliceStationMapDataQuery1 = @"{""size"": 1000,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"",""pagename"", ""post_message"",
                        ""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",
                        ""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",
                        ""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
                        , ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",
                        ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""  ],
                        ""query"":""#SEARCHWHERE#""}}}}}";


            public const string UpdateLocationCountUsingHangfireDataQuery1 = @"{""size"": 1000,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"",""pagename"", ""post_message"",
                        ""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",
                        ""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",
                        ""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
                        , ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",
                        ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""  ],
                        ""query"":""#SEARCHWHERE#""}}}}}";




            public const string HomeDataQuery1 = @"{""size"": 1000,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"",""pagename"", ""post_message"",
                        ""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",
                        ""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",
                        ""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
                        , ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",
                        ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""  ],
                        ""query"":""#SEARCHWHERE#""}}}}}";




            public const string CreateNotificationForSocialMediaUsingHangfireQuery1 = @"{""size"": 1000,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"",""pagename"", ""post_message"",
                        ""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",
                        ""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",
                        ""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
                        , ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",
                        ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""  ],
                        ""query"":""#SEARCHWHERE#""}},""filter"":{""terms"":{""is_notified"":[false]}}}},
""highlight"":{""fields"":{
""srNo"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""cameraName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""locationName"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""policeStation"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""longitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""latitude"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ipAddress"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""rtspLink"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""typeOfCamera"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""make"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unityp"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unit_status"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""unique_id"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""track_personnel"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""tycod"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""sub_tycod"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""station"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""eid"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""dgroup"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""ag_id"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""user"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""messages"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""title"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""description"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""caption"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""pagename"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""post_message"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""hashtags"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""text"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
""location"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}
}}}";
            public const string CreateNotificationForSocialMediaUsingHangfireQuery2 = @"{""size"": 1000,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"",""pagename"", ""post_message"",
                                ""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",
                                ""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",
                                ""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
                                , ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",
                                ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""  ],
                                ""query"":""#SEARCHWHERE#""}},""filter"":{""terms"":{""is_notified"":[false]}}}},""script"": { ""source"": ""ctx._source['is_notified'] =true""}}";



            public const string ShowAlertQuery1 = @"{}
{""query"":{""multi_match"":{""fields"":[""message"", ""title"",""pagename"", ""post_message"",""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""],""query"":""#SEARCHWHERE#""}}}
";

            public const string ManageElasticDbQuery1 = @"{""hashtags"" : ""#HASHTAGS#"" ,""created_at"" :""#DATE#"" ,""text"" : ""#TEXT#"",""location"" : ""#LOCATION#"",""is_notified"":false,""is_alerted"":false}";





        }
        public class Document
        {
            public const string ReadDocumentQuery = @"{""size"": 100,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""notesubject"", ""notedescription"",""filename"",""noteno"" ],
            ""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}},""filter"":{""terms"":{""_id"":[""#IDS#""]}}}},""highlight"":{""fields"":{
            ""notesubject"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
            ""notedescription"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
            ""filename"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},
            ""noteno"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}

            }}}";
        }
    }
}
