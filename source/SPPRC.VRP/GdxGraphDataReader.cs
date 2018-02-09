using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAMS;
using SPPRC.Model;

namespace SPPRC.VRP
{
    internal class GdxGraphDataReader
    {

        internal Graph ReadGraph(GAMSDatabase gdxFile)
        {
            Node[] nodes = ReadNodes(gdxFile);
            Arc[] arcs = GetArcs(gdxFile, nodes);
            Graph graph = new Graph();
            graph.Arcs.AddRange(arcs);
            graph.Nodes.AddRange(nodes);
            (Node, Node) pair = GetSourceAndSink(gdxFile, nodes);
            graph.StartNode = pair.Item1;
            graph.EndNode = pair.Item2;
            graph.ResourceInfo = new ResourceInfo()
            {
                ResCount = 1,
                IntitalValues =  new double[] { 0 }
            };
            throw new ArgumentException();
        }

        private (Node, Node) GetSourceAndSink(GAMSDatabase gdxFile, Node[] nodes)
        {
            string src = gdxFile.GetSet(_str.SourceNode).FirstRecord().Keys[0];
            string snk = gdxFile.GetSet(_str.SinkNode).FirstRecord().Keys[0];

            Node sourceNode = nodes.FirstOrDefault(n => n.Label == src);
            Node sinkNode = nodes.FirstOrDefault(n => n.Label == snk);
            if (sourceNode == null || sinkNode == null)
                throw new ArgumentException("Source/Sink not defined.");
            return (sourceNode, sinkNode);
        }

        private Arc[] GetArcs(GAMSDatabase gdxFile, Node[] nodes)
        {
            Random rand = new Random(); //for debug run

            GAMSParameter par = gdxFile.GetParameter(_str.FzMatrix);
            Arc[] arcs = new Arc[par.NumberRecords];
            int c = 0;
            var nLookup = nodes.ToDictionary(n => n.Label);
            foreach (GAMSParameterRecord rec in par)
            {
                var n1 = nLookup[rec.Keys[0]];
                var n2 = nLookup[rec.Keys[1]];
                var v = rec.Value;
                //debug run with random dual
                Arc a = new Arc(c, n1, n2, v, v - rand.Next(0,100), new double[] { v });
                arcs[c] = a;
                c++;
            }
            return arcs;
        }

        private Node[] ReadNodes(GAMSDatabase gdxFile)
        {
            var n = gdxFile.GetSet(_str.NodeSet);
            var s = gdxFile.GetParameter(_str.StartTimes);
            var e = gdxFile.GetParameter(_str.EndTimes);
            Node[] nodes = new Node[n.NumberRecords];
            double v = 0;
            double w = 0;
            int c = 0;
            foreach (GAMSSetRecord r in n)
            {
                var startTime = s.FindRecord(r.Keys);
                var endTime = s.FindRecord(r.Keys);
                v = (startTime != null) ? startTime.Value : 0.0;
                w = (endTime != null) ? endTime.Value : 0.0;
                ResourceConstraint[] constraint = new ResourceConstraint[] { new ResourceConstraint(v, w) };
                nodes[c] = new Node(c, r.Keys[0], constraint, c, c);
                c++;
            }
            return nodes;
        }

        private GdxSymbolStrings _str;
    }

    internal class GdxSymbolStrings
    {
        internal string NodeSet = "i";
        internal string StartTimes = "fzST";
        internal string EndTimes = "fzET";
        internal string FzMatrix = "fz";
        internal string SourceNode = "src";
        internal string SinkNode = "snk";
    }
}
