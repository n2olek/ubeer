using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S9.Utility
{
    public class ValueValidationUtil
    {
        public static string GetString(object data)
        {
            string result = "";
            try
            {
                result = (string)(data == null ? "" : data.ToString());
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("CheckValue", "GetString()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public static int GetInt(object data)
        {
            int result = 0;
            try
            {
                result = (data == null ? 0 : Convert.ToInt32(data));
                //or
                //result=Convert.ToInt16(data);                
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("CheckValue", "GetInt()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public static short GetShort(object data)
        {
            short result = 0;
            try
            {
                result = (short)(data == null ? 0 : data);
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("CheckValue", "GetShort()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public static decimal GetDecimal(object data)
        {
            decimal result = 0;
            try
            {
                result = (decimal)(data == null ? 0 : data);
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("CheckValue", "GetDecimal()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public static decimal GetLong(object data)
        {
            long result = 0;
            try
            {
                result = (long)(data == null ? 0 : data);
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("CheckValue", "GetLong()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public static DateTime GetDateTime(object data)
        {
            DateTime result = DateTime.Now;
            try
            {
                result = (DateTime)(data == null ? DateTime.Now : data);
                //result = Convert.ToDateTime(data);
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("CheckValue", "GetDateTime()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public static DateTime? GetDateTimeNull(object data)
        {
            DateTime? result = null;
            try
            {
                result = (DateTime?)(data == null ? null : data);
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("CheckValue", "GetDateTime()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }
        public static bool GetBoolean(object data)
        {
            bool result = false;
            try
            {
                result = (bool)(data == null ? false : data);
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("CheckValue", "GetBool()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public static bool IsNumeric(string data)
        {
            return IsNumeric(data, false);
        }

        public static bool IsNumeric(string data, bool allowDecimal)
        {
            bool result = true;
            if (String.IsNullOrEmpty(data))
            {
                return false;
            }

            if (data.StartsWith("-"))
            {
                data = data.Substring(1);
            }

            char[] chars = data.ToCharArray();

            if (allowDecimal)
            {
                bool decimalFound = false;
                foreach (char c in chars)
                {
                    if (c == '.' && !decimalFound)
                    {
                        decimalFound = true;
                    }
                    else
                    {
                        result = result & (char.IsNumber(c));
                    }
                }
            }
            else
            {
                foreach (char c in chars)
                {
                    result = result & char.IsNumber(c);
                }
            }

            return result;
        }

        public static bool IsDate(string data)
        {
            DateTime dt;
            if (DateTime.TryParse(data, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out dt) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
