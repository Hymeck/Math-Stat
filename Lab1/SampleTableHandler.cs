using System.Data;
using Syncfusion.Pdf.Tables;
using Syncfusion.Pdf;
using System.Drawing;
using System.Diagnostics;
using System.Linq;

namespace Lab1
{
    public class SampleTableHandler
    {
        string _tableName;
        DataTable _table;
        public DataTable Table => _table.Copy();

        public SampleTableHandler(string tableName = null)
        {
            _tableName = tableName;
            _table = new DataTable(_tableName);
        }

        public void Set(string[] headers, object[,] data)
        {
            for (int i = 0; i < headers.Length; i++)
                _table.Columns.Add(headers[i]);

            for (int i = 0; i < data.GetLength(0); i++)
            {
                object[] row = Enumerable.Range(0, data.GetLength(1))
                                         .Select(x => data[i, x])
                                         .ToArray();
                _table.Rows.Add(row);
            }
        }

        public void Draw(string fileName)
        {
            PdfLightTable pdfLightTable = new PdfLightTable { DataSource = _table };

            pdfLightTable.ApplyBuiltinStyle(PdfLightTableBuiltinStyle.GridTable1Light);
            pdfLightTable.Style.CellPadding = 3;
            pdfLightTable.Style.ShowHeader = true;

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            pdfLightTable.Draw(page, new PointF(0, 0));

            doc.Save(fileName);
            doc.Close(true);
        }
    }
}
