using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class AxisParallelSplit
    {

        private double[][] _data;
        private int _dimension;
        private int _k;

        public AxisParallelSplit(double[][] data, int dimension, int k)
        {
            _data = data;
            _dimension = dimension;
            _k = k;
        }

        public int SameClass()
        {
            int classValue = (int)_data[0][_dimension - 1];
            foreach (double[] line in _data)
            {
                if (line[_dimension - 1] != classValue)
                    return -1;
            }
            return classValue;
        }

        public int SameAtributtes(int[] atributtes)
        {
            double[] values = _data[0];
            int[] classCount = new int[_dimension];

            foreach (double[] line in _data)
            {
                foreach (int d in atributtes)
                {
                    classCount[(int)line[_dimension - 1]]++;
                    if (values[d] != line[d])
                        return -1;
                }
            }

            int maxScore = 0;
            int maxClass = 0;
            for(int i = 0; i < _dimension; i++)
            {
                if (classCount[i] > maxScore)
                    maxClass = i;
            }
            return maxClass;
        }

        public double Entropy(double[][] input)
        {
            double entropy = 0;
            foreach(double p in getClasesProbabilities(input))
                entropy += -p * Math.Log(p, 2);
            return entropy;
        }

        public int[] getClases()
        {
            List<int> clases = new List<int>();
            foreach(double[] line in _data)
            {
                if(!clases.Contains((int)line[_dimension - 1]))
                    clases.Add((int)line[_dimension - 1]);
            }
            return clases.ToArray();
        }

        public double[] getClasesProbabilities(double[][] input)
        {
            int total = input.Length;
            double[] probabilities = new double[getClases().Length];
            for (int i = 0; i < input.Length; i++)
            {
                probabilities[(int)input[i][_dimension - 1] - 1]++;
            }
            return probabilities.Select(p => p/total).ToArray();
        }

        public double Gain(int atributte, double[][] input)
        {
            double gain = Entropy(input);
            int total = input.Length;

            double min = 0;
            double max = 0;
            foreach (double[] line in input)
            {
                if (line[atributte] > max)
                    max = line[atributte];
                if (line[atributte] < min)
                    min = line[atributte];
            }
            double delta = (max - min) / _k;

            List<double[]>[] divisionGroupsLine = new List<double[]>[_k];
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < _k; j++)
                {
                    if(input[i][atributte] < (min + (j+1)*delta))
                        divisionGroupsLine[j].Add(_data[i]);
                }
            }

            for (int i = 0; i < divisionGroupsLine.Length; i++)
            {
                gain -= (divisionGroupsLine[i].Count() / total) * Entropy(divisionGroupsLine[i].ToArray());
            }

            return gain;
        }
    }
}
