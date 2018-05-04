using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S9.Utility
{
    public class TextUtil
    {

        public static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable

            if (String.IsNullOrEmpty(param) || length < 0)
            {
                return String.Empty;
            }
            else
            {
                return param.Substring(0, Math.Min(length, param.Length));
            }
            //    string result = param.Substring(0, length);
            //    //return the result of the operation
            //    return result;
        }

        public static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            if (String.IsNullOrEmpty(param) || length < 0)
            {
                return String.Empty;
            }
            else
            {
                return param.Substring(param.Length - length, Math.Min(length, param.Length));
            }

            //string result = param.Substring(param.Length - length, length);
            ////return the result of the operation
            //return result;
        }

        public static string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            if (String.IsNullOrEmpty(param) || length < 0)
            {
                return String.Empty;
            }
            else
            {
                return param.Substring(startIndex, Math.Min(length, param.Length));
            }
            //string result = param.Substring(startIndex, length);
            ////return the result of the operation
            //return result;
        }

        public static string Mid(string param, int startIndex)
        {
            //start at the specified index and return all characters after it
            //and assign it to a variable
            if (String.IsNullOrEmpty(param) || startIndex < 0)
            {
                return String.Empty;
            }
            else
            {
                return param.Substring(startIndex);
            }

            //string result = param.Substring(startIndex);
            ////return the result of the operation
            //return result;
        }
        
    }
}
