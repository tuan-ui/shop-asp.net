using Data.Models;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Web.Common
{
    public static class ReportHelper
    {
        public static async Task GeneratePdf(string html, string filePath)
        {
            await Task.Run(() =>
            {
                using (FileStream ms = new FileStream(filePath, FileMode.Create))
                {
                    var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                    pdf.Save(ms);
                }
            });
        }
        public static Task GenerateXlsProduct<T>(List<T> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    ws.Cells[1, 1].Value = "From Date:";
                    ws.Cells["A1"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    var properties = typeof(T).GetProperties();
                    foreach (var prop in properties)
                    {
                        int i = 0;
                        if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                        {
                            ws.Column(i + 1).Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                        }
                        i++;
                    }
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }
        public static Task GenerateXlsOrder<T>(List<T> datasource, string filePath, DateTime fromDate, DateTime toDate)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Thống kê doanh thu");
                    ws.Cells["F1"].Value = "Thống kê doanh thu";
                    ws.Cells["F1"].Style.Font.Size = 16;
                    ws.Cells["F1"].Style.Font.Bold = true;
                    ws.Cells["B3"].Value = "Từ ngày:";
                    ws.Cells["C3"].Value = fromDate.ToString("dd/MM/yyyy");
                    ws.Cells["B4"].Value = "Đến ngày:";
                    ws.Cells["C4"].Value = toDate.ToString("dd/MM/yyyy");
                    ws.Cells["A6"].LoadFromCollection(datasource, true, TableStyles.Light1);

                    var properties = typeof(T).GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (properties[i].PropertyType == typeof(DateTime) || properties[i].PropertyType == typeof(DateTime?))
                        {
                            ws.Column(i + 1).Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                        }
                    }
                    ws.Cells.AutoFitColumns();
                    int totalRow = datasource.Count + 7;
                    var totalPriceProperty = properties.FirstOrDefault(p => p.Name == "TotalPrice");
                    if (totalPriceProperty != null)
                    {
                        var totalPrice = datasource.Sum(item => (decimal?)totalPriceProperty.GetValue(item) ?? 0);
                        ws.Cells[totalRow, 11].Value = "Tổng tiền:";
                        ws.Cells[totalRow, 12].Value = totalPrice;
                    }
                    ws.Cells[totalRow + 2, 9].Value = "Thành phố Hồ Chí Minh, ngày ... tháng ... năm 2024";
                    ws.Cells[totalRow + 3, 10].Value = "Người xuất hóa đơn";
                    pck.Save();
                }
            });
        }
    }
}