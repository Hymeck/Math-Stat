using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace Labs
{
    public class BarChartHandler
    {
        SampleHandler _sampleHandler;
        public BarChartHandler(SampleHandler sampleHandler)
        {
            _sampleHandler = sampleHandler;
        }

        public void Plot()
        {
            int volume = _sampleHandler.Volume;
            double[] samplePoints = _sampleHandler.SamplePoints;
            int intervalCount = volume <= 100 ? 
                                (int)Math.Sqrt(volume) : 
                                (int)(4 * Math.Log10(volume));

            double[] leftBounds = new double[intervalCount],
                     rightBounds = new double[intervalCount],
                     differences = new double[intervalCount],
                     densityValues = new double[intervalCount];

            leftBounds[0] = samplePoints[0]; // A_0

            double factor = (double)intervalCount / volume;
            for (int i = 1; i < intervalCount; i++)
            {
                int j = i - 1;
                double leftBound = (samplePoints[(i * intervalCount) - 1] +
                                    samplePoints[(i * intervalCount)]) / 2;

                rightBounds[j] = leftBounds[i] = leftBound;
                differences[j] = leftBound - leftBounds[j];
                densityValues[j] = factor / differences[j];
            }

            rightBounds[intervalCount - 1] = samplePoints[volume - 1];
            differences[intervalCount - 1] = rightBounds[intervalCount - 1] - leftBounds[intervalCount - 1];
            densityValues[intervalCount - 1] = factor / differences[intervalCount - 1];

            bool test = true;
        }
    }
}
