using System;
//using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

using Ionic.Zip;

namespace S9.Utility
{
    public class ZipUtil
    {

        public static void Compress(string fileName,string zipName)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddFile(fileName,"");
                zip.Save(zipName);
            }
        }

        public static void Compress(string[] fileNames, string zipName)
        {
            using (ZipFile zip = new ZipFile())
            {
                foreach (var file in fileNames)
                    zip.AddFile(file,"");

                try
                {
                    zip.Save(zipName);
                }
                catch(Exception ex) 
                {
                    string strTemp = ex.Message;
                }
            }
        }

        public static void Compress(string[] files, Stream output)
        {
            using (ZipFile zip = new ZipFile())
            {
                try
                {                    
                    foreach (var file in files)
                        zip.AddFile(file, "");
                    
                    zip.Save(output);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }


        public static void Decompress(string zipName, string targetDirectory)
        {
            using (ZipFile zip = ZipFile.Read(zipName))
            {
                foreach (ZipEntry e in zip)
                {
                    e.Extract(targetDirectory);
                }
            }
        }

    }
}
