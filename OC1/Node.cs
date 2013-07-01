using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class Node : ICloneable
    {

        private double[] _weights;
        private double _baias, _Pmove;
        private int _dimension, _class;
        private Node _parent;
        private Node[] _child;
        private double[][] _data;
        List<double[]>[] _divisionGroupsLine;
        bool[] _activeAtributtes;

        private static int splitConstant;
        private static double Pstag;

        public Node(double[] weights, double[][] data, bool[] activeAtributes)
        {
            _dimension = data[0].Length;
            _weights = weights;
            _baias = 0;
            _data = data;
            _child = new Node[splitConstant];
            _activeAtributtes = activeAtributes;
            _divisionGroupsLine = new List<double[]>[splitConstant];
            for (int i = 0; i < splitConstant; i++)
                _divisionGroupsLine[i] = new List<double[]>();
            _class = -1;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int Class
        {
            get { return _class; }
            set { _class = value; }
        }

        public static void setSplitConstant(int k)
        {
            splitConstant = k;
        }

        public static void SetPmode(double p)
        {
            Pstag = p;
        }

        public bool[] ActiveAtributtes
        {
            get { return _activeAtributtes; }
        }

        public int AtributtesUse()
        {
            int result = 0;
            for (int i = 0; i < _activeAtributtes.Length; i++)
                if(_activeAtributtes[i])
                    result++;
            return result;
        }

        public List<double[]>[] DivisionGroups
        {
            get { return _divisionGroupsLine; }
        }

        public double[] Weight
        {
            get { return _weights;}
            set { _weights = value; }
        }

        public void Pertube(int m)
        {
            if (!_activeAtributtes[m])
                return;

            double[] U = new double[_data.Length];
            int localMaxInput = 0;
            double localMax = 0;
            for (int j = 0; j < U.Length; j++)
            {
                U[j] = (_data[j][m] - score(_data[j]))/_data[j][m];
                if (score(_data[j]) > localMax)
                {
                    localMaxInput = j;
                    localMax = (_data[j][m] - score(_data[j])) / _data[j][m];
                }
            }

            Node auxNode = new Node(_weights, _data, _activeAtributtes);
            auxNode.ChangeWeigth(localMaxInput, localMax);
            if (auxNode.Gain() > this.Gain())
            {
                _weights = auxNode.Weight;
                _Pmove = Pstag;
            }
            else if (auxNode.Gain() == this.Gain())
            {
                Random rand = new Random();
                if (rand.NextDouble() < _Pmove)
                    _weights = auxNode.Weight;
                _Pmove -= 0.1 * Pstag;
            }
            
        }

        public void ChangeWeigth(int atributte, double amount)
        {
            _weights[atributte] = amount;
        }

        public double score(double[] input)
        {
            double score = 0;
            for (int i = 0; i < _dimension; i++)
                score += _weights[i] * input[i];

            return score + _baias;
        }

        public void AddChild(Node n, int i)
        {
            _child[i] = n;
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

        public void setDivisionGroups()
        {
            double min = Double.MaxValue;
            double max = Double.MinValue;
            foreach (double[] line in _data)
            {
                if (score(line) > max)
                    max = score(line);
                if (score(line) < min)
                    min = score(line);
            }
            double delta = (max - min) / splitConstant;

            for (int i = 0; i < _data.Length; i++)
            {
                for (int j = 0; j < splitConstant; j++)
                {
                    if (score(_data[i]) >= (min + j * delta) && score(_data[i]) < (min + (j + 1) * delta))
                        _divisionGroupsLine[j].Add(_data[i]);
                }
            }
        }

        public double Gain()
        {
            double gain = Entropy(_data);
            int total = _data.Length;
            setDivisionGroups();

            for (int i = 0; i < _divisionGroupsLine.Length; i++)
            {
                gain = gain - ((double)_divisionGroupsLine[i].Count() / total) * Entropy(_divisionGroupsLine[i].ToArray());
            }

            return gain;
        }
    }
}
