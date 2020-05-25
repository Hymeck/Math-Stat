using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class Sample
    {
        public delegate double Function(double x);

        double[] _arguments;
        double[] _samplePoints;
        bool _wasSorted;
        Function _function;

        public Sample(VariableGenerator arguments, Function function)
        {
            _arguments = arguments.GetVariables();
            _function = function;
        }

        public Sample(double[] arguments, Function function)
        {
            _arguments = arguments;
            _function = function;
        }

        public double[] GetSamplePoints(bool isSorted = false)
        {
            if (_samplePoints == null)
            {
                int length = _arguments.Length;
                _samplePoints = new double[length];
                for (uint i = 0; i < length; i++)
                    _samplePoints[i] = Math.Round(_function(_arguments[i]), 5);
            }

            if (isSorted && !_wasSorted)
            {
                Array.Sort(_samplePoints);
                _wasSorted = true;
            }

            double[] samplePoints = new double[_samplePoints.Length];
            _samplePoints.CopyTo(samplePoints, 0);
            return samplePoints;
        }
    }
}
