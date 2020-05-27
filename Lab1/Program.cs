using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;

namespace Lab1
{
    class Program
    {
        // Y = cos(x); a = -pi/2; b = pi/2; y_0 = 0.5;
        // f(y) - ?; write f(y_0)
        // http://www.alexeypetrov.narod.ru/C/r_gener_about.html
        // https://habr.com/ru/post/263993/
        static double pi = Math.PI;

        static void Main(string[] args)
        {
            var sampleHandler = new SampleHandler(
                new VariableGenerator(-pi / 2, pi / 2, 30), 
                x => Math.Cos(x));

            TableHandler sampleInfo = new TableHandler("Sample table: cos(Y_i)");
            sampleInfo.SetTable(
                new string[] { "Y_i", "n_i", "w_i", "w_a"},
                sampleHandler.ToTableRows());

            sampleInfo.DrawTable("Table.pdf");
            sampleInfo.OpenTable("Table.pdf");
        }

    }
}
