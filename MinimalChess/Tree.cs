using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MinimalChess
{
    public class Tree
    {
        Node root;
        public Tree()
        {
            root = new Node();
        }
        public void setRoot(Node rt)
        {
            root = rt;
        }
        public Node getRoot()
        {
            return root;
        }
    }
}
