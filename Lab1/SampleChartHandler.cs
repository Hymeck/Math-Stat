using System;
using ScottPlot;
using System.Data;
using System.Drawing;

namespace Lab1
{
    public class SampleChartHandler
    {
        double[] _values;
        double[] _accumulatedFrequencies;

        Color blueColor = Color.Blue;
        Color blackColor = Color.Black;
        MarkerShape puncturedPoint = MarkerShape.openCircle;
        const double markerSize = 6;
        const double lineWidth = 2;

        public SampleChartHandler(DataTable sampleTable)
        {
            int count = sampleTable.Rows.Count - 1;
            _values = new double[count];
            _accumulatedFrequencies = new double[count];

            for (int i = 0; i < count; i++)
            {
                var row = sampleTable.Rows[i].ItemArray;
                _values[i] = Convert.ToDouble(row[0]);
                _accumulatedFrequencies[i] = Convert.ToDouble(row[3]);
            }
        }

        public void PlotEmpirical(string fileName)
        {
            var plt = new Plot();
            int length = _values.Length;
            double min = _values[0], 
                   max = _values[length - 1], 
                   continious = Math.Abs(max - min) / 5;

            plt.PlotLine(-continious, 0, min, 0, blueColor, lineWidth: lineWidth);
            for (int i = 0; i < length - 1; i++)
            {
                double x = _values[i],
                       y = _accumulatedFrequencies[i];

                plt.PlotLine(x, y, _values[i + 1], y, blueColor, lineWidth: lineWidth);
                plt.PlotPoint(x, y, markerShape: puncturedPoint, color: blackColor, markerSize: markerSize);
            }
            plt.PlotPoint(max, 1, markerShape: puncturedPoint, color: blackColor, markerSize: markerSize);
            plt.PlotLine(max, 1, max + continious, 1, blueColor, lineWidth: lineWidth);

            plt.YLabel("F*(X)");
            plt.XLabel("X");

            plt.AxisAuto(0);
            plt.SaveFig(fileName);
        }
    }
}
