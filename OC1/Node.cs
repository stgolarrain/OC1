using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class Node
    {

        private double[] _weights;
        private double _baias;
        private int _dimension;
        private Node _left, _right, _parent;
        private double[][] _data;
        private static int splitConstant;

        public Node(int dimension, double[] weights, double[][] data)
        {
            _dimension = dimension;
            _weights = weights;
            _baias = 0;
            _data = data;
        }

        public static void setSplitConstant(int k)
        {
            splitConstant = k;
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

        /*
         * Calculates the Entropy of a data set.
         */
        public double Entropy(double[][] input)
        {
            double entropy = 0;

            foreach (double p in getClassesProbabilities(input))
            {
                if (p > 0)
                    entropy += -p * Math.Log(p, 2);
            }
            return entropy;
        }

        /*
         * Calculates the clases of a given data set.
         */
        public int[] getClases(double[][] input)
        {
            int dimension = input[0].Length;
            List<int> clases = new List<int>();
            foreach (double[] line in input)
            {
                if (!clases.Contains((int)line[dimension - 1]))
                    clases.Add((int)line[dimension - 1]);
            }
            return clases.ToArray();
        }

        /*
         * Calculates the probability of each class on the dataset
         * CAMBIAR
         */
        public double[] getClassesProbabilities(double[][] input)
        {
            int total = input.Length;
            int dimension = input[0].Length;
            double[] probabilities = new double[getClases(input).Length];
            for (int i = 0; i < input.Length; i++)
            {
                probabilities[(int)input[i][dimension - 1] - 1]++;
            }
            return probabilities.Select(p => p / total).ToArray();
        }

        public double Gain(int atributte)
        {
            double gain = Entropy(_data);
            int total = _data.Length;

            double min = Double.MaxValue;
            double max = Double.MinValue;
            foreach (double[] line in _data)
            {
                if (score(line) > max)
                    max = line[atributte];
                if (score(line) < min)
                    min = line[atributte];
            }
            double delta = (max - min) / splitConstant;

            List<double[]>[] divisionGroupsLine = new List<double[]>[splitConstant];
            for (int i = 0; i < splitConstant; i++)
                divisionGroupsLine[i] = new List<double[]>();

            for (int i = 0; i < _data.Length; i++)
            {
                for (int j = 0; j < splitConstant; j++)
                {
                    if (score(_data[i]) >= (min + j * delta) && score(_data[i]) < (min + (j + 1) * delta))
                        divisionGroupsLine[j].Add(_data[i]);
                }
            }

            for (int i = 0; i < divisionGroupsLine.Length; i++)
            {
                gain = gain - ((double)divisionGroupsLine[i].Count() / total) * Entropy(divisionGroupsLine[i].ToArray());
            }

            return gain;
        }

        /*public int GetHigherGain()
        {
            int atributte = 0;
            double gain = -1;
            for (int i = 0; i < _data[0].Length - 1; i++)
            {
                if (_activeAtributtes[i])
                {
                    double localGain = Gain(i);
                    if (localGain > gain)
                    {
                        gain = Gain(i);
                        atributte = i;
                    }
                }
            }
            return atributte;
        }*/
    }
}
