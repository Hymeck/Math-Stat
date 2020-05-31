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

            var variableGenerator = new VariableGenerator(-pi / 2, pi / 2, volume);
            double[] xs = variableGenerator.GetVariables();
            var sampleHandler = new SampleHandler(xs, x => Math.Cos(x));

            var sampleTable = new SampleTableHandler();
            sampleTable.Set(
                new string[] { "Y_i", "n_i", "w_i", "w_a"},
                sampleHandler.ToTableRows());
            sampleTable.Draw("Sample_Table.pdf");

            var chartHandler = new SampleChartHandler(sampleTable.Table);
            chartHandler.PlotEmpirical("SampleChart_Empirical.png");
        }
    }
}
