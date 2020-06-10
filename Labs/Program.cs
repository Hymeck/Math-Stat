using System;
using System.Linq;
using System.Collections.Generic;
using ScottPlot;

namespace Labs
{
    class Program
    {
        static double pi = Math.PI;

        // Y = cos(X); a = -pi / 2; b = pi / 2;
        static double leftBound = -pi / 2;
        static double rightBound = pi / 2;
        static void Main(string[] args)
        {
            //Lab1();
            //Lab3ChiSquared();
            //Lab3Kolmogorov();
            //Lab3Mises();
            Lab4Task1();
        }

        static void Lab1()
        {
            uint volume;
            Console.Write("Volume: ");
            while (!uint.TryParse(Console.ReadLine(), out volume))
            {
                Console.WriteLine("Incorrect input. Try again.");
                Console.Write("Volume: ");
            }

            double[] arguments = new VariableGenerator(-pi / 2, pi / 2, volume).GetVariables();
            var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x));

            var sampleTable = new TableHandler();
            sampleTable.Set(new string[] { "Y_i", "n_i", "w_i", "w_a" },
                            sampleHandler.ToTableRows());
            sampleTable.Draw("SampleTable.pdf");

            var chartHandler = new SampleChartHandler(sampleTable.Table,
                                                      arguments,
                                                      sampleHandler.Function);
            chartHandler.Plot("SampleChart.png");
        }

        static void WriteResult(string statisticName, double statistic, bool isConfirmed)
        {
            Console.WriteLine(string.Format("{0} statistic: {1}", statisticName, Math.Round(statistic, 3)));
            if (isConfirmed)
                Console.WriteLine("Confirm");
            else
                Console.WriteLine("Reject");
            Console.WriteLine("\n\n");
        }

        static void Lab3ChiSquared()
        {
            uint volume = 200;
            double[] arguments = new VariableGenerator(leftBound, rightBound, volume).GetVariables();
            var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x), 9);
            var histogramData = new HistogramData(sampleHandler);
            new HistogramHandler(histogramData).Plot("Histogram.png");

            var chiSquaredTest = new ChiSquaredTest(histogramData);

            var chiSquaredTable = new TableHandler();
            chiSquaredTable.Set(new string[] {"i", "F(A_i)", "F(B_i)", "p_i", "p*_i", "n * (p_i - p*_i)^2 / p_i" },
                                chiSquaredTest.ToTableRows());
            chiSquaredTable.Draw("Chi-SquaredTable.pdf");

            WriteResult("Chi-squared", chiSquaredTest.ChiSquaredStatistic, chiSquaredTest.IsConfirmed);
        }

        static void Lab3Kolmogorov()
        {
            uint volume = 30;
            double[] arguments = new VariableGenerator(leftBound, rightBound, volume).GetVariables();
            var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x), 3);

            var sampleTable = new TableHandler();
            sampleTable.Set(new string[] { "Y_i", "n_i", "w_i", "w_a" },
                            sampleHandler.ToTableRows());

            var chartHandler = new SampleChartHandler(sampleTable.Table,
                                                      arguments,
                                                      sampleHandler.Function);
            chartHandler.Plot("SampleChart_Kolmogorov.png");

            var kolmogorovTest = new KolmogorovTest(chartHandler.AccumulatedFrequencies, chartHandler.AnalyticalFrequencies, sampleHandler.Volume);
            WriteResult("Kolmogorov", kolmogorovTest.KolmogorovStatistic, kolmogorovTest.IsConfirmed);
        }

        static void Lab3Mises()
        {
            uint volume = 50;
            double[] arguments = new VariableGenerator(leftBound, rightBound, volume).GetVariables();
            var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x), 3);

            Func<double, double> analyticalDistributionFunction = x => 1 - (2 * Math.Acos(x) / Math.PI);
            double[] analyticalFrequencies = sampleHandler.SamplePoints.Select(x => analyticalDistributionFunction(x)).ToArray();
            //Array.Sort(analyticalFrequencies);

            var misesCriterion = new MisesCriterion(analyticalFrequencies);
            WriteResult("Mises", misesCriterion.MisesStatistic, misesCriterion.IsConfirmed);
        }

        // gammas: 0.90, 0.95, 0.98, 0.99
        static double[] gammas = new double[] { 0.90, 0.95, 0.98, 0.99 };

        // key - sample volume, value - array of t_gamma_n-1 T-distribution values
        static Dictionary<int, double[]> studentTFunctionValues = new Dictionary<int, double[]>
        {
            {20, new double[] { 1.729132811521367, 2.093024054408263, 2.5394831906222883, 2.860934606449914 } },
            {30, new double[] { 1.6991270265334972, 2.045229642132703, 2.4620213601503833, 2.7563859036703344 } },
            {50, new double[] { 1.6765508919142629, 2.009575234489209, 2.4048917596601207, 2.67995197363155 } },
            {70, new double[] { 1.6672385485425922, 1.9949454146328136, 2.3816145030996787, 2.6489767689254546 } },
            {100, new double[] { 1.6603911559963895, 1.9842169515086827, 2.3646058614359737, 2.6264054563851857 } },
            {150, new double[] { 1.6551445337952997, 1.976013177679155, 2.3516348950235146, 2.6092279073321927 } },
        };

        static int[] volumes = new int[] { 20, 30, 50, 70, 100, 150 };

        // D(X) = (b - a)^2 / 12;
        static double AnalyticalVariance(double leftBound, double rightBound)
        {
            return Math.Pow(rightBound - leftBound, 2) / 12;
        }

        // M(X) = (a + b) / 2;
        static double AnalyticalMean(double leftBound, double rightBound)
        {
            return (leftBound + rightBound) / 2;
        }

        // S * t_gamma_n-1 / sqrt(n - 1)
        static double Deviation(double sampleVariance, int volume, double studentTFunctionValue)
        {
            return sampleVariance * studentTFunctionValue / Math.Sqrt(volume - 1);
        }

        static void Draw(double[] deviations, string fileName)
        {
            var plt = new Plot();

            plt.PlotScatter(gammas, deviations.Select(x => x * 2).ToArray());
            plt.YLabel("Confidence interval value");
            plt.XLabel("Significance level");

            plt.SaveFig(fileName);
        }

        static void Lab4Task1()
        {
            double mean = AnalyticalMean(0, 1),
                   variance = AnalyticalVariance(0, 1);

            Console.WriteLine($"Analytical mean: {mean}");
            Console.WriteLine($"Analytical variance: {variance}\n");

            int count = 1;
            foreach (int volume in volumes)
            {
                double[] arguments = new VariableGenerator(leftBound, rightBound, (uint)volume).GetVariables();
                var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x), 9);

                double sampleMean = sampleHandler.SampleMean,
                       unbiasedSampleVariance = sampleHandler.UnbiasedSampleVariance;

                double[] deviations = studentTFunctionValues[volume].Select
                                                                    (x => Deviation(unbiasedSampleVariance, volume, x))
                                                                    .ToArray();

                Console.WriteLine($"Volume: {volume}");
                Console.WriteLine($"Sample mean: {Math.Round(sampleMean, 4)}");
                Console.WriteLine($"Unbiased sample variance: {Math.Round(unbiasedSampleVariance, 4)}");

                for (int i = 0; i < deviations.Length; i++)
                {
                    Console.WriteLine($"{Math.Round(sampleMean - deviations[i], 4)} <= m_x <= " +
                                      $"{Math.Round(sampleMean + deviations[i], 4)} " +
                                      $"Student t-function value = {Math.Round(studentTFunctionValues[volume][i], 4)}.");
                }
                Console.WriteLine();

                Draw(deviations, $"Lab3Task1_Gamma-dependence_{count}.png");
                count++;
            }
        }
    }
}
