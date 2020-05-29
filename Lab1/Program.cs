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
        static double pi = Math.PI;

        static void Main(string[] args)
        {
            var sampleHandler = new SampleHandler(
                new VariableGenerator(-pi / 2, pi / 2, 30), 
                x => Math.Cos(x));

            SampleTableHandler sampleTable = new SampleTableHandler("Sample table");
            sampleTable.Set(
                new string[] { "Y_i", "n_i", "w_i", "w_a"},
                sampleHandler.ToTableRows());

            sampleTable.Draw("Sample_Table.pdf");
            //sampleTable.Open("Table.pdf");
            var chartHandler = new SampleChartHandler(sampleTable.Table);
            chartHandler.PlotEmpirical("SampleChart_Empirical.png");
            chartHandler.PlotAnalitycal("SampleChart_Analitycal.png");
        }

    }
}
