using SPPRC.Model;
using System.Collections.Generic;

namespace SPPRC.Sample
{
    internal class SimpleExampleData
    {
        internal ResourceInfo rsinfo { get; private set; }
        internal List<Node> nodes { get; private set; }
        internal List<Arc> arcs { get; private set; }

        internal SimpleExampleData()
        {
            WriteRSInfo();
            WriteNodes();
            WriteArcs();
        }

       private void WriteRSInfo()
        {
            ResourceInfo rsnew = new ResourceInfo();
            rsnew.ResCount = 2;
            rsnew.GeneralLimit = new double[] { 225, 360 };
            rsnew.IntitalValues = new double[] { 0, 0 };
            rsinfo = rsnew;
        }

        private void WriteNodes()
        {
            nodes = new List<Node>();
            for(int i=1 ; i < 9; i++)
            {
                Node newnode = new Node(i,rsinfo.GeneralLimit, i, i);
                nodes.Add(newnode);
            }
        }

        private void WriteArcs()
        {
            Arc a1 = new Arc(1,nodes[0],nodes[1],100,0, new double[] {0,0});
            Arc a2 = new Arc(2,nodes[0], nodes[2], 145, 1000, new double[] { 80, 90 });
            Arc a3 = new Arc(3,nodes[0], nodes[3], 140, 1000, new double[] { 70, 80 });
            Arc a4 = new Arc(4,nodes[1], nodes[2], 10, 0, new double[] { 0, 20 });
            Arc a5 = new Arc(5,nodes[1], nodes[3], 10, 0, new double[] { 0, 20 });
            Arc a6 = new Arc(6,nodes[1], nodes[4], 10, 0, new double[] { 0, 20 });
            Arc a7 = new Arc(7,nodes[2], nodes[4], 50, 1000, new double[] { 90, 100 });
            Arc a8 = new Arc(8,nodes[2], nodes[5], 10, 0, new double[] { -255, 60 });
            Arc a9 = new Arc(9,nodes[3], nodes[4], 10, 0, new double[] { -255, 50 });
            Arc a10 = new Arc(10,nodes[3], nodes[5], 60, 1000, new double[] { 110, 120 });
            Arc a11 = new Arc(11,nodes[4], nodes[6], 5, 0, new double[] { 0, 10 });
            Arc a12 = new Arc(12,nodes[4], nodes[7], 50, 1000, new double[] { 90, 100 });
            Arc a13 = new Arc(13,nodes[5], nodes[6], 10, 0, new double[] { 0, 20 });
            Arc a14 = new Arc(14,nodes[5], nodes[7], 45, 1000, new double[] { 80, 90 });
            Arc a15 = new Arc(15,nodes[6], nodes[7], 0, 0, new double[] { 0, 0 });
            arcs = new List<Arc>() { a1, a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13,a14,a15 };
        }

    }
}
