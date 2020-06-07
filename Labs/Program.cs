using System;
using System.Linq;

namespace Labs
{
    class Program
    {
        static double pi = Math.PI;
        static void Main(string[] args)
        {
            //Lab1();
            Lab3ChiSquared();
            Lab3Kolmogorov();
            Lab3Mises();
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
            double[] arguments = new VariableGenerator(-pi / 2, pi / 2, volume).GetVariables();
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
            double[] arguments = new VariableGenerator(-pi / 2, pi / 2, volume).GetVariables();
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
            double[] arguments = new VariableGenerator(-pi / 2, pi / 2, volume).GetVariables();
            var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x), 3);

            Func<double, double> analyticalDistributionFunction = x => 1 - (2 * Math.Acos(x) / Math.PI);
            double[] analyticalFrequencies = sampleHandler.SamplePoints.Select(x => analyticalDistributionFunction(x)).ToArray();
            //Array.Sort(analyticalFrequencies);

            var misesCriterion = new MisesCriterion(analyticalFrequencies);
            WriteResult("Mises", misesCriterion.MisesStatistic, misesCriterion.IsConfirmed);
        }
    }
}
