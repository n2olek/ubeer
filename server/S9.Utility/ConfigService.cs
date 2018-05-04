using System;
using System.Collections.Specialized;
using System.Configuration;

namespace S9.Utility
{
    public class ConfigService
    {
        protected static NameValueCollection AppSettings = ConfigurationManager.AppSettings;

        public static string GetValue(string key, string defaultValue = "")
        {
            try
            {
                return AppSettings[key].ToString();
            }
            catch
            {
            }

            return defaultValue;
        }
        
        public static int GetValueAsInt(string key, int defaultValue = default(int))
        {
            int value = defaultValue;
            try
            {
                return int.TryParse(AppSettings[key].ToString(), out value) ? value : defaultValue;
            }
            catch { }

            return value;            
        }

        public static long GetValueAsLong(string key, long defaultValue = default(long))
        {
            long value = defaultValue;
            try
            {
                return long.TryParse(AppSettings[key].ToString(), out value) ? value : defaultValue;
            }
            catch { }

            return value;
        }



        public static bool GetValueAsBool(string key, bool defaultValue = default(bool))
        {
            try
            {
                return (ConfigurationManager.AppSettings[key].ToString().ToLower().Trim() == "true");
            }
            catch
            {
            }

            return defaultValue;
        }

        public static float GetValueAsFloat(string key, float defaultValue = default(float))
        {
            float value = defaultValue;
            try
            {
                return float.TryParse(AppSettings[key].ToString(), out value) ? value : defaultValue;
            }
            catch { }

            return value;
        }

        public static double GetValueAsDouble(string key, double defaultValue = default(double))
        {
            double value = defaultValue;
            try
            {
                return double.TryParse(AppSettings[key].ToString(), out value) ? value : defaultValue;
            }
            catch { }

            return value;
        }
        
        protected static DateTime GetValueAsDateTime(string key, DateTime defaultValue = default(DateTime))
        {
            DateTime value = defaultValue;
            try
            {
                return DateTime.TryParse(AppSettings[key].ToString(), out value) ? value : defaultValue;
            }
            catch { }

            return value;
        }

        protected static T GetValueAsDateTime<T>(string key, T defaultValue = default(T)) where T : struct
        {
            T ret;
            return Enum.TryParse(AppSettings[key], out ret) ? ret : defaultValue;
        }


    }
}