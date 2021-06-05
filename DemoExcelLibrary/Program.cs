using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DemoExcelLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var filePath = @"D:\HungTran\TOOL-1.xlsx";
            using ExcelReader excelReader = new ExcelReader(filePath);
            var headers = excelReader.GetHeaders(sheetName: "REPORT");
            Console.ReadKey();
        }

        public class ExcelReader:IDisposable {
            private ExcelPackage excelPackage;
            public ExcelReader(string filePath)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                excelPackage = new ExcelPackage(new FileInfo(filePath));
            }
            public IList<string> GetHeaders(string sheetName)
            {
                var listHeaders = new List<string>();
                var workSheets = excelPackage.Workbook.Worksheets;
                ExcelWorksheet workSheet = workSheets.FirstOrDefault(_ => _.Name.Trim().ToLower() == sheetName.ToLower());
                if (workSheet == null) return listHeaders;

                int totalRow = workSheet.Dimension.Rows;
                int totalColumn = workSheet.Dimension.Columns;

                for (int column = 1; column < totalColumn; column++)
                {
                    listHeaders.Add(workSheet.Cells[1, column].Value + "");
                }
                return listHeaders;
            }
            public void Dispose()
            {
                excelPackage.Dispose();
            }
        }
    }
}
