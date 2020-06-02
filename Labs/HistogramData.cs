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
                int volume = _samplePoints.Length;
                int intervalCount = IntervalCount;

                double[] leftBounds = new double[intervalCount],
                         rightBounds = new double[intervalCount],
                         differences = new double[intervalCount],
                         densityValues = new double[intervalCount];

                leftBounds[0] = _samplePoints[0]; // A_0

                int elementsPerInterval = ElementsPerInterval;
                double factor = (double)1 / intervalCount;
                for (int i = 1; i < intervalCount; i++)
                {
                    int j = i - 1,
                        index = i * elementsPerInterval;
                    double leftBound = (_samplePoints[index - 1] +
                                        _samplePoints[index]) / 2;

                    rightBounds[j] = leftBounds[i] = leftBound;
                    differences[j] = leftBound - leftBounds[j];
                    densityValues[j] = factor / differences[j];
                }

                intervalCount--;

                rightBounds[intervalCount] = _samplePoints[volume - 1];
                differences[intervalCount] = rightBounds[intervalCount] - leftBounds[intervalCount];
                densityValues[intervalCount] = factor / differences[intervalCount];

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
