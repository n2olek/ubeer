using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.IO;



// ***** R E Q U I R E *******
// add 'DocumentFormat.OpenXml' to the references
// add WindowsBase 'WindowsBased' to the references


// OpenXML for exporting data to Excel 
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

// For getting class/object properties
using System.Reflection;


namespace S9.Utility
{
    public class SLExcelStatus
    {
        public string Message { get; set; }
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }

    public class SLExcelData
    {
        public SLExcelStatus Status { get; set; }
        public Columns ColumnConfigurations { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> DataRows { get; set; }
        public string SheetName { get; set; }

        public SLExcelData()
        {
            Status = new SLExcelStatus();
            Headers = new List<string>();
            DataRows = new List<List<string>>();
        }
    }

    public class ExcelReader
    {
        public SLExcelData Read(string file, string sheetName)
        {
            var data = new SLExcelData();

            // Check if the file is excel

            // Open the excel document
            WorkbookPart workbookPart; List<Row> rows;
            try
            {
                var document = SpreadsheetDocument.Open(file, false);
                workbookPart = document.WorkbookPart;

                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.First();
                data.SheetName = sheet.Name;

                var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                data.ColumnConfigurations = columns;

                var sheetData = workSheet.Elements<SheetData>().First();
                rows = sheetData.Elements<Row>().ToList();
            }
            catch
            {
                data.Status.Message = "Unable to open the file";
                return data;
            }

            // Read the header
            if (rows.Count > 0)
            {
                var row = rows[0];
                var cellEnumerator = GetExcelCellEnumerator(row);
                while (cellEnumerator.MoveNext())
                {
                    var cell = cellEnumerator.Current;
                    var text = ReadExcelCell(cell, workbookPart).Trim();
                    data.Headers.Add(text);
                }
            }

            // Read the sheet data
            if (rows.Count > 1)
            {
                for (var i = 1; i < rows.Count; i++)
                {
                    var dataRow = new List<string>();
                    data.DataRows.Add(dataRow);
                    var row = rows[i];
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    while (cellEnumerator.MoveNext())
                    {
                        var cell = cellEnumerator.Current;
                        var text = ReadExcelCell(cell, workbookPart).Trim();
                        dataRow.Add(text);
                    }
                }
            }

            return data;
        }


        private string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                // ASCII 'A' = 65
                int current = i == 0 ? letter - 65 : letter - 64;
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        private IEnumerator<Cell> GetExcelCellEnumerator(Row row)
        {
            int currentCount = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Cell()
                    {
                        DataType = null,
                        CellValue = new CellValue(string.Empty)
                    };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        private string ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            return (text ?? string.Empty).Trim();
        }

    }

    // to support type generic format (can be used with any type)
    public class ExcelGenerator<T>
    {
        // member variables
        private string _fileName;
        private string _sheetName;
        //private bool _isIO; // memeory
        private List<T> _data;
        
        // constructor
        public ExcelGenerator(string fileName, string sheetName, List<T> data)
        {
            // store data into the member variable
            _fileName = fileName;
            _sheetName = sheetName;
            _data = data;
        }

        public MemoryStream GetStream()
        {
            MemoryStream s = new MemoryStream();

            // open/load the file, prepare document
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(s, SpreadsheetDocumentType.Workbook);

            Save(spreadsheetDocument);
            spreadsheetDocument.Close();

            return s;
        }


        public bool Save()
        {
            bool bResult = false;

            // open/load the file, prepare document
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(_fileName, SpreadsheetDocumentType.Workbook);
            bResult = Save(spreadsheetDocument);
            spreadsheetDocument.Close();
            
            return bResult;
        }


        private bool Save(SpreadsheetDocument spreadsheetDocument)
        {
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();

            SheetData sd = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sd);

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = _sheetName
            };
            sheets.Append(sheet);

            // 2. write the header
            Row rowHeader = new Row();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int iColumn = propertyInfos.Count();
            for (int i = 0; i < iColumn; i++)
            {
                Cell cellH = new Cell();

                // split type and field name
                string[] strFields;
                string strFieldName = "";
                strFields = propertyInfos[i].ToString().Split(' ');

                if (strFields.Length > 1)
                    strFieldName = strFields[1];
                else
                    strFieldName = strFields[0];

                cellH.DataType = CellValues.InlineString;
                cellH.InlineString = new InlineString() { Text = new Text(strFieldName) };

                rowHeader.Append(cellH);
            }
            sd.Append(rowHeader);




            // 3. write record(s) of data
            foreach (T p in _data)
            {
                Row rowData = new Row(); //{ RowIndex = (UInt32Value)2U };

                foreach (PropertyInfo prop in propertyInfos)
                {
                    object propValue = prop.GetValue(p, null);

                    Cell cell = new Cell();

                    // split the field type and field name
                    string[] strFields;
                    string strFieldType = "";
                    string strFieldName = "";
                    strFields = prop.ToString().Split(' ');

                    if (strFields.Length > 1)
                    {
                        strFieldType = strFields[0];
                        strFieldName = strFields[1];
                    }
                    else
                        strFieldName = strFields[0];

                    if (strFieldType.ToLower().Contains("int"))
                    {
                        try
                        {
                            // int
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                            cell.CellValue = new CellValue(propValue.ToString());
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            // others - string, datetime, boolean
                            cell.DataType = CellValues.InlineString;
                            cell.InlineString = new InlineString() { Text = new Text(propValue.ToString()) };
                        }
                        catch (Exception)
                        {

                        }
                    }

                    rowData.Append(cell);
                }

                sd.Append(rowData);
            }



            // Close the document
            workbookpart.Workbook.Save();            
            return true;
        }    
    }
}
