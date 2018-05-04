using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace S9.Utility
{
    public class DateUtil
    {
        public enum DateFormat
        {
            DateDMY = 1,
            DateMDY = 2,
            DateYMD = 3,
            StringDMY = 4,
            StringYMD = 5,
            StringMY = 6,
            StringYM = 7
        }

        public enum YearFormat
        {
            Buddhist = 1,
            Christian = 2 //Defuat
        }

        public static string ConvertDate(string _dt, DateFormat _display, YearFormat _year = YearFormat.Christian)
        {
            string _date = "";
            if (( _dt != null ))
            {
                if (_dt.Trim( ).Length > 0)
                {
                    string _dd = "";
                    string _mm = "";
                    string _yy = "";
                    string[] _data = null;
                    _data = _dt.Split('/');   
                    if (_data.Length > 1)
                    {
                        _dd = _data[0];
                        _mm = _data[1];
                        _yy = _data[2];
                    }
                    else
                    {
                        _dd = TextUtil.Right(_dt, 2);
                        _mm = TextUtil.Mid(_dt, 4, 2);
                        _yy = TextUtil.Left(_dt, 4);
                    }

                    if (_dd.Trim( ).Length == 1)
                    {
                        _dd = "0" + _dd.Trim( );
                    }

                    if (_mm.Trim( ).Length == 1)
                    {
                        _mm = "0" + _mm.Trim( );
                    }

                    switch (_year)
                    {
                        case YearFormat.Buddhist:
                            if (Convert.ToInt32(_yy) < 2400)
                            {
                               _yy =Convert.ToString(Convert.ToInt32(_yy) + 543);
                            }
                            break;
                        case YearFormat.Christian:
                            if (Convert.ToInt32(_yy) > 2400)
                            {
                                _yy = Convert.ToString(Convert.ToInt32(_yy) - 543);
                            }
                            break;
                    }

                    switch (_display)
                    {
                        case DateFormat.DateDMY:
                            _date = _dd + "/" + _mm + "/" + _yy;
                            break;
                        case DateFormat.DateMDY:
                            _date = _mm + "/" + _dd + "/" + _yy;
                            break;
                        case DateFormat.DateYMD:
                            _date = _yy + "/" + _mm + "/" + _dd;
                            break;
                        case DateFormat.StringDMY:
                            _date = _dd + _mm + _yy;
                            break;
                        case DateFormat.StringYMD:
                            _date = _yy + _mm + _dd;
                            break;
                        case DateFormat.StringMY:
                            _date = _mm + _yy;
                            break;
                        case DateFormat.StringYM:
                            _date = _yy + _mm;
                            break;
                    }
                }
            }
            return _date;
        }

        /// <summary>
        /// Check date
        /// </summary>
        /// <param name="value">yyyy/mm/dd</param>
        /// <returns></returns>
        public static bool IsDate(string value)
        {
            DateTime dt;
            if (DateTime.TryParse(value, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out dt) == true)
            {
                return true;
            }
            else
            {
                return false;
            }

            //try
            //{
            //    DateTime dt = DateTime.Parse(value,CultureInfo.CreateSpecificCulture("en-US"));
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public static string ConvertDateToThaiDate(DateTime dt)
        {
            return ConvertDateToThaiDate(dt, string.Empty);
        }

        // thai year?
        public static string ConvertDateToThaiDate(DateTime dt, string format)
        {
            var culture = new CultureInfo("th-TH");
            var dateValue = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
            return dateValue.ToString(( format.Length == 0 ) ? "d" : format, culture);
        }

        public static string ConvertDate(string sdt)
        {
            CultureInfo culture = new CultureInfo("en-US");
            return DateTime.ParseExact(sdt, "MM-dd-yy hh:mmtt", culture, DateTimeStyles.AllowInnerWhite).ToString("dd/MM/yyyy HH:mm");

        }

        public static DateTime ConvertDate(string sdt, string format)
        {
            CultureInfo culture = new CultureInfo("en-US");
            return DateTime.ParseExact(sdt, format, culture);
        }

        public static DateTime ConvertDate(string sdt, int hh, int mm)
        {
            CultureInfo culture = new CultureInfo("en-US");
            string cdate = sdt + " " + hh + ":" + mm;
            return DateTime.Parse(cdate, culture);
        }

        //========================
            //string period = UtilityOfDate.ConvertDate("20120114", UtilityOfDate.DateFormat.DateYMD);            
            //Console.WriteLine(period);
            //Console.WriteLine("First day of Month: {0}",UtilityOfDate.FirstDayOfMonth(DateTime.Now));
            //Console.WriteLine("Last day of Month: {0}",UtilityOfDate.LastDayOfMonth(DateTime.Now));
            //Console.WriteLine("First day Previous Month: {0}", UtilityOfDate.FirstDayPreviousMonth(DateTime.Now));
            //Console.WriteLine("Last day Previous Month: {0}", UtilityOfDate.LastDayPreviousMonth(DateTime.Now));
            //DateTime dt= new DateTime(2012,9,20);
            //Console.WriteLine("format: {0}",UtilityOfDate.ConvertDate(dt,"yyyy-MM-dd"));
        //========================

        public static string ConvertDate(DateTime dt, string format)
        {
            var culture = new CultureInfo("en-US");
            var dateValue = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
            return dateValue.ToString(format, culture);
        }

        public static DateTime GetFirstDayOfYear(DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }


        public static DateTime GetFirstDayOfMonth(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime GetLastDayOfMonth(DateTime dt)
        {
            DateTime firstDayOfTheMonth = new DateTime(dt.Year, dt.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public static DateTime GetFirstDayPreviousMonth(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1).AddMonths(-1);
        }

        public static DateTime GetLastDayPreviousMonth(DateTime dt)
        {
            DateTime firstDayOfTheMonth = new DateTime(dt.Year, dt.Month, 1);
            return firstDayOfTheMonth.AddDays(-1); //return last day of pervious
        }

        public static DateTime GetFirstDayOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
                diff += 7;

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;

        }

        public static double DateTimeToUnixTimestamp(DateTime dt)
        {
            return (dt - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public static DateTime ConvertDateTimeFromMillieconds(long milisec)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(milisec);
            return new DateTime(1970, 1, 1) + time;
        }


    }
}
