using System;

namespace Labs
{
    public class VariableGenerator : Random
    {
        uint _count;
        double _leftBound;
        double _rightBound;

        public VariableGenerator(double leftBound, double rightBound, uint count) : base()
        {
            _leftBound = leftBound;
            _rightBound = rightBound;
            _count = count;
        }

        public double[] GetVariables()
        {
            double[] variables = new double[_count];

            double boundDifference = _rightBound - _leftBound;
            for (uint i = 0; i < _count; i++)
                variables[i] = _leftBound + (boundDifference * Sample());

            return variables;
        }
    }
}
