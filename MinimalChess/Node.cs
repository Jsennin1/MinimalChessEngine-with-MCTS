using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalChess
{
    public class Node
    {
        State state;
        Node parent;
        List<Node> childArray;
        public Node()
        {
            state = new State();
            childArray = new List<Node>();
        }
        public void copyStateOfNode(Node node)
        {
            state = new State();
            state.CopyState(node.getState());
            
            parent = node.getParent();

        }
        public void setState(State st)
        {
            state = st;
        }
        public State getState()
        {
            return state;
        }
        public void setParent(Node pt)
        {
            parent = pt;
        }
        public Node getParent()
        {
            return parent;
        }
        public void setChildArray(List<Node> ch)
        {
            childArray = ch;
        }
        public List<Node> getChildArray()
        {
            return childArray;
        }

        public Node getRandomChildNode()
        {
            Random rnd = new Random();
            int randomChildNum = rnd.Next(childArray.Count);
            return childArray[randomChildNum];
        }
        public Node getChildWithMaxScore()
        {
            if (childArray.Count == 0)
                return null;
            double max = int.MinValue;
            Node selectedNode = null;
            foreach (var item in childArray)
            {
                if (item.getState().getWinScore() >= max)
                {
                    max = item.getState().getWinScore();
                    selectedNode = item;
                }
            }
            return selectedNode;
        }
    }
}

