using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class Train
    {
        private double[][] _data;
        private int _k;
        public DesicionTree tree { get; set; }

        private const int R = 100;
        private const int J = 50;

        public Train(double[][] dataInput, int k)
        {
            _data = dataInput;
            _k = k;
            double[] weights = new double[_data[0].Length];
            tree = new DesicionTree(new Node(weights, 0));
        }

        public void Algorithm()
        {
            Algorithm(_data, tree.GetRoot());
        }

        public void Algorithm(double[][] dataInput, Node node)
        {
            /*
            * Choose class for node if is the case.
            */
            node.classType = sameClass(dataInput);
            if (node.classType != -1)
                return;

            node.classType = sameAtributtes(dataInput);
            if (node.classType != -1)
                return;

            /*
             * Choose best axis-parallel split node for dataInput.
             */
            bestAxisParallel(dataInput, node);

            /*
             * Apply OC1
             */
            Node auxNode = (Node)node.Clone();
            for (int i = 0; i < R; i++)
            {
                if (i > 0)
                    auxNode.Random();

            step1:
                double initialGain = gain(dataInput, node); // Gain for better node.
                for (int d = 0; d < dataInput[0].Length; d++)
                {
                    pertube(d, auxNode, dataInput);
                    if (gain(dataInput, auxNode) > initialGain)
                    {
                        node = (Node)auxNode.Clone();
                        break;
                    }
                }

                // step 2
                initialGain = gain(dataInput, node);
                for (int j = 0; j < J; j++)
                {
                    Random rand = new Random();
                    int direction = rand.Next(dataInput[0].Length - 1);
                    pertube(direction, auxNode, dataInput);
                    if (gain(dataInput, auxNode) > initialGain)
                    {
                        node = (Node)auxNode.Clone();
                        goto step1;
                    }
                }
                if (gain(dataInput, auxNode) > gain(dataInput, node))
                    node = (Node)auxNode.Clone();
            }

            /*
             * Recursive Algorithm for each branch for node.
             */ 
            Node left = new Node(new double[dataInput[0].Length], 0);
            Node right = new Node(new double[dataInput[0].Length], 0);

            node.SetLeft(left);
            node.SetRight(left);
            List<double[]>[] data = divideData(dataInput, node);
            Algorithm(data[0].ToArray(), node.GetLeft());
            Algorithm(data[1].ToArray(), node.GetRight());
        }

        private void pertube(int m, Node node, double[][] dataInput)
        {
            if (!node.ActiveAtributtes[m])
                return;

            Node auxNode = (Node)node.Clone();
            double[] U = new double[dataInput.Length];
            for (int i = 0; i < dataInput.Length; i++)
                U[i] = Math.Min((node.weights[m] * dataInput[i][m] - node.Evaluate(dataInput[i])) / dataInput[i][m], 0);

            auxNode.weights[m] = U.Max();
            if (gain(dataInput, auxNode) > gain(dataInput, node))
            {
                node.weights[m] = U.Max();
                node.ResetPmove();
            }
            else if (gain(dataInput, auxNode) == gain(dataInput, node))
            {
                if(node.EvaluatePmove())
                    node.weights[m] = U.Max();
                node.DecreasePmove();
            }
        }

        private double sameClass(double[][] dataInput)
        {
            if (dataInput.Length == 0)
                return -1;

            int dimension = dataInput[0].Length;
            double classValue = (int)dataInput[0][dimension - 1];
            foreach (double[] line in dataInput)
            {
                if (line[dimension - 1] != classValue)
                    return -1;
            }
            return classValue;
        }

        private static List<double> getClases(double[][] dataInput)
        {
            List<double> classes = new List<double>();
            int dimension = dataInput[0].Length;
            for (int i = 0; i < dataInput.Length; i++)
            {
                if(!classes.Contains(dataInput[i][dimension - 1]))
                    classes.Add(dataInput[i][dimension - 1]);
            }

            return classes;
        }

        public double sameAtributtes(double[][] dataInput)
        {
            int dimension = dataInput[0].Length;
            double[] values = dataInput[0];
            int nclass = 10;
            int[] classCount = new int[nclass];

            foreach (double[] line in dataInput)
            {
                for (int a = 0; a < dimension; a++)
                {
                    classCount[(int)line[dimension - 1]]++;
                    if (values[a] != line[a])
                        return -1;
                }
            }

            int maxScore = 0;
            int maxClass = 0;
            for (int i = 0; i < nclass; i++)
            {
                if (classCount[i] > maxScore)
                    maxClass = i;
            }
            return maxClass;
        }

        private void bestAxisParallel(double[][] dataInput, Node n)
        {
            bool[] dimensionUsed = new bool[dataInput[0].Length];
            dimensionUsed[dataInput[0].Length - 1] = true;
            for (int k = 0; k < _k; k++)
            {
                int bestDimension = 0;
                double bestGain = gain(dataInput, n);
                for (int d = 0; d < dataInput[0].Length; d++)
                {
                    if (!dimensionUsed[d])
                    {
                        n.weights[d] = 1;
                        for (int i = 0; i < dataInput.Length; i++)
                        {
                            double baias = n.baias;
                            n.SetBaias(dataInput[i]);
                            if (gain(dataInput, n) > bestGain)
                            {
                                bestDimension = d;
                                bestGain = gain(dataInput, n);
                            }
                            else
                                n.baias = baias;
                        }
                        n.weights[d] = 0;
                    }
                }
                dimensionUsed[bestDimension] = true;
                n.weights[bestDimension] = 1;
                n.Activate(bestDimension);
            }
        }

        private static List<PairValue> classProbabilities(double[][] input)
        {
            List<PairValue> probabilities = new List<PairValue>();
            int dimension = input[0].Length;
            int total = input.Length;

            for (int i = 0; i < input.Length; i++)
            {
                bool condition = false;    
                foreach (PairValue pair in probabilities)
                {
                    if (pair.Key == input[i][dimension - 1])
                    {
                        pair.Value++;
                        condition = true;
                        break;
                    }
                }
                if(!condition)
                    probabilities.Add(new PairValue(input[i][dimension - 1], 1));
            }

            foreach (PairValue pair in probabilities)
                pair.Value /= total;

            return probabilities;
        }

        private static double entropy(double[][] input)
        {
            double entropy = 0;
            foreach (PairValue pair in classProbabilities(input))
            {
                if (pair.Value > 0)
                    entropy += -pair.Value * Math.Log(pair.Value, 2);
            }
            return entropy;
        }

        /*
         * data[0]: left
         * data[1]: right
         */ 
        private static List<double[]>[] divideData(double[][] input, Node node)
        {
            List<double[]>[] data = new List<double[]>[2];
            data[0] = new List<double[]>();
            data[1] = new List<double[]>();

            for (int i = 0; i < input.Length; i++)
            {
                if (node.Evaluate(input[i]) < 0)
                    data[0].Add(input[i]);
                else
                    data[1].Add(input[i]);
            }
            return data;
        }

        private static double gain(double[][] input, Node node)
        {
            double gain = entropy(input);
            List<double[]>[] dataGroups = divideData(input, node);
            for (int i = 0; i < 2; i++)
            {
                if(dataGroups[i].Count > 0)
                    gain -= (dataGroups[i].Count * entropy(dataGroups[i].ToArray())) / input.Length;
            }
            return gain;
        }
    }
}
