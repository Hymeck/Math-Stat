using System;

namespace Labs
{
    public class MisesCriterion
    {
        // alpha = 0.01
        public double _mises = 0.744;

        public double MisesStatistic { get; private set; }

        public MisesCriterion(double[] analyticalFrequencies)
        {
            MisesStatistic = ComputeStatistic(analyticalFrequencies);
        }

        double ComputeStatistic(double[] analyticalFrequencies)
        {
            int volume = analyticalFrequencies.Length;
            double statistic = 1 / (12 * volume);

            for (int i = 0; i < volume; i++)
                statistic += Math.Pow(analyticalFrequencies[i] - ((i - 0.5) / volume), 2);

            return statistic;
        }

        public bool IsConfirmed => MisesStatistic <= _mises ? true : false;
    }
}
