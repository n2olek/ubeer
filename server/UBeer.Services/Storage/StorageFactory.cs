using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using UBeer.Models;
using S9.Utility;

namespace UBeer.Services
{
    public class StorageFactory
    {
        // using Factory design pattern, 
        // in the future if we change the Storage ie from physical to Amazon cloud, 
        // we just simply change the the object instantiation
        public static IStorage GetInstance(string URL, string physicalPath)
        {
            return new Storage(URL, physicalPath);
        }

    }
}