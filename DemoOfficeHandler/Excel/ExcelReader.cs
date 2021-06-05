using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoOfficeHandler.Excel
{
    public static class StringUtils
    {
        private static string DuplicateTicksForSql(this string s)
        {
            return s.Replace("'", "''");
        }

        /// <summary>
        /// Takes a List collection of string and returns a delimited string.  Note that it's easy to create a huge list that won't turn into a huge string because
        /// the string needs contiguous memory.
        /// </summary>
        /// <param name="list">The input List collection of string objects</param>
        /// <param name="qualifier">
        /// The default delimiter. Using a colon in case the List of string are file names,
        /// since it is an illegal file name character on Windows machines and therefore should not be in the file name anywhere.
        /// </param>
        /// <param name="insertSpaces">Whether to insert a space after each separator</param>
        /// <returns>A delimited string</returns>
        /// <remarks>This was implemented pre-linq</remarks>
        public static string ToDelimitedString(this List<string> list, string delimiter = ":", bool insertSpaces = false, string qualifier = "", bool duplicateTicksForSQL = false)
        {
            var result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string initialStr = duplicateTicksForSQL ? list[i].DuplicateTicksForSql() : list[i];
                result.Append((qualifier == string.Empty) ? initialStr : string.Format("{1}{0}{1}", initialStr, qualifier));
                if (i < list.Count - 1)
                {
                    result.Append(delimiter);
                    if (insertSpaces)
                    {
                        result.Append(' ');
                    }
                }
            }
            return result.ToString();
        }
    }
    public class ExcelReader : IDisposable
    {
        private ExcelPackage excelPackage;
        public ExcelReader(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            excelPackage = new ExcelPackage(new FileInfo(filePath));
        }
        public IList<string> GetHeaders(string sheetName)
        {
            var listHeaders = new List<string>();
            ExcelWorksheet workSheet = GetSheet(sheetName);
            if (workSheet == null) return listHeaders;

            int totalRow = workSheet.Dimension.Rows;
            int totalColumn = workSheet.Dimension.Columns;

            for (int column = 1; column < totalColumn; column++)
            {
                listHeaders.Add(workSheet.Cells[1, column].Value + "");
            }
            return listHeaders;
        }

        public void ConvertToCsv(string sheetName, string targetFile)
        {
            ExcelWorksheet workSheet = GetSheet(sheetName);
            if (workSheet == null) return;

            var maxColumnNumber = workSheet.Dimension.End.Column;
            var currentRowData = new List<string>(maxColumnNumber);
            var totalRowCount = workSheet.Dimension.End.Row;
            var currentRowNum = 1;

            using (var writer = new StreamWriter(targetFile, false, Encoding.UTF8))
            {
                //the rest of the code remains the same
                while (currentRowNum <= totalRowCount)
                {
                    currentRowData = BuildRow(workSheet, currentRowNum, maxColumnNumber);
                    WriteRecordToFile(currentRowData, writer, currentRowNum, totalRowCount);
                    currentRowData.Clear();
                    currentRowNum++;
                }
            } 
        }

        public void Dispose()
        {
            excelPackage.Dispose();
        }

        #region helper-method
        private ExcelWorksheet GetSheet(string sheetName)
        {
            var workSheets = excelPackage.Workbook.Worksheets;
            ExcelWorksheet workSheet = workSheets.FirstOrDefault(_ => _.Name.Trim().ToLower() == sheetName.ToLower());
            return workSheet;
        }

        private static void WriteRecordToFile(List<string> record, StreamWriter sw, int rowNumber, int totalRowCount)
        {
            var commaDelimitedRecord = record.ToDelimitedString(",");

            if (rowNumber == totalRowCount)
            {
                sw.Write(commaDelimitedRecord);
            }
            else
            {
                sw.WriteLine(commaDelimitedRecord);
            }
        }
        private static List<string> BuildRow(ExcelWorksheet worksheet, int currentRowNum, int maxColumnNumber)
        {
            var rowData = new List<string>(maxColumnNumber);
            for (int i = 1; i <= maxColumnNumber; i++)
            {
                var cell = worksheet.Cells[currentRowNum, i];
                if (cell == null)
                {
                    // add a cell value for empty cells to keep data aligned.
                    AddCellValue(string.Empty, rowData);
                }
                else
                {
                    AddCellValue(GetCellText(cell), rowData);
                }
            }

            return rowData;
        }
        private static string GetCellText(ExcelRangeBase cell)
        {
            return cell.Value == null ? string.Empty : cell.Value.ToString();
        }

        private static void AddCellValue(string s, List<string> record)
        {
            record.Add(string.Format("{0}{1}{0}", '"', s));
        }
        #endregion

    }
}
