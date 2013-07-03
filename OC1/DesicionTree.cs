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
    }
}
