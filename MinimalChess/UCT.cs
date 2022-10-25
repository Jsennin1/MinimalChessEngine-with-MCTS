using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MinimalChess
{
    public class UCT
    {
        public static double uctValue(
          int totalVisit, double nodeWinScore, int nodeVisit)
        {
            if (nodeVisit == 0)
            {
                return int.MaxValue;
            }
            return ((double)nodeWinScore / (double)nodeVisit)
              + 1.41 * Math.Sqrt(Math.Log(totalVisit) / (double)nodeVisit);
        }

        public static Node findBestNodeWithUCT(Node node)
        {
            int parentVisit = node.getState().getVisitCount();
            double max = int.MinValue;
            Node selectedNode = null;
            foreach (var item in node.getChildArray())
            {
                double uctVal = uctValue(parentVisit, item.getState().getWinScore(), item.getState().getVisitCount());
                if (uctVal >= max)
                {
                    max = uctVal;
                    selectedNode = item;
                }
            }
            return selectedNode;
        }
    }
}
