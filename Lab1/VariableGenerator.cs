using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class VariableGenerator
    {
        uint _count;
        double _leftBound;
        double _rightBound;

        Random randomProvider;

        public VariableGenerator(double leftBound, double rightBound, uint count)
        {
            _leftBound = leftBound;
            _rightBound = rightBound;
            _count = count;
            randomProvider = new Random();
        }

        public double[] GetVariables()
        {
            double[] variables = new double[_count];

            double boundDifference = _rightBound - _leftBound;
            for (uint i = 0; i < _count; i++)
                variables[i] = _leftBound + (boundDifference * randomProvider.NextDouble()); // boundDifference * eps_i

            return variables;
        }
    }
}
