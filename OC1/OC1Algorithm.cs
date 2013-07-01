using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class OC1Algorithm
    {

        private double[][] _data;
        private int _splitConstant;
        private int _dimension, _rIteration;
        private DesicionTree _desicionTree;

        public OC1Algorithm(double[][] inputData, int splitK)
        {
            _data = inputData;
            _splitConstant = splitK;
            _dimension = inputData[0].Length;
            _rIteration = 100;

            bool[] activeAtributtes = new bool[_dimension];
            for (int i = 0; i < _dimension; i++)
                activeAtributtes[i] = true;
            AxisParallelSplit axis = new AxisParallelSplit(_data, _dimension, _splitConstant, activeAtributtes);

            int firstSplit = axis.GetHigherGain();
            double[] weight = new double[_dimension];
            weight[firstSplit] = 1;
            Node root = new Node(_dimension, weight);
            _desicionTree = new DesicionTree(root);
        }

        public void SolveAlgorithm()
        {


        }
    }
}
