using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace S9.Utility
{
    public class FileUtil
    {
        public static string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public static string GetFileNameWithoutExtension(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        public static bool CopyFile(string source, string destination)
        {
            try
            {
                File.Copy(source, destination, true);
                return true;
            }
            catch
            {

            }
            
            return false;
        }


        public static bool MoveFile(string source, string destination)
        {
            try
            {
                if (File.Exists(destination))
                    File.Delete(destination);
                File.Move(source, destination);
                
                return true;
            }
            catch
            {
            
            }
            /*/
            catch (DirectoryNotFoundException DirNotFound)
            {
                //LogFilesUtil.WriteToLog("UtilityOfFiles", "MoveFile", DirNotFound.Message);
                return false;
            }
            catch (UnauthorizedAccessException UnAuthDir)
            {
                //LogFilesUtil.WriteToLog("UtilityOfFiles", "MoveFile", UnAuthDir.Message);
                return false;
            }
            catch (PathTooLongException LongPath)
            {
                //LogFilesUtil.WriteToLog("UtilityOfFiles", "MoveFile", LongPath.Message);
                return false;
            }
            catch (Exception ex)
            {
                //LogFilesUtil.WriteToLog("UtilityOfFiles", "MoveFile", ex.Message);
                return false;
            }
            /*/

            return false;
        }

        public static bool DeleteFile(string destination)
        {
            if (File.Exists(destination))
            {
                try
                {
                    //File.OpenRead(destination).Dispose();
                    File.Delete(destination);
                }
                catch { }
            }                
            return true;
        }

        // Make sure the path exists: 
        // if the path already exists, it's fine; otherwise, create the new one
        // can create nested new folder
        public static bool MakeFolderExist(string strFolder)
        {
            if (false == Directory.Exists(strFolder))
            {
                try
                {
                    Directory.CreateDirectory(strFolder);
                }
                catch
                {
                    // It somehow failed to create the folder, 
                    // check the permission of the executing user
                    return false;
                }
            }

            // It means the folder exists
            return true;
        }

        public string ReadFirstLine(string filename)
        {
            string returnValue = "";
            using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default))
            {
                returnValue = sr.ReadLine( );
                //sr.Close( );
            }
            return returnValue;
        }

        public string ReadLastLine(string filename)
        {
            string returnValue = "";
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            stream.Seek(-1024, SeekOrigin.End);     // rewind enough for > 1 line 

            StreamReader reader = new StreamReader(stream);
            reader.ReadLine( );      // discard partial line 
            string nextLine;
            while (!reader.EndOfStream)
            {
                nextLine = reader.ReadLine( );
                if (nextLine != null)
                {
                    if (nextLine.Substring(0, 3) == "FT|")
                    {
                        returnValue = nextLine;
                    }
                }
            }
            stream.Close( );
            return returnValue;
        }

        public int CountLine(string filename)
        {
            int result = 0;
            using (StreamReader fs = new StreamReader(filename))
            {
                while (fs.ReadLine( ) != null)
                {
                    result++;
                }
                //fs.Close( );
            }
            return result;
        }


        // get files sorting by modified date (new to old)
        public static List<FileInfo> GetFiles(string folder)
        {
            // direcotry object
            var directory = new DirectoryInfo(folder);

            // list files in the folder
            var files = (
                            from 
                                f in directory.GetFiles()
                            orderby 
                                f.LastWriteTime descending
                            select 
                                f
                          ).ToList();

            return files;
        }


        // get list of direcotries
        public static List<DirectoryInfo> GetDirectories(string folder)
        {
            // root
            var directory = new DirectoryInfo(folder);

            // list directories in the folder
            var dirs = (
                            from
                                f in directory.GetDirectories()
                            orderby
                                f.LastWriteTime descending
                            select
                                f
                          ).ToList();

            return dirs;
        }

    }
}
