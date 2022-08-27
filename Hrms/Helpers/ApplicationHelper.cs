using Hrms.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hrms.Helpers
{
    public static class ApplicationHelper
    {
        #region "Page Name Constant Value Helper"
        public const string PageLogin = "login";
        public const string PageForgotPassword = "forgot-password";
        public const string PageResetPassword = "reset-password";
        public const string PageRegister = "register";
        public const string PageDashboard = "dashboard";
        public const string PageProfile = "profile";
        public const string PageLogout = "logout";
        #endregion
        #region Value Helper
        public const string SessionUserLogin = "SessionUserLogin";
        public const string EncryptKey = "FCSystem";
        public const string Website_Date_Format = "dd/MM/yyyy";
        public const string UserProfileImageFilePath = "uploads/profileimages";
        #endregion
        #region Enum Helper
        public static class EnumPageType
        {
            public const string Index = "Index";
            public const string Add = "Add";
            public const string Edit = "Edit";
            public const string View = "View";
        }
        public static class EnumRole
        {
            public const string SuperAdministrator = "Super Administrator";
            public const string Administrator = "Administrator";
            public const string Employee = "Employee";
        }
        public static class EnumOrganizationRoles
        {
            public static string Admin = "Admin";
            public static string Employee = "Employee";
        }
        public static class EnumEmployeeStatus
        {
            public const string Active = "Active";
            public const string Resigned = "Resigned";
            public const string Terminated = "Terminated";
            public const string Suspended = "Suspended";
            public const string Absconder = "Absconder";
            public const string UnderProcess = "UnderProcess";
            public const string NotJoined = "NotJoined";
            public const string Released = "Released";
        }
        public class AjaxResponse
        {
            public bool Success { get; set; }
            public string Type { get; set; }
            public string FieldName { get; set; }
            public string Message { get; set; }
            public string TargetURL { get; set; }
            public string Data { get; set; }
        }
        public class EnumEnableDisable
        {
            public const string Enable = "Enable";
            public const string Disable = "Disable";
        }
        public class EnumExpenses
        {
            public const string Paid = "Paid";
            public const string UnPaid = "UnPaid";
        }
        public class EnumMonths
        {
            public const string January = "January";
            public const string February = "February";
            public const string March = "March";
            public const string April = "April";
            public const string May = "May";
            public const string June = "June";
            public const string July = "July";
            public const string August = "August";
            public const string September = "September";
            public const string October = "October";
            public const string November = "November";
            public const string December = "December";
        }
        public class EnumYears
        {
            public const string Y2021 = "2021";
            public const string Y2022 = "2022";
            public const string Y2023 = "2023";
            public const string Y2024 = "2024";
            public const string Y2025 = "2025";
            public const string Y2026 = "2026";
            public const string Y2027 = "2027";
            public const string Y2028 = "2028";
            public const string Y2029 = "2029";
            public const string Y2030 = "2030";
            public const string Y2031 = "2031";
            public const string Y2032 = "2032";
            public const string Y2033 = "2033";
            public const string Y2034 = "2034";
            public const string Y2035 = "2035";
            public const string Y2036 = "2036";
            public const string Y2037 = "2037";
            public const string Y2038 = "2038";
            public const string Y2039 = "2039";
            public const string Y2040 = "2040";
        }
        public static class EnumResponseStatus
        {
            public const string FinanceProceeded = "Finance Proceeded";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Pending = "Pending";
            public const string Cancel = "Cancel";
            public const string Completed = "Completed";
        }
        public static class EnumMenuType
        {
            public const string Parent = "P";
            public const string Children = "C";
        }
        public static class EnumMaritalStatus
        {
            public const string Single = "Single";
            public const string Married = "Married";
            public const string Divorced = "Divorced";
        }
        public static class EnumReligionStatus
        {
            public const string Islam = "Islam";
            public const string Bahai = "Baha'i";
            public const string Buddhism = "Buddhism";
            public const string Christianity = "Christianity";
            public const string Confucianism = "Confucianism";
            public const string Hinduism = "Hinduism";
            public const string Jainism = "Jainism";
            public const string Judaism = "Judaism";
            public const string Shinto = "Shinto";
            public const string Sikhism = "Sikhism";
            public const string Taoism = "Taoism";
            public const string Zoroastrianism = "Zoroastrianism";
        }
        #endregion
        #region Encrpt and Decrypt
        public static byte[] GetKey
        {
            get
            {
                return ASCIIEncoding.ASCII.GetBytes(EncryptKey);
            }
        }
        public static string Encrypt(string _value)
        {
            string ReturnValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(_value))
            {
                ReturnValue = EncryptString(_value, GetKey);
            }
            return ReturnValue;
        }
        private static string EncryptString(string _value, byte[] _bytes)
        {
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(_bytes, _bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(_value);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        public static string Decrypt(string _value)
        {
            string ReturnValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(_value))
            {
                ReturnValue = DecryptString(_value, GetKey);
            }
            return ReturnValue;
        }
        private static string DecryptString(string _value, byte[] _bytes)
        {
            _value = Regex.Replace(_value, "[ ]", "+");
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(_value));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(_bytes, _bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
        public static string CreatePassword(int _length)
        {
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder ReturnValue = new StringBuilder();
            Random rnd = new Random();
            while (0 < _length--)
            {
                ReturnValue.Append(characters[rnd.Next(characters.Length)]);
            }
            return ReturnValue.ToString();
        }
        public static string CreateTokenURL()
        {
            Random rnd = new Random();
            string str = rnd.Next(0, 10000) + GetUtcDateTime().ToString();
            ASCIIEncoding AE = new ASCIIEncoding();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(AE.GetBytes(str));
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            StringBuilder sb = new StringBuilder(16);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion
        #region Session Helper
        public static void AddSession(Controller _controller, string _key, object _value)
        {
            _controller.HttpContext.Session.SetString(_key, JsonConvert.SerializeObject(_value));
        }
        public static object GetSession(Controller _controller, string _key)
        {
            object ReturnObject = null;
            var SessionObject = _controller.HttpContext.Session.GetString(_key);
            if (SessionObject != null)
            {
                ReturnObject = SessionObject;
            }
            return ReturnObject;
        }
        public static void RemoveSession(Controller _controller, string _key)
        {
            var SessionObject = _controller.HttpContext.Session.Get(_key);
            if (SessionObject != null)
            {
                _controller.HttpContext.Session.Remove(_key);
            }
        }
        public static bool IsUserLogin(Controller _controller)
        {
            bool ReturnValue = false;
            if (GetSession(_controller, SessionUserLogin) != null)
            {
                ReturnValue = true;
            }
            return ReturnValue;
        }
        public static UserSessionData GetUserData(Controller _controller)
        {
            UserSessionData UserRecord = null;
            if (IsUserLogin(_controller))
            {
                var jsonResult = JsonConvert.DeserializeObject(GetSession(_controller, SessionUserLogin).ToString());
                UserRecord = JsonConvert.DeserializeObject<UserSessionData>(jsonResult.ToString());
            }
            return UserRecord;
        }
        #endregion
        #region "Setting Table Helper"
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        public static string GetSubDomainSpecificUrl(string url, string subDomain)
        {
            string ReturnContent = "#";
            string[] parts = url.Split('/');
            StringBuilder newPath = new StringBuilder(parts[0]);
            for (int i = 1; i < parts.Length; i++)
            {
                if (i == 2)
                {
                    newPath.AppendFormat("/{0}", subDomain);
                    if (subDomain.EndsWith("."))
                    {
                        newPath.Append(parts[i]);
                    }
                    else
                    {
                        newPath.AppendFormat(".{0}", parts[i]);
                    }
                }
                else
                {
                    newPath.AppendFormat("/{0}", (string.IsNullOrEmpty(parts[i]) ? "" : parts[i]));
                }
            }
            ReturnContent = newPath.ToString();
            return ReturnContent;
        }
        public static string GetSettingContentByName(HrmsContext dbContext, string _Key)
        {
            string ReturnContent = "#";
            var SettingRecord = dbContext.Settings.FirstOrDefault(o => o.Name.Equals(_Key));
            if (SettingRecord != null)
            {
                ReturnContent = SettingRecord.Content;
            }
            return ReturnContent;
        }
        #endregion
        #region Core Functions
        public static string GetViewName(string _viewName)
        {
            return "~/Views/" + _viewName + ".cshtml";
        }
        public static DateTime GetDateTime(string timeZoneId)
        {
            TimeZoneInfo time_zone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, time_zone);
        }
        public static DateTime GetUtcDateTime()
        {
            //TimeZoneInfo time_zone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            //return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, time_zone);
            return DateTime.UtcNow;
        }
        public static string TextToSlug(string _value)
        {
            Regex rgx = new Regex(@"[^-a-zA-Z0-9\d\s]");
            // Replace Special Charater and space with emptystring 
            string finalOutput = rgx.Replace(_value, "");
            Regex rgx1 = new Regex("\\s+");
            // Replace space with underscore 
            finalOutput = rgx1.Replace(finalOutput, "-");
            if (finalOutput.StartsWith("-") || finalOutput.StartsWith(" "))
            {
                finalOutput = finalOutput.TrimStart(finalOutput[0]);
            }
            if (finalOutput.EndsWith("-") || finalOutput.EndsWith(" "))
            {
                finalOutput = finalOutput.TrimEnd(finalOutput[finalOutput.Length - 1]);
            }
            return finalOutput.ToLower();
        }
        public static string TrimCharacters(string _value, int _length = 1)
        {
            if (string.IsNullOrWhiteSpace(_value))
            {
                return _value;
            }
            else
            {
                return _value.TrimEnd(_value[_value.Length - _length]);
            }
        }
        public static string ConvertToWebURL(HrmsContext dbContext, string _value)
        {
            if (!string.IsNullOrWhiteSpace(_value) && !_value.Equals("#"))
            {
                if (_value.IndexOf("www.") == -1 && _value.IndexOf("http") == -1 && _value.IndexOf("https") == -1)
                {
                    if (dbContext == null)
                    {
                        dbContext = new HrmsContext();
                    }
                    if (_value.ToLower().Equals("home") || _value.Equals("/"))
                    {
                        _value = GetSettingContentByName(dbContext, "Website URL") + _value;
                    }
                    else
                    {
                        _value = GetSettingContentByName(dbContext, "Website URL") + _value;
                    }
                }
            }
            else
            {
                _value = "#";
            }
            return _value;
        }
        public static string StripHtmlTags(string _value)
        {
            return Regex.Replace(_value, "<.*?>|&.*?;", string.Empty);
        }
        public static string UpperCaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        public static string UpperCaseFirstLetter(string _value)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(_value))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(_value[0]) + _value.Substring(1);
        }
        public static List<string> GetTimeZoneList()
        {
            List<string> timeList = new List<string>();
            var tzone = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo tzi in tzone)
            {
                timeList.Add(tzi.Id);
            }
            return timeList;
        }
        public static string CreateFileName(string fileName)
        {
            string[] strGUID = Guid.NewGuid().ToString().Split(new char[] { '-' });
            return fileName + "-" + strGUID[0];
        }
        #endregion
        #region "Redirect Page Helper"
        public static List<string> WithOutLoginView()
        {
            List<string> PageList = new List<string>();
            PageList.Add(PageLogin);
            PageList.Add(PageForgotPassword);
            PageList.Add(PageResetPassword);
            PageList.Add(PageRegister);
            return PageList;
        }
        public static List<string> WithLoginView()
        {
            List<string> PageList = new List<string>();
            PageList.Add(PageProfile);
            PageList.Add(PageLogout);
            return PageList;
        }
        public static List<string> AllowedLink()
        {
            List<string> PageList = new List<string>();
            PageList.Add("dashboard");
            PageList.Add("dashboard/logout");
            PageList.Add("dashboard/profile");
            PageList.Add("dashboard/employeeprofile");
            PageList.Add("dashboard/accessunauthorized");
            PageList.Add("dashboard/changepassword");
            PageList.Add("employees/downloaddefaultdataformat");
            PageList.Add("payrolls/downloaddefaultdataformat");
            PageList.Add("payrolls/downloadorganizationallowanceformat");
            PageList.Add("employeeleaves/downloadattachements");
            PageList.Add("leaves/downloadattachements");
            PageList.Add("employeeexpenses/downloadattachements");
            PageList.Add("travelexpenses/downloadattachements");
            PageList.Add("employeeleaves/downloadattachements");
            PageList.Add("advancesalaries/downloadattachements");
            PageList.Add("loans/downloadattachements");
            PageList.Add("employeeloans/downloadattachements");
            PageList.Add("attendancereport/orgattendanceexport");
            PageList.Add("attendancereport/getdeviceattandancereport");
            PageList.Add("attendancereport/ExportTopdf");
            PageList.Add("rosters/downloaddefaultdataformat");
            PageList.Add("monthlyattendance/downloaddefaultdataformat");
            PageList.Add("employees/downloaddefaultemployeesformat");
            PageList.Add("employeetaxdeductions/downloadattachements");
            PageList.Add("employeetaxadjustables/downloadattachements");
            PageList.Add("taxdeductions/downloadattachements");
            PageList.Add("taxadjustables/downloadattachements");
            PageList.Add("notes/downloadattachements");
            PageList.Add("employeenotes/downloadattachements");
            PageList.Add("employeeannouncementcontents/employeeannouncementdetail");
            PageList.Add("employeeletters");
            PageList.Add("vacancies/review");
            PageList.Add("vacancies/applicationdetail");
            PageList.Add("vacancies/downloadattachements");
            PageList.Add("organizationclearancereviews/downloadattachements");
            PageList.Add("vacancyapplicationinterview/candidatedetail");
            PageList.Add("employeevacancies/review");
            PageList.Add("employeevacancies/applicationdetail");
            PageList.Add("employeevacancies/downloadattachements");
            return PageList;
        }
        #endregion
        #region "Default Values Helper"
        public static int ParseInt(object value)
        {
            int parseVal;
            return ((value == null) || (value == DBNull.Value)) ? 0 : int.TryParse(value.ToString(), out parseVal) ? parseVal : 0;
        }
        public static decimal ParseDecimal(object value)
        {
            decimal parseVal;
            return ((value == null) || (value == DBNull.Value)) ? 0 : decimal.TryParse(value.ToString(), out parseVal) ? parseVal : 0;
        }
        public static double ParseDouble(object value)
        {
            double parseVal;
            return ((value == null) || (value == DBNull.Value)) ? 0 : double.TryParse(value.ToString(), out parseVal) ? parseVal : 0;
        }
        public static DateTime ParseDateTime(object value)
        {
            DateTime parseVal;
            return ((value == null) || (value == DBNull.Value)) ? new DateTime(1900, 1, 1) : DateTime.TryParse(value.ToString(), out parseVal) ? parseVal : new DateTime(1900, 1, 1);
        }
        public static DateTime ParseExactDateTime(object value)
        {
            DateTime parseVal;
            CultureInfo ci = CultureInfo.CreateSpecificCulture("ur-PK");
            return ((value == null) || (value == DBNull.Value)) ? new DateTime(1900, 1, 1) : DateTime.TryParseExact(value.ToString(), Website_Date_Format, ci, DateTimeStyles.None, out parseVal) ? parseVal : new DateTime(1900, 1, 1);
        }
        public static DateTime ParsePickerDateTime(object value)
        {
            DateTime parseVal;
            CultureInfo ci = CultureInfo.CreateSpecificCulture("ur-PK");
            return ((value == null) || (value == DBNull.Value)) ? new DateTime(1900, 1, 1) : DateTime.TryParse(value.ToString(), ci.DateTimeFormat, DateTimeStyles.None, out parseVal) ? parseVal : new DateTime(1900, 1, 1);
        }
        public static string ParseString(object value)
        {
            return ((value == null) || (value == DBNull.Value)) ? string.Empty : value.ToString();
        }
        public static bool ParseBoolean(object value)
        {
            bool parseVal;
            return ((value == null) || (value == DBNull.Value)) ? false : bool.TryParse(value.ToString(), out parseVal) ? parseVal : false;
        }
        public static bool IsEmailAddressValid(string EmailAddress)
        {
            string pattern = @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(EmailAddress);
        }
        #endregion
    }
}