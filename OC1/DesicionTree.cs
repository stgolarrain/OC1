using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class DesicionTree
    {
        private Node _root;

        public DesicionTree(Node root)
        {
            _root = root;
        }

        public Node GetRoot()
        {
            return _root;
        }

        public double Evaluate(double[] input)
        {
            Node node = _root;
            while (node != null)
            {
                if (node.Evaluate(input) < 0)
                {
                    if (node.GetLeft() != null)
                        node = node.GetLeft();
                    else
                        return node.classType;
                }
                else
                {
                    if (node.GetRight() != null)
                        node = node.GetRight();
                    else
                        return node.classType;
                }
            }
            return -1;
        }

        public void Prunning(int n)
        {
            Node node = _root;
            if (node.leaf > n)
            {
                if (node.GetRight() != null)
                    Prunning(n, node.GetRight());
                if (node.GetLeft() != null)
                    Prunning(n, node.GetLeft());
            }
        }

        private void Prunning(int n, Node node)
        {
            if (node.GetLeft() != null)
                Prunning(n, node.GetLeft());
            if (node.GetRight() != null)
                Prunning(n, node.GetRight());

            if (node.GetRight() == null && node.GetLeft() == null)
                return;

            if (node.GetRight() == null && node.GetLeft().leaf < n)
            {
                node.leaf += node.GetLeft().leaf;
                node.classType = node.GetLeft().classType;
                node.RemoveLeft();
            }
            else if (node.GetLeft() == null && node.GetRight().leaf < n)
            {
                node.leaf += node.GetRight().leaf;
                node.classType = node.GetRight().classType;
                node.RemoveRight();
            }

            else if (node.GetRight().leaf < n && node.GetLeft().leaf >= n)
            {
                node.classType = node.GetRight().classType;
                node.leaf += node.GetRight().leaf;
                node.RemoveRight();
            }
            else if (node.GetRight().leaf >= n && node.GetLeft().leaf < n)
            {
                node.classType = node.GetLeft().classType;
                node.leaf += node.GetLeft().leaf;
                node.RemoveLeft();
            }
            else if (node.GetRight().leaf < n && node.GetLeft().leaf < n)
            {
                if (node.GetRight().leaf > node.GetLeft().leaf)
                    node.classType = node.GetRight().classType;
                else
                    node.classType = node.GetLeft().classType;

                node.leaf += node.GetLeft().leaf + node.GetRight().leaf;
                node.RemoveLeft();
                node.RemoveRight();
            }
            
        }
    }
}
