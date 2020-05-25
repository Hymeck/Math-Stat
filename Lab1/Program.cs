using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Pdf;
using OxyPlot.Axes;

namespace Lab1
{
    class Program
    {
        // Y = cos(x); a = -pi/2; b = pi/2; y_0 = 0.5;
        // f(y) - ?; write f(y_0)
        // http://www.alexeypetrov.narod.ru/C/r_gener_about.html
        // https://habr.com/ru/post/263993/
        static double pi = Math.PI;

        static void Main(string[] args)
        {
            var sample = new Sample(
                new VariableGenerator(-pi / 2, pi / 2, 10), 
                x => Math.Cos(x));
            
            var samplePoints = sample.GetSamplePoints(true);
            for (int i = 0; i < samplePoints.Length; i++)
            {
                Console.Write($"{samplePoints[i]} ");
            }
        }
    }
}
