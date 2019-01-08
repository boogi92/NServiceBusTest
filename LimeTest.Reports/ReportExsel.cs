using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;

namespace LimeTest.Reports
{
    public class ReportExcel<T>
    {
        private IEnumerable<T> Report { get; }

        public ReportExcel()
        {
            Report = null;
        }


        public ReportExcel(IEnumerable<T> report)
        {
            this.Report = report;
        }


        /// <summary>
        /// Получение 
        /// </summary>
        /// <returns></returns>
        public Stream GetExcelStream()
        {
            try
            {
                using (var package = new ExcelPackage())
                {
                    Filling(package, Report);

                    return new MemoryStream(package.GetAsByteArray());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveAs(string path)
        {
            try
            {
                var existingFile = new FileInfo(path);
                if (existingFile.Exists)
                    existingFile.Delete();

                using (var package = new ExcelPackage(existingFile))
                {
                    Filling(package, Report);

                    package.Save();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveAs(string path, Stream stream)
        {
            try
            {
                var existingFile = new FileInfo(path);
                if (existingFile.Exists)
                    existingFile.Delete();

                using (var package = new ExcelPackage(stream))
                {
                    package.SaveAs(existingFile);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public Stream AddRowsToStream(Stream stream)
        {
            try
            {
                using (var package = new ExcelPackage(stream))
                {
                    FillingAddRaws(package, Report);

                    return new MemoryStream(package.GetAsByteArray());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void SaveAs(string path, IEnumerable<T> value)
        {
            try
            {
                var existingFile = new FileInfo(path);
                if (existingFile.Exists)
                    existingFile.Delete();
                using (var package = new ExcelPackage(existingFile))
                {
                    if (existingFile.Exists)
                    {
                        FillingAddRaws(package, value);
                    }
                    else
                    {
                        Filling(package, value);
                    }

                    package.Save();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static DataTable ListProp()
        {
            try
            {
                DataTable dataSource = new DataTable();

                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    dataSource.Columns.Add(info.GetCustomAttribute<DisplayAttribute>().Name);
                }

                return dataSource;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void Filling(ExcelPackage package, IEnumerable<T> report)
        {
            try
            {
                var ws = package.Workbook.Worksheets.Add("Лист1");
                ws.Cells["A1"].LoadFromDataTable(ListProp(), true);
                FillingAddRaws(package, report);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void FillingAddRaws(ExcelPackage package, IEnumerable<T> report)
        {
            try
            {
                var ws = package.Workbook.Worksheets.FirstOrDefault();
                var lastRow = ws.Cells.Max(x => x.End.Row) + 1;
                ws.Cells[$"A{lastRow}"].LoadFromCollection(report);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}