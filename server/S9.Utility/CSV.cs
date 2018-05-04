using System;
using System.Data;

using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using System.IO;

namespace S9.Utility
{
    public class CSV
    {
        private string m_strFileName = "";  // for physical file mode
        private Stream m_stream = null;     // for memory mode

        private string m_strDelimiter = "";
        
        private bool m_bContentSourcePhysical = true;
        private bool m_bDisposed = false;

        // default constructor
        public CSV()
        {
            m_strDelimiter = ","; // default is CSV
        }


        // constructor 1
        public CSV(string strFileName)
        {
            m_strFileName = strFileName;
            m_bContentSourcePhysical = true;            
            m_strDelimiter = ","; // default is CSV
        }

        // constructor 2        
        public CSV(string strFileName, string strDelimiter)
        {
            m_strFileName = strFileName;
            m_bContentSourcePhysical = true;            
            m_strDelimiter = strDelimiter; // probably tab separated value
        }

        // constructor 3
        public CSV(Stream stream)
        {
            m_stream = stream;
            m_bContentSourcePhysical = false;
            m_strDelimiter = ","; // default is CSV
        }

        
        // START destructor
        protected virtual void Dispose(bool disposing)
        {
            if (!m_bDisposed) // only dispose once!
            {
                if (disposing)
                {
                    //Console.WriteLine("Not in destructor, OK to reference other objects");
                }

                // Do something here
                // 
                // ---- 
            }
            this.m_bDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            // tell the GC not to finalize
            GC.SuppressFinalize(this);
        }

        ~CSV()
        {
            Dispose(false);
        }

        public DataTable Read(bool isContainHeader)
        {
            DataTable dt = new DataTable();

            int iRowStart = 0;

            if (true == isContainHeader)
                iRowStart = 1;

            StreamReader sr = null;
            if(m_bContentSourcePhysical)
                sr = new StreamReader(m_strFileName);
            else
                sr = new StreamReader(m_stream);
                
            string strBuff = sr.ReadToEnd().Trim();
            //string[] arrstrLines = strBuff.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string[] arrstrLines = strBuff.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
            if ( 0 < arrstrLines.Length )
            {
                // Header
                string[] arrstrItems = arrstrLines[0].Split(new string[] { m_strDelimiter }, StringSplitOptions.None);

                // Add column
                for (int iLoop = 0; iLoop < arrstrItems.Length; iLoop++)
                    if (true == isContainHeader)
                        // Use the header of the file
                        dt.Columns.Add(arrstrItems[iLoop]);
                    else
                        // Make the dummy column names in format C0, C1, C2, and so on
                        dt.Columns.Add("C" + iLoop);

            }

            for (int iLineLoop = iRowStart; iLineLoop < arrstrLines.Length; iLineLoop++)
            {
                // Data
                string[] arrstrItems = arrstrLines[iLineLoop].Split(new string[] { m_strDelimiter }, StringSplitOptions.None);
                dt.Rows.Add(arrstrItems);    
            }

            // Close the file
            sr.Close();

            return dt;
        }


        public bool Save(string[] strItems, bool isContainHeader)
        {
            // Create a CSV file to which grid data will be exported (not append).
            StreamWriter sw = new StreamWriter(m_strFileName, false, Encoding.UTF8);

            // Count the column 
            int iColCount = strItems.Length;

            // First, write the headers
            if (true == isContainHeader)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(strItems[i]);
                    if (i < iColCount - 1)
                        sw.Write(",");
                }

                sw.Write(sw.NewLine);
            }


            // write all data to the rows.
            foreach (string str in strItems)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(str[i]))
                        sw.Write(str);
                    if (i < iColCount - 1)
                        sw.Write(",");
                }
                sw.Write(sw.NewLine);
            }

            // Close the file
            sw.Close();

            return true;
        }

        
        // save to create a new file (not append)
        public bool Save(DataTable dt, bool isContainHeader)
        {
            // create a new file
            Save(dt, isContainHeader, false);

            return true;
        }

        public bool Save(DataTable dt, bool bIncludeHeader, bool append)
        {
            // Create a CSV file to which grid data will be exported (not append).
            // also add the UTF8 encode to support the Thai character                
            StreamWriter sw = new StreamWriter(m_strFileName, append, Encoding.UTF8);

            // Count the column 
            int iColCount = dt.Columns.Count;

            // First, write the headers
            if (true == bIncludeHeader)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < iColCount - 1)
                        sw.Write(",");
                }

                sw.Write(sw.NewLine);
            }


            // write all data to the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                        sw.Write(dr[i].ToString());
                    if (i < iColCount - 1)
                        sw.Write(",");
                }
                sw.Write(sw.NewLine);
            }

            // Close the file
            sw.Close();

            return true;
        }




        public MemoryStream GetStream(DataTable dt, bool bIncludeHeader)
        {
            using(var s = new MemoryStream())
            {
                // Create a CSV file to which grid data will be exported (not append).
                // also add the UTF8 encode to support the Thai character
                StreamWriter sw = new StreamWriter(s, Encoding.UTF8);

                // Count the column 
                int iColCount = dt.Columns.Count;

                // First, write the headers
                if (true == bIncludeHeader)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        sw.Write(dt.Columns[i]);
                        if (i < iColCount - 1)
                            sw.Write(",");
                    }

                    sw.Write(sw.NewLine);
                }


                // write all data to the rows.
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                            sw.Write(dr[i].ToString());
                        if (i < iColCount - 1)
                            sw.Write(",");
                    }
                    sw.Write(sw.NewLine);
                }

                sw.Flush();
                // Close the file
                sw.Close();

               return new MemoryStream(s.ToArray(), false /*writable*/);
            }
        }

        public static DataTable ListToDatatable<T>(IList<T> list)
        {             
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            
            DataTable table = new DataTable();            
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;    
        }

    }
}
