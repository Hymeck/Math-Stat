using System;

namespace Labs
{
    public class KolmogorovTest
    {
        static double kolmogorovLambda = 1.34;
        int _volume;
        public double MaxAbsDifference { get; private set; }

        public double KolmogorovStatistic { get; private set; }

        public KolmogorovTest(double[] accumulatedFrequencies, double[] analyticalFrequencies, int volume)
        {
            MaxAbsDifference = FindMaxDifference(analyticalFrequencies);
            _volume = volume;
            KolmogorovStatistic = Math.Sqrt(_volume) * MaxAbsDifference;
        }

        double FindMaxDifference(double[] analyticalFrequencies)
        {
            int volume = analyticalFrequencies.Length;
            double maxAbsDifference = 0;

            for (int i = 0; i < volume; i++)
            {
                double differencePlus = Math.Abs(((double)i / volume) - analyticalFrequencies[i]),
                       differenceMinus = Math.Abs(analyticalFrequencies[i] - (((double)i - 1) / volume)),
                       tempMax = differenceMinus > differencePlus ? differenceMinus : differencePlus;

                if (tempMax > maxAbsDifference)
                    maxAbsDifference = tempMax;
            }

            return maxAbsDifference;
        }

        public bool IsConfirmed => KolmogorovStatistic <= kolmogorovLambda ? true : false;
    }
}
