using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    public class SampleHandler
    {
        Function _function;

        double[] _samplePoints;
        public double[] SamplePoints
        {
            get
            {
                double[] samplePoints = new double[_samplePoints.Length];
                _samplePoints.CopyTo(samplePoints, 0);
                return samplePoints;
            }

            private set
            {
                double[] samplePoints = value.Select(x => Math.Round(_function(x), 1)).ToArray();
                Array.Sort(samplePoints);
                _samplePoints = samplePoints;
            }
        }

        Dictionary<double, int> _sample;
        public Dictionary<double, int> Sample => new Dictionary<double, int>(_sample);

        void SetSample()
        {
            int volume = _samplePoints.Length;
            Dictionary<double, int> sample = new Dictionary<double, int>(volume);

            for (int i = 0; i < volume; i++)
            {
                double samplePoint = _samplePoints[i];
                if (!sample.TryGetValue(samplePoint, out int count))
                    sample[samplePoint] = 1;
                else
                    sample[samplePoint]++;
            }

            _sample = sample;
        }

        public SampleHandler(VariableGenerator arguments, Function function)
        {
            _function = function;
            SamplePoints = arguments.GetVariables();
            SetSample();
        }

        public SampleHandler(double[] arguments, Function function)
        {
            _function = function;
            SamplePoints = arguments;
            SetSample();
        }

        //public double[] GetRelativeFrequencies()
        //{
        //    double[] relativeFrequencies = new double[SamplePoints.Length]
        //}

        public string[,] ToTableRows()
        {
            int volume = _samplePoints.Length;
            int size = _sample.Count;
            string[,] rows = new string[size + 1, 4];
            int[] counts = _sample.Values.ToArray();
            double[] points = _sample.Keys.ToArray();

            int roundDigits = 5;

            for (int i = 0; i < size; i++)
            {
                double count = counts[i], 
                       relativeFrequency = Math.Round(count / volume, roundDigits),
                       accumulatedFrequency = relativeFrequency;

                if (i != 0)
                    accumulatedFrequency += Convert.ToDouble(rows[i - 1, 3]);

                rows[i, 0] = points[i].ToString();
                rows[i, 1] = count.ToString();
                rows[i, 2] = relativeFrequency.ToString();
                rows[i, 3] = accumulatedFrequency.ToString();
            }

            rows[size - 1, 3] = Math.Round(Convert.ToDouble(rows[size - 1, 3])).ToString();

            rows[size, 0] = "SUM:";
            rows[size, 1] = volume.ToString();
            rows[size, 2] = "1";
            rows[size, 3] = " ";

            return rows;
        }

        public delegate double Function(double x);
    }
}
