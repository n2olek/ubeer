using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

namespace S9.Utility
{
    public class UniqueGenerator
    {
        // generate global unique ID
        public static string GenerateGUID()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }

        // generate random digits (for OTP)
        public static string GenerateOTPDigits(int digitCount)
        {
            Random generator = new Random();
            int r = generator.Next(100000, 1000000);
            string random = r.ToString().PadLeft(digitCount, '0');

            return random;
        }


    }
}