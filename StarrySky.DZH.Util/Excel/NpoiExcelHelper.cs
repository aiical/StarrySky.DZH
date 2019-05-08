using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Excel
{
    public class NpoiExcelHelper
    {
        /// <summary>
        /// 只支持属性全是string的T对象（防止类型转换异常）
        /// excel第一行需为列名
        /// </summary>
        /// <typeparam name="T">只支持属性全是string的T对象（防止类型转换异常）</typeparam>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static List<T> ExportToList<T>(string fileName, Stream stream, string sheetName) where T : class, new()
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            try
            {
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                {
                    workbook = new XSSFWorkbook(stream);
                }
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                {
                    workbook = new HSSFWorkbook(stream);
                }
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }

                var t = new T();
                var properties = t.GetType().GetProperties();
                var fields = properties.Select(x => x.Name).ToArray();

                List<T> list = new List<T>();
                //遍历每一行数据
                for (int i = sheet.FirstRowNum + 1, len = sheet.LastRowNum + 1; i < len; i++)
                {
                    t = new T();
                    IRow row = sheet.GetRow(i);

                    for (int j = 0, len2 = fields.Length; j < len2; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null) break;
                        cell.SetCellType(CellType.String);
                        if (cell.StringCellValue != null)
                        {
                            //stuUser.setPhone(row.getCell(0).getStringCellValue());
                            var cellValue = cell.StringCellValue.Trim();
                            typeof(T).GetProperty(fields[j])?.SetValue(t, cellValue, null);
                        }
                        //string cellValue = null;
                        //switch (cell.CellType)
                        //{
                        //    case CellType.String: //文本
                        //        cellValue = cell.StringCellValue.Trim();
                        //        break;
                        //    case CellType.Numeric: //数值
                        //        cellValue = cell.NumericCellValue.ToString().Trim();
                        //        break;
                        //    case CellType.Boolean: //bool
                        //        cellValue = cell.BooleanCellValue.ToString().Trim();
                        //        break;
                        //    case CellType.Blank: //空白
                        //        cellValue = "";
                        //        break;
                        //    default:
                        //        cellValue = "ERROR";
                        //        break;
                        //}
                        //if (cellValue == "ERROR")
                        //{
                        //    return null;
                        //}
                        //typeof(T).GetProperty(firstRow.GetCell(j).StringCellValue).SetValue(t, cellValue, null);
                    }
                    list.Add(t);
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// 转list到execl
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static NpoiMemoryStream ListToExecl<T>(List<T> list) where T : class, new()
        {
            var workbook = new XSSFWorkbook();
            workbook.CreateSheet("Sheet1");
            var sheetOne = workbook.GetSheet("Sheet1"); //获取名称为Sheet1的工作表

            var sheetRow0 = sheetOne.CreateRow(0);  //获取Sheet1工作表的首行
            var properties0 = new T().GetType().GetProperties();
            for (var j = 0; j < properties0.Length; j++)
            {
                var col = sheetRow0.CreateCell(j);
                col.SetCellValue(properties0[j].Name);
                sheetRow0.Cells.Add(col);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                var sheetRow = sheetOne.CreateRow(i + 1);
                var properties = item.GetType().GetProperties();
                for (var j = 0; j < properties.Length; j++)
                {
                    var col = sheetRow.CreateCell(j);
                    col.SetCellValue(properties[j].GetValue(item, null)?.ToString() ?? "");
                    sheetRow.Cells.Add(col);
                }
            }
            var ms = new NpoiMemoryStream
            {
                AllowClose = false
            };
            workbook.Write(ms);
            return ms;
        }


        #region DataTable
        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        //public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        //{
        //    int i = 0;
        //    int j = 0;
        //    int count = 0;
        //    try
        //    {
        //        if (workbook != null)
        //        {
        //            sheet = workbook.CreateSheet(sheetName);
        //        }
        //        else
        //        {
        //            return -1;
        //        }

        //        if (isColumnWritten == true) //写入DataTable的列名
        //        {
        //            IRow row = sheet.CreateRow(0);
        //            for (j = 0; j < data.Columns.Count; ++j)
        //            {
        //                row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
        //            }
        //            count = 1;
        //        }
        //        else
        //        {
        //            count = 0;
        //        }

        //        for (i = 0; i < data.Rows.Count; ++i)
        //        {
        //            IRow row = sheet.CreateRow(count);
        //            for (j = 0; j < data.Columns.Count; ++j)
        //            {
        //                row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
        //            }
        //            ++count;
        //        }
        //        //workbook.Write(fs); //写入到excel
        //        return count;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //        return -1;
        //    }
        //}

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        //public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        //{
        //    DataTable data = new DataTable();
        //    int startRow = 0;
        //    try
        //    {
        //        if (sheet != null)
        //        {
        //            IRow firstRow = sheet.GetRow(0);
        //            int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

        //            if (isFirstRowColumn)
        //            {
        //                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
        //                {
        //                    ICell cell = firstRow.GetCell(i);
        //                    if (cell != null)
        //                    {
        //                        string cellValue = cell.StringCellValue;
        //                        if (cellValue != null)
        //                        {
        //                            DataColumn column = new DataColumn(cellValue);
        //                            data.Columns.Add(column);
        //                        }
        //                    }
        //                }
        //                startRow = sheet.FirstRowNum + 1;
        //            }
        //            else
        //            {
        //                startRow = sheet.FirstRowNum;
        //            }

        //            //最后一列的标号
        //            int rowCount = sheet.LastRowNum;
        //            for (int i = startRow; i <= rowCount; ++i)
        //            {
        //                IRow row = sheet.GetRow(i);
        //                if (row == null) continue; //没有数据的行默认是null　　　　　　　

        //                DataRow dataRow = data.NewRow();
        //                for (int j = row.FirstCellNum; j < cellCount; ++j)
        //                {
        //                    if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
        //                        dataRow[j] = row.GetCell(j).ToString();
        //                }
        //                data.Rows.Add(dataRow);
        //            }
        //        }

        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //        return null;
        //    }
        //}

        #endregion
    }

    /// <summary>
    /// 支持Npoi到内存流的直接操作
    /// </summary>
    public class NpoiMemoryStream : MemoryStream
    {
        /// <summary>
        /// 
        /// </summary>
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        /// <summary>
        /// 总是关闭的
        /// </summary>
        public bool AllowClose { get; set; }

        /// <summary>
        /// 手动关闭
        /// </summary>
        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}
