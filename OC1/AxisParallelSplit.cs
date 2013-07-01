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
        private bool[] _activeAtributtes;

        public AxisParallelSplit(double[][] data, int k, bool[] activeAtributtes)
        {
            _data = data;
            _dimension = data[0].Length;
            _k = k;
            _activeAtributtes = activeAtributtes;
        }

        public int GetClass()
        {
            if (SameClass() != -1)
                return SameClass();
            if (SameAtributtes() != -1)
                return SameAtributtes();
            return -1;
            
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

        /*
         * Returns -1 if the input doesn't have the same value for all the atributtes.
         * Returns the atributte number in other case.
         */
        public int SameAtributtes()
        {
            double[] values = _data[0];
            int[] classCount = new int[_dimension];

            foreach (double[] line in _data)
            {
                for(int a = 0; a < _dimension; a++)
                {
                    classCount[(int)line[_dimension - 1]]++;
                    if (values[a] != line[a])
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

        /*
         * Calculates the Entropy of a data set.
         */ 
        public static double Entropy(double[][] input)
        {
            double entropy = 0;
            foreach (double p in getClasesProbabilities(input))
            {
                if(p > 0)
                    entropy += -p * Math.Log(p, 2);
            }
            return entropy;
        }

        /*
         * Calculates the clases of a given data set.
         */ 
        public static int[] getClases(double[][] input)
        {
            int dimension = input[0].Length;
            List<int> clases = new List<int>();
            foreach (double[] line in input)
            {
                if(!clases.Contains((int)line[dimension - 1]))
                    clases.Add((int)line[dimension - 1]);
            }
            return clases.ToArray();
        }

        /*
         * Calculates the probability of each class on the dataset
         */ 
        public static double[] getClasesProbabilities(double[][] input)
        {
            int total = input.Length;
            int dimension = input[0].Length;
            double[] probabilities = new double[getClases(input).Length];
            for (int i = 0; i < input.Length; i++)
            {
                probabilities[(int)input[i][dimension - 1] - 1]++;
            }
            return probabilities.Select(p => p/total).ToArray();
        }

        public double Gain(int atributte)
        {
            double gain = Entropy(_data);
            int total = _data.Length;

            double min = Double.MaxValue;
            double max = Double.MinValue;
            foreach (double[] line in _data)
            {
                if (line[atributte] > max)
                    max = line[atributte];
                if (line[atributte] < min)
                    min = line[atributte];
            }
            double delta = (max - min) / (_k);

            List<double[]>[] divisionGroupsLine = new List<double[]>[_k];
            for (int i = 0; i < _k; i++)
                divisionGroupsLine[i] = new List<double[]>();

            for (int i = 0; i < _data.Length; i++)
            {
                for (int j = 0; j < _k; j++)
                {
                    if(_data[i][atributte] >= (min + j*delta) && _data[i][atributte] < (min + (j+1)*delta))
                        divisionGroupsLine[j].Add(_data[i]);
                }
            }

            for (int i = 0; i < divisionGroupsLine.Length; i++)
            {
                gain = gain - ((double)divisionGroupsLine[i].Count() / total) * Entropy(divisionGroupsLine[i].ToArray());
            }

            return gain;
        }

        public int GetHigherGain()
        {
            int atributte = 0;
            double gain = -1;
            for (int i = 0; i < _data[0].Length - 1; i++)
            {
                if(!_activeAtributtes[i])
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
        }
    }
}
