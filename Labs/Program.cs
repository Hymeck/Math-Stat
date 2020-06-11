using System;
using System.Linq;
using System.Collections.Generic;
using ScottPlot;

namespace Labs
{
    class Program
    {
        // Y = cos(X); a = -pi / 2; b = pi / 2;
        static double pi = Math.PI;

        Func<double, double> AnaliticalDensityFunction = x => 2 / (Math.PI * Math.Sqrt(1 - (x * x)));
        static double analyticalVariance = 0.0947;
        static double analyticalMean = 2 / pi;

        static double leftBound = -pi / 2;
        static double rightBound = pi / 2;
        static void Main(string[] args)
        {
            //Lab1();
            //Lab3();
            Lab4();
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

        #region Lab 3
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

        static void Lab3()
        {
            Lab3ChiSquared();
            Lab3Kolmogorov();
            Lab3Mises();
        }

        #endregion Lab 3

        #region Lab 4
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

        static Dictionary<int, double[]> chiSquaredLeftValues = new Dictionary<int, double[]>
        {
            {20, new double[] { 31.410432844230918, 34.16960690283833, 37.56623478662507, 39.99684631293865 } },
            {30, new double[] { 43.77297182574219, 46.97924224367115, 50.89218131151707, 53.671961930240585 } },
            {50, new double[] { 67.5048065495412, 71.42019518750642, 76.1538912490127, 79.48997846682893 } },
            {70, new double[] { 90.53122543488065, 95.02318419040617, 100.42518422881135, 104.21489877981679 } },
            {100, new double[] { 124.34211340400407, 129.5611971858366, 135.80672317102676, 140.1694894423138 } },
            {150, new double[] { 179.58063415418053, 185.80044700379327, 193.20768638551056, 198.36020599864545 } },
        };

        static Dictionary<int, double[]> chiSquaredRightValues = new Dictionary<int, double[]>
        {
            {20, new double[] { 10.850811394182585, 9.590777392264867, 8.260398332546398, 7.433844262934232 } },
            {30, new double[] { 18.49266098195347, 16.790772265566623, 14.953456528455447, 13.786719859502718 } },
            {50, new double[] { 34.76425168350175, 32.357363695658655, 29.70668269884127, 27.99074886637332 } },
            {70, new double[] { 51.739278048962916, 48.757564805039536, 45.44171731081055, 43.27517954582346 } },
            {100, new double[] { 77.92946516501726, 74.22192747492373, 70.0648949253998, 67.32756330547917 } },
            {150, new double[] { 122.69177538711729, 117.98451540290291, 112.66758283241268, 109.14224810953266 } },
        };

        // n
        static int[] volumes = new int[] { 20, 30, 50, 70, 100, 150 };

        // S * t_gamma_n-1 / sqrt(n - 1)
        static double MeanDeviation(double variance, int volume, double studentTFunctionValue)
        {
            return studentTFunctionValue * Math.Sqrt(variance) / Math.Sqrt(volume - 1);
        }

        // n * S^2 / chi-squared
        static double VarianceDeviation(double variance, int volume, double chiSquared)
        {
            return volume * variance / chiSquared;
        }

        static void Draw(double[] deviations, double[] deviationsWithTrueCharacteristic, string fileName, bool isMean = true)
        {
            var plt = new Plot();

            if (isMean)
            {
                plt.PlotScatter(gammas, deviations.Select(x => x * 2).ToArray(), label: "With unknown variance");
                plt.PlotScatter(gammas, deviationsWithTrueCharacteristic.Select(x => x * 2).ToArray(), label: "With known variance");
            }

            else
            {
                plt.PlotScatter(gammas, deviations, label: "With unknown mean");
                plt.PlotScatter(gammas, deviationsWithTrueCharacteristic, label: "With known mean");
            }

            plt.YLabel("Confidence interval value");
            plt.XLabel("Significance level");
            plt.Legend();

            plt.SaveFig(fileName);
        }

        static void DrawWithVolume(List<double[]> deviationList, string fileName)
        {
            var plt = new Plot();

            double[] averageValues = new double[volumes.Length];
            for (int i = 0; i < volumes.Length; i++)
            {
                double averageValue = 0;
                for (int j = 0; j < deviationList[i].Length; j++)
                    averageValue += deviationList[i][j];

                averageValues[i] = averageValue / deviationList[i].Length;
            }

            plt.PlotScatter(volumes.Select(x => (double)x).ToArray(), averageValues);
            plt.XLabel("Volume");
            plt.YLabel("Average confidence interval value");
            plt.SaveFig(fileName);
        }

        static void Lab4Task1(double analyticalVariance)
        {
            int count = 1;
            var deviationsList = new List<double[]>();
            foreach (int volume in volumes)
            {
                double[] arguments = new VariableGenerator(leftBound, rightBound, (uint)volume).GetVariables();
                var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x), 9);

                double sampleMean = sampleHandler.SampleMean,
                       unbiasedSampleVariance = sampleHandler.UnbiasedSampleVariance;

                double[] deviations = studentTFunctionValues[volume].Select
                                                                    (x => MeanDeviation(unbiasedSampleVariance, volume, x))
                                                                    .ToArray();
                double[] deviationsWithTrueVariance = studentTFunctionValues[volume].Select
                                                                    (x => MeanDeviation(analyticalVariance, volume, x))
                                                                    .ToArray();

                Console.WriteLine($"Volume: {volume}");
                Console.WriteLine($"Sample mean: {Math.Round(sampleMean, 4)}");
                Console.WriteLine($"Unbiased sample variance: {Math.Round(unbiasedSampleVariance, 4)}");

                for (int i = 0; i < deviations.Length; i++)
                {
                    Console.WriteLine($"{Math.Round(sampleMean - deviations[i], 4)} <= Mean <= " +
                                      $"{Math.Round(sampleMean + deviations[i], 4)} " +
                                      $"Student t-function value = {Math.Round(studentTFunctionValues[volume][i], 4)}.");
                }
                Console.WriteLine();

                Draw(deviations, deviationsWithTrueVariance, $"Lab3Task1_SampleMean-GammaDependence_{count}.png");
                deviationsList.Add(deviations);
                count++;
            }
            DrawWithVolume(deviationsList, "Lab3Task1_SampleMean-VolumeDependence.png");
        }

