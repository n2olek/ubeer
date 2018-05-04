using System;


namespace UBeer.Services
{
    public interface IStorage
    {
        string GetFullPath(string relativePath);
        string GetFullURL(string relativeURL);
        bool MakeFolderExist(string filePath); 
       
        bool DeleteFile(string file);
        bool MoveFile(string fileSrc, string fileDst);
        bool CopyFile(string fileSrc, string fileDst);
    }
}
