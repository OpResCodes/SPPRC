using SPPRC.Data.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPPRC.VRP.Tests
{
    public class TestGraphBuilder
    {

        public Graph Build()
        {
            List<Node> nodes = new List<Node>();
            List<Arc> arcs = new List<Arc>();
            for (int i = 1; i < 4; i++)
            {
                Node n = new Node(i, new double[] { 100 }, 0, 0);
                nodes.Add(n);
            }
            int arcCounter = 0;
            double[,] cost = new double[4, 4];
            cost[1, 2] = 2;
            cost[1, 3] = 3;
            cost[2, 3] = 4;

            for (int i = 1; i < 4 - 1; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    cost[j, i] = cost[i, j];
                    arcCounter++;
                    arcs.Add(new Arc(arcCounter, nodes[i - 1], nodes[j - 1], 1, cost[i, j], new double[] { 2 }));
                    arcCounter++;
                    arcs.Add(new Arc(arcCounter, nodes[j - 1], nodes[i - 1], 1, cost[j, i], new double[] { 2 }));
                }

            }
            return new Graph()
            {
                Arcs = arcs,
                Nodes = nodes,
                NeedsCalculation = true,
                StartNode = nodes[0]
            };
        }

    }
}
