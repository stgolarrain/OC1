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
            AxisParallelSplit axis = new AxisParallelSplit(_data, _splitConstant, activeAtributtes);

            int firstSplit = axis.GetHigherGain();
            double[] weight = new double[_dimension];
            weight[firstSplit] = 1;
            bool[] activeAtributes = new bool[_dimension];
            activeAtributes[firstSplit] = true;
            Node root = new Node(weight, _data, activeAtributes);
            _desicionTree = new DesicionTree(root);
        }

        public void ConstructTree()
        {
            Node node = _desicionTree.getRoot();
            int i = 0;
            foreach (List<double[]> dataSplit in node.DivisionGroups)
            {
                AxisParallelSplit axisParallel = new AxisParallelSplit(dataSplit.ToArray(), _splitConstant, node.ActiveAtributtes);
                bool[] activeAtributtes = node.ActiveAtributtes;
                activeAtributtes[axisParallel.GetHigherGain()] = true;
                double[] weights = new double[_dimension];
                weights[axisParallel.GetHigherGain()] = 1;
                Node nextNode = new Node(weights, dataSplit.ToArray(), activeAtributtes);
                node.AddChild(FindSplitH(nextNode), i);
                i++;
                ConstructTree(node);
            }
        }

        public void ConstructTree(Node n)
        {
            Node node = n;
            int i = 0;
            foreach (List<double[]> dataSplit in node.DivisionGroups)
            {
                AxisParallelSplit axisParallel = new AxisParallelSplit(dataSplit.ToArray(), _splitConstant, node.ActiveAtributtes);
                if (axisParallel.GetClass() == -1)
                {
                    bool[] activeAtributtes = node.ActiveAtributtes;
                    activeAtributtes[axisParallel.GetHigherGain()] = true;
                    double[] weights = new double[_dimension];
                    weights[axisParallel.GetHigherGain()] = 1;
                    Node nextNode = new Node(weights, dataSplit.ToArray(), activeAtributtes);
                    node.AddChild(FindSplitH(nextNode), i);
                    ConstructTree(node);
                }
                else
                {
                    Node nextNode = new Node(node.Weight, dataSplit.ToArray(), node.ActiveAtributtes);
                    nextNode.Class = axisParallel.GetClass();
                    node.AddChild(nextNode, i);
                }
                i++;
            }
        }

        public Node FindSplitH(Node node)
        {
            Node H = (Node)node.Clone();
            for (int i = 0; i < _rIteration; i++)
            {
                if(i > 0)
                    H = RandomH(H);
                // Falta condicion de random H
            step1:
                double gain = H.Gain();
                while (!(H.Gain() > gain))
                {
                    for (int j = 0; j < _dimension; j++)
                    {
                        H.Pertube(j);
                    }
                }
            step2:
                gain = H.Gain();
                Random rand = new Random();
                for (int j = 0; j < _rIteration; j++)
                {
                    node.Pertube(rand.Next(_dimension));
                    if (node.Gain() > gain)
                        goto step1;
                }
                if (H.Gain() > node.Gain())
                    node = (Node)H.Clone();
            }
            return node;
        }

        public Node RandomH(Node node)
        {
            double[] weights = new double[_dimension];
            bool[] atributtesActivation = node.ActiveAtributtes;
            Random rand = new Random();
            for (int i = 0; i < _dimension; i++)
            {
                if (atributtesActivation[i])
                    weights[i] = rand.NextDouble();
            }

            Node result = (Node)node.Clone();
            result.Weight = weights;
            return result;
        }
        
    }
}
