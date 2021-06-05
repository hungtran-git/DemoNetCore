using DemoOfficeHandler.Excel;
using System;

namespace DemoOfficeHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"D:\HungTran\TOOL-1.xlsx";
            using var excelReader = new ExcelReader(filePath);

            var csvOutput = @"D:\HungTran\Report.csv";
            excelReader.ConvertToCsv("report", csvOutput);
            Console.ReadKey();
        }
    }
}
