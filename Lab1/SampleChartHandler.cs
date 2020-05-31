using System;
using ScottPlot;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Lab1
{
    public class SampleChartHandler
    {
        Func<double, double> _analyticalFunction = x => 1 - (2 * Math.Acos(x) / Math.PI);

        double[] _analyticalValues;

        double[] _empiricalValues;
        double[] _accumulatedFrequencies;

        Color blueColor = Color.Blue;
        Color blackColor = Color.Black;
        MarkerShape puncturedPoint = MarkerShape.openCircle;
        const double markerSize = 6;
        const double lineWidth = 2;

        public SampleChartHandler(DataTable sampleTable, double[] arguments, Func<double, double> function)
        {
            int count = sampleTable.Rows.Count - 1;
            _empiricalValues = new double[count];
            _accumulatedFrequencies = new double[count];

            for (int i = 0; i < count; i++)
            {
                var row = sampleTable.Rows[i].ItemArray;
                _empiricalValues[i] = Convert.ToDouble(row[0]);
                _accumulatedFrequencies[i] = Convert.ToDouble(row[3]);
            }

            _analyticalValues = new double[arguments.Length];
            _analyticalValues = arguments.Select(x => function(x)).ToArray();
            Array.Sort(_analyticalValues);
        }

        public void Plot(string fileName)
        {
            var plt = new Plot();
            int length = _empiricalValues.Length;
            double min = _empiricalValues[0], 
                   max = _empiricalValues[length - 1], 
                   continious = Math.Abs(max - min) / 5;

            plt.PlotLine(-continious, 0, min, 0, blueColor, lineWidth: lineWidth, label: "Empirical F(Y)");
            for (int i = 0; i < length - 1; i++)
            {
                double x = _empiricalValues[i],
                       y = _accumulatedFrequencies[i];

                plt.PlotLine(x, y, _empiricalValues[i + 1], y, blueColor, lineWidth: lineWidth);
                plt.PlotPoint(x, y, markerShape: puncturedPoint, color: blackColor, markerSize: markerSize);
            }
            plt.PlotPoint(max, 1, markerShape: puncturedPoint, color: blackColor, markerSize: markerSize);
            plt.PlotLine(max, 1, max + continious, 1, blueColor, lineWidth: lineWidth);


            double[] analyticalYs = _analyticalValues.Select(x => _analyticalFunction(x)).ToArray();
            plt.PlotScatter(_analyticalValues, analyticalYs, Color.Green, lineWidth, 0, label:"Analytical F(Y)");

            plt.YLabel("F(Y)");
            plt.XLabel("Y = cos(X)");
            plt.Legend();

            plt.AxisAuto(0);
            plt.SaveFig(fileName);
        }
    }
}
