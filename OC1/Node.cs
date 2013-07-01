using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class Node
    {

        double[] _weights;
        double _baias;
        int _dimension;
        Node _left, _right, _parent;

        public Node(int dimension, double[] weights)
        {
            _dimension = dimension;
            _weights = weights;
            _baias = 0;
        }

        public void Pertube(double[][] input)
        {
            double[] U = new double[input.Length];
            for (int j = 0; j < U.Length; j++)
            {
                U[j] = score(input[j]);
            }
        }

        public double score(double[] input)
        {
            double score = 0;
            for (int i = 0; i < _dimension; i++)
                score += _weights[i] * input[i];

            return score + _baias;
        }

        public void AddRightChild(Node n)
        {
            _right = n;
            n.SetParent(this);
        }

        public void AddLeftChild(Node n)
        {
            _left = n;
            n.SetParent(this);
        }

        public void SetParent(Node n)
        {
            _parent = n;
        }
    }
}
