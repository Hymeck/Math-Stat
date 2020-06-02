using System;

namespace Labs
{
    class Program
    {
        static double pi = Math.PI;
        static void Main(string[] args)
        {
            //Lab1();
            Lab3();
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

        static void Lab3()
        {
            uint volume = 200;
            double[] arguments = new VariableGenerator(-pi / 2, pi / 2, volume).GetVariables();
            var sampleHandler = new SampleHandler(arguments, x => Math.Cos(x), 9);
            var histogramData = new HistogramData(sampleHandler);
            var histogramHandler = new HistogramHandler(histogramData);
            histogramHandler.Plot();
        }
    }
}
