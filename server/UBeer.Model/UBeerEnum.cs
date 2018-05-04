using System;
using System.Collections.Generic;
using System.Linq;

// Enum description
using System.ComponentModel;
using System.Reflection;


namespace UBeer.Models
{
    public class UBeerEnum
    {
        // store values in enum variable instead of in the database 
        // as we believe the role shouldn't be dynamic
        // if the roles changed, we have to change the logic also, anyway

        // To add description we can use technic to add extra attribute to enum later


        public enum SOURCE
        {
            [DescriptionAttribute("Facebook")]
            Facebook = 1,

            [DescriptionAttribute("Twitter")]
            Twitter = 2,

            [DescriptionAttribute("Instagram")]
            Instagram = 3,

            [DescriptionAttribute("Upload File")]
            UploadFile = 4,
        }

        public enum CONTENT_TYPE
        {
            [DescriptionAttribute("Video")]
            Video = 1,

            [DescriptionAttribute("Photo")]
            Photo = 2,
        }

        public enum SERVICE
        {
            [DescriptionAttribute("Feed Monitoring")]
            FeedMonitoring = 1,
        }
    }


    public class EnumList<T>
    {
        public static List<T> List()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }

    // Use "Extension method" technique, this can use with enum        
    public static class EnumExt
    {
        // ext method
        public static string ToDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(
                                                typeof(DescriptionAttribute),
                                                false
                                                );

                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;

            }

            return en.ToString();
        }


        // ext method
        public static bool IsPart(this Enum en, int valueToCheck)
        {
            int iValue = Convert.ToInt32(en);
            return (0 != (iValue & valueToCheck));
        }

    }
}
