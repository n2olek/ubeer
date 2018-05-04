using S9.Utility;

namespace UBeer.Services
{
    // helper in business logic
    public class Storage : IStorage
    {
        private string _URL;
        private string _physicalPath;

        public Storage(string URL, string physicalPath)
        {
            _URL = URL;
            _physicalPath = physicalPath;
        }

        public string GetFullURL(string relativeURL)
        {
            if (relativeURL.StartsWith("/"))
                // take out "/"
                relativeURL = relativeURL.Substring(1, relativeURL.Length - 1);
            
            if (relativeURL.StartsWith("~/"))
                // take out "~/"                
                relativeURL = relativeURL.Substring(2, relativeURL.Length - 2);
            
            return _URL + relativeURL;
        }

        public string GetFullPath(string relativePath)
        {
            relativePath = relativePath.Replace("~", "");
            if (relativePath.StartsWith(@"\"))
                // take out "\"
                relativePath = relativePath.Substring(1, relativePath.Length - 1);
            
            return _physicalPath + relativePath;
        }

        public bool MakeFolderExist(string filePath)
        {
            return FileUtil.MakeFolderExist(filePath);
        }


        public bool DeleteFile(string file)
        {
            return FileUtil.DeleteFile(file);
        }

        public bool MoveFile(string fileSrc, string fileDst)
        {
            return FileUtil.MoveFile(fileSrc, fileDst);
        }

        public bool CopyFile(string fileSrc, string fileDst)
        {
            return FileUtil.CopyFile(fileSrc, fileDst);
        }


    }
}