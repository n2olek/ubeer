using System;
using S9.Utility;

namespace UBeer.Services
{
    // helper in business logic
    public class Helper
    {
        // property for controller to set        
        public static string DisplayDateFormat
        {
            get
            {
                return "dd.MM.yyyy";                
            }
        }
            
        public static string DisplayTimeFormat
        {
            get
            {
                return "HH:mm:ss";
            }
        }

        public static string DateTimeSignature
        {
            get
            {
                DateTime dtNow = DateTime.UtcNow;
                return dtNow.ToString( "yyyyMMddHHmmss" );
            }
        }

        public static int ScoreScale
        {
            get
            {
                return 5;
            }
        }

        public static string GetPaddedID(int ID)
        {
            return ID.ToString().PadLeft(6, '0');
        }

        public static string GetThumbnailFileName(string imageFileName)
        {
            return FileUtil.GetFileNameWithoutExtension(imageFileName) + "_t" + FileUtil.GetFileExtension(imageFileName);
        }

        public static string GetCreditCardAMask(string creditCardNumber)
        {
            return "XXXX XXXX XXXX " + creditCardNumber.Substring(12, 4);
        }


        /*/
        public static string GetImageRejectFlagString(int flag)
        {
            string strResult = "";

            foreach (UBeer_Enum.PHOTO_REJECT_REASON_FLAG f in EnumList<UBeer_Enum.PHOTO_REJECT_REASON_FLAG>.List())
            {
                // concatenate string
                if (0 < (flag & (int)f))
                    strResult += f.ToDescription() + " ";
            }

            return strResult;
        }
        /*/        
    }
}