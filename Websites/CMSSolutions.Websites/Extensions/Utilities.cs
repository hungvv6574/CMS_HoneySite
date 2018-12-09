using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace CMSSolutions.Websites.Extensions
{
    public class Utilities
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static T ConvertJsonToObject<T>(string input)
        {
            var json = new JavaScriptSerializer();
            return json.Deserialize<T>(input.Trim());
        }

        public static string ConvertObjectToJson<T>(T input)
        {
            var json = new JavaScriptSerializer();
            return json.Serialize(input);
        }

        public static string GetCurrency(string value)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            var rs = double.Parse(value).ToString("#,###", cul.NumberFormat);
            return rs.Replace('.', ',');
        }
        public static string GetCharUnsigned(string values)
        {
            if (string.IsNullOrEmpty(values))
            {
                return string.Empty;
            }

            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = values.Normalize(NormalizationForm.FormD);
            var converttext = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            converttext = Regex.Replace(converttext, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);

            var list = new char[] { ' ', '/', ',', '&', '\"', '?', '|', ':', '"', '`', '\\', ';', '~', '!', '@', '#', '$', '%', '^', '*', '(', ')', '\'', '_', '=', '+', '{', '}', '[', ']', '.', '>', '<' };
            converttext = list.Aggregate(converttext, (current, schar) => current.Replace(schar, ' '));

            converttext = converttext.Replace("--", " ").Trim('.').TrimEnd(' ').TrimStart(' ');

            return converttext.ToLower();
        }

        public static int[] ParseListInt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new int[0];
            }

            return value.Split(',').Select(Int32.Parse).ToArray();
        }

        public static string ParseString(int[] list)
        {
            if (list != null && list.Length > 0)
            {
                return string.Join(", ", list);
            }

            return string.Empty;
        }

        public static string GetAlias(string values)
        {
            if (string.IsNullOrEmpty(values))
            {
                return string.Empty;
            }

            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = values.Normalize(NormalizationForm.FormD);
            var converttext = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            converttext = Regex.Replace(converttext, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);

            var list = new char[] { ' ', '/', ',', '&', '\"', '?', '|', ':', '"', '`', '\\', ';', '~', '!', '@', '#', '$', '%', '^', '*', '(', ')', '\'', '_', '=', '+', '{', '}', '[', ']', '.', '>', '<' };
            converttext = list.Aggregate(converttext, (current, schar) => current.Replace(schar, '-'));

            converttext = converttext.Replace("--", "-").Trim('.').TrimEnd('-').TrimStart('-');

            return converttext.ToLower();
        }

        public static bool IsNotNull(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.Equals(Constants.IsNull) || value.Equals(""))
            {
                return false;
            }

            if (value.Equals(Constants.IsUndefined))
            {
                return false;
            }

            return true;
        }

        public static void WriteEventLog(string messages)
        {
            try
            {
                var eventLogName = "ApplicationErrors";
                if (!EventLog.SourceExists(eventLogName))
                {
                    EventLog.CreateEventSource(eventLogName, "Application Errors");
                }

                var log = new EventLog { Source = eventLogName };
                log.WriteEntry(messages, EventLogEntryType.Error);
            }
            catch (Exception)
            {

            }
        }
    }
}