        static void Lab4Task2()
        {
            int count = 1;
            var deviationsList = new List<double[]>();
            foreach (int volume in volumes)
            {
                double[] arguments = new VariableGenerator(leftBound, rightBound, (uint)volume).GetVariables();
                var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x), 9);

                double sampleMean = sampleHandler.SampleMean,
                       unbiasedSampleVariance = sampleHandler.UnbiasedSampleVariance,
                       biasedSampleMean = sampleHandler.BiasedSampleVariance;

                double[] leftDeviations = chiSquaredLeftValues[volume].Select(x => VarianceDeviation(unbiasedSampleVariance, volume, x)).ToArray();
                double[] rightDeviations = chiSquaredRightValues[volume].Select(x => VarianceDeviation(unbiasedSampleVariance, volume, x)).ToArray();


                double[] leftDeviationsWithBiasedVariance = chiSquaredLeftValues[volume].Select(x => VarianceDeviation(biasedSampleMean, volume, x)).ToArray();
                double[] rightDeviationsWithBiasedVariance = chiSquaredRightValues[volume].Select(x => VarianceDeviation(biasedSampleMean, volume, x)).ToArray();

                Console.WriteLine($"Volume: {volume}");
                Console.WriteLine($"Sample mean: {Math.Round(sampleMean, 4)}");
                Console.WriteLine($"Unbiased sample variance: {Math.Round(unbiasedSampleVariance, 4)}");

                double[] deviations = new double[leftDeviations.Length];
                double[] deviationsWithTrueMean = new double[leftDeviations.Length];
                for (int i = 0; i < leftDeviations.Length; i++)
                {
                    deviationsWithTrueMean[i] = rightDeviationsWithBiasedVariance[i] - leftDeviationsWithBiasedVariance[i];
                    deviations[i] = rightDeviations[i] - leftDeviations[i];

                    Console.WriteLine($"{Math.Round(leftDeviations[i], 4)} <= Variance <= " +
                                      $"{Math.Round(rightDeviations[i], 4)} " +
                                      $"Chi-squared left = {Math.Round(chiSquaredLeftValues[volume][i], 4)}, " +
                                      $"Chi-squared right = {Math.Round(chiSquaredRightValues[volume][i], 4)}");
                }
                Console.WriteLine();
                Draw(deviations, deviationsWithTrueMean, $"Lab3Task2_SampleVariance-GammaDependence_{count}.png", false);
                deviationsList.Add(deviations);
                count++;
            }
            DrawWithVolume(deviationsList, "Lab3Task2_SampleVariance-VolumeDependence.png");
        }

        static void Lab4()
        {
            Console.WriteLine($"Analytical mean: {analyticalMean}");
            Console.WriteLine($"Analytical variance: {analyticalVariance}\n\n");
            Lab4Task1(analyticalVariance);
            Console.WriteLine("\n\n\n");
            Lab4Task2();
        }
        #endregion Lab 4
    }
}
