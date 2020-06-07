using System;

namespace Labs
{
    public class ChiSquaredTest
    {
        static int _tableRoundDigits = 6;
        // alpha = 0.01;
        // freedomPower = 8;
        static double chiSquared = 20.10; // chi_0.01,8 = 20.10

        Func<double, double> _analyticalDistributionFunction = x => 1 - (2 * Math.Acos(x) / Math.PI);
        HistogramData _histogramData;

        public ChiSquaredTest(HistogramData histogramData)
        {
            _histogramData = histogramData;
        }
        public double ChiSquaredStatistic { get; private set; }

        public object[,] ToTableRows()
        {
            var histogramData = _histogramData.Data;

            int intervalCount = _histogramData.IntervalCount,
                volume = _histogramData.Volume;

            double probability = (double)_histogramData.ElementsPerInterval / volume; // p*_i

            double[] leftBounds = histogramData[0],
                     rightBounds = histogramData[1];

            object[,] rows = new object[intervalCount + 1, 6];

            double chiSquared = 0;
            for (int i = 0; i < intervalCount; i++)
            {
                double leftProbability = _analyticalDistributionFunction(leftBounds[i]),
                       rightProbability = _analyticalDistributionFunction(rightBounds[i]),
                       analyticalProbability = rightProbability - leftProbability,
                       chiSquaredPart = volume * (analyticalProbability - probability) *
                                        (analyticalProbability - probability) / analyticalProbability;

                chiSquared += chiSquaredPart;

                rows[i, 0] = i;
                rows[i, 1] = Math.Round(leftProbability, _tableRoundDigits);
                rows[i, 2] = Math.Round(rightProbability, _tableRoundDigits);
                rows[i, 3] = Math.Round(analyticalProbability, _tableRoundDigits);
                rows[i, 4] = Math.Round(probability, _tableRoundDigits);
                rows[i, 5] = Math.Round(chiSquaredPart, _tableRoundDigits);
            }
            ChiSquaredStatistic = chiSquared;

            rows[intervalCount, 5] = "Chi-squared: " + Math.Round(chiSquared, _tableRoundDigits).ToString();

            return rows;
        }

        public bool IsConfirmed => ChiSquaredStatistic <= chiSquared ? true : false;
    }
}
