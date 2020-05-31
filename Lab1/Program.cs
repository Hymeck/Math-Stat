using System;

namespace Lab1
{
    class Program
    {
        static double pi = Math.PI;
        static void Main(string[] args)
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

            var sampleTable = new SampleTableHandler();
            sampleTable.Set(new string[] { "Y_i", "n_i", "w_i", "w_a"},
                            sampleHandler.ToTableRows());
            sampleTable.Draw("SampleTable.pdf");

            var chartHandler = new SampleChartHandler(sampleTable.Table, 
                                                      arguments, 
                                                      sampleHandler.Function);
            chartHandler.Plot("SampleChart.png");
        }
    }
}
