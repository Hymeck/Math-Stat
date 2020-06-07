using ScottPlot;
using System;
using System.Drawing;
using System.Linq;

namespace Labs
{
    public class HistogramHandler
    {
        Func<double, double> _analiticalDensityFunction = x => 2 / (Math.PI * Math.Sqrt(1 - (x * x)));

        HistogramData _histogramData;

        Color fillColor = Color.Blue;
        Color lineColor = Color.Black;

        public HistogramHandler(HistogramData histogramData)
        {
            _histogramData = histogramData;
        }

        public void Plot(string fileName)
        {
            // 0 - leftBounds; 1 - rightBounds, 2 - differences; 3 - densityValues
            var data = _histogramData.Data;

            var plt = new Plot();
            double[] leftBounds = data[0],
                     rightBounds = data[1],
                     differences = data[2],
                     densityValues = data[3];

            int dataLength = leftBounds.Length;
            for (int i = 0; i < dataLength; i++)
            {
                double middle = (rightBounds[i] + leftBounds[i]) / 2;
                plt.PlotBar(new double[] { middle },
                            new double[] { densityValues[i] },
                            barWidth: differences[i],
                            fillColor: fillColor,
                            outlineColor: lineColor,
                            outlineWidth: 1);
            }

            double[] xs = DataGen.Range(leftBounds[0], rightBounds[dataLength - 1], 0.01, true);
            double[] analyticalYs = xs.Select(x => _analiticalDensityFunction(x)).ToArray();
            plt.PlotScatter(xs, analyticalYs, Color.Green, lineWidth: 2, markerSize: 0, label: "Analytical f(Y)");
            
            plt.Axis(y1: 0);
            plt.XLabel("Y = cos(X)");
            plt.YLabel("f*(Y)");
            plt.Legend(location: legendLocation.upperLeft);

            plt.SaveFig(fileName);
        }
    }
}
