using System;
using System.Collections.Generic;

namespace Labs
{
    public class HistogramData
    {
        double[] _samplePoints;

        public int Volume => _samplePoints.Length;
        public int IntervalCount => _samplePoints.Length <= 100 ?
                                    (int)Math.Sqrt(_samplePoints.Length) :
                                    (int)(4 * Math.Log10(_samplePoints.Length));

        public int ElementsPerInterval => _samplePoints.Length / IntervalCount;

        public HistogramData(SampleHandler sampleHandler)
        {
            _samplePoints = sampleHandler.SamplePoints;
        }

        public List<double[]> Data
        {
            get
            {
                int volume = _samplePoints.Length; // n
                int intervalCount = IntervalCount; // M

                double[] leftBounds = new double[intervalCount], // A
                         rightBounds = new double[intervalCount], // B
                         differences = new double[intervalCount], // h
                         densityValues = new double[intervalCount]; // f*

                leftBounds[0] = _samplePoints[0]; // A_0

                int elementsPerInterval = ElementsPerInterval, j, index;
                double factor = (double)1 / intervalCount; // in f*_i formula: v_i/n; v_i/n = (n/M)/ n = 1 / M
                for (int i = 1; i < intervalCount; i++) // i = 1..M - 1
                {
                    index = i * elementsPerInterval;
                    double leftBound = (_samplePoints[index - 1] +
                                        _samplePoints[index]) / 2; // A_i = (x[(n/M) - 1] + x[n/M]) / 2

                    j = i - 1;
                    rightBounds[j] = leftBounds[i] = leftBound; // B_i-1 = A_i
                    differences[j] = leftBound - leftBounds[j]; // h_i-1 = B_i-1 - A_i-1
                    densityValues[j] = factor / differences[j]; // f*_i-1 = 1 / (M * h_i-1)
                }

                intervalCount--; // just for simplicity to set values to last elements of B, h, f*

                rightBounds[intervalCount] = _samplePoints[volume - 1]; // B_last = x_last;
                differences[intervalCount] = rightBounds[intervalCount] - leftBounds[intervalCount]; // h_i_last
                densityValues[intervalCount] = factor / differences[intervalCount]; // f*_i_last

                var data = new List<double[]>(4)
                {
                    leftBounds,
                    rightBounds,
                    differences,
                    densityValues
                };

                return data;
            }
        }
    }
}
