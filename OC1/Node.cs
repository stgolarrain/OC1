using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class Node : ICloneable
    {

        public double[] weights { get; set; }
        public double baias { get; set; }
        public double classType {get; set;}
        public int leaf { get; set; }

        private bool[] _activeAtributtes;
        private Node _parent, _left, _right;
        private double _Pmove;

        private const double Pstag = .5;
 

        public Node(double[] weights, double baias)
        {
            this.weights = weights;
            this.baias = baias;
            _activeAtributtes = new bool[weights.Length];
            classType = -1;
            _Pmove = Pstag;
        }

        public void SetNode(Node input)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = input.weights[i];
            }
            baias = input.baias;
        }

        public bool[] ActiveAtributtes { get { return _activeAtributtes; } set { _activeAtributtes = value; } }

        public void Activate(int n)
        {
            _activeAtributtes[n] = true;
        }

        public object Clone()
        {
            Node clone = (Node)this.MemberwiseClone();

            clone.weights = new double[this.weights.Length];
            clone.ActiveAtributtes = new bool[this.ActiveAtributtes.Length];
            for (int i = 0; i < this.weights.Length; i++ )
            {
                clone.weights[i] = this.weights[i];
                clone.ActiveAtributtes[i] = this.ActiveAtributtes[i];
            }
            clone.classType = -1;

            return clone;
        }

        public double Evaluate(double[] input)
        {
            if (classType != -1)
                return classType;

            double evaluation = baias;
            for (int i = 0; i < input.Length; i++)
                evaluation += input[i] * weights[i];

            return evaluation;
        }

        public void ResetPmove()
        {
            _Pmove = Pstag;
        }

        public void DecreasePmove()
        {
            _Pmove -= 0.1 * Pstag;
        }

        public bool EvaluatePmove()
        {
            Random rand = new Random();
            if (rand.NextDouble() < _Pmove)
                return true;

            return false;
        }

        public void Random()
        {
            Random rand = new Random();
            for (int i = 0; i < weights.Length; i++)
            {
                if (_activeAtributtes[i] && i!=weights.Length)
                    weights[i] = 2 * rand.NextDouble() - 1;
            }       
        }

        private void setParent(Node parent)
        {
            _parent = parent;
        }

        public void SetRight(Node right)
        {
            _right = right;
            _right.setParent(this);
        }

        public void SetLeft(Node left)
        {
            _left = left;
            _left.setParent(this);
        }

        public Node GetRight()
        {
            return _right;
        }

        public Node GetLeft()
        {
            return _left;
        }

        public void SetBaias(double[] point)
        {
            baias = 0;
            for (int i = 0; i < point.Length; i++)
                baias -= weights[i] * point[i];
        }

        public void RemoveRight()
        {
            _right = null;
        }

        public void RemoveLeft()
        {
            _left = null;
        }
    }
}
