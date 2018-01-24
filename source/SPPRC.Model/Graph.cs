using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SPPRC.Model
{
    public class Graph
    {

        public Graph()
        {
            ResetGraph();
        }
        
        #region Properties

        public bool NeedsCalculation { get; set; }

        public GraphStats Stats
        {
            get
            {
                var stats = new GraphStats();
                stats.ArcCount = Arcs.Count;
                stats.NodeCount = Nodes.Count;
                if (SubGraphs != null && SubGraphs.Any())
                {
                    stats.SubGraphCount = SubGraphs.Count();
                }
                else
                {
                    stats.SubGraphCount = 0;
                }
                stats.NodeStatesCount = Nodes.Select((n) => n.States).Sum((nsl) => nsl.Count);
                return stats;
            }
        }
        
        public bool CanCalculate
        {
            get
            {
                return (IsDataAvailable && NeedsCalculation);
            }
        }

        public bool CanCalculateParallel
        {
            get
            {
                return (CanCalculate && (SubGraphs != null && SubGraphs.Count > 0));
            }
        }
        
        public ResourceInfo ResourceInfo { get; set; }

        public List<SubGraph> SubGraphs { get; set; }

        public List<Node> Nodes { get; set; }

        public List<Arc> Arcs { get; set; }

        public Node StartNode { get; set; }

        public Node EndNode { get; set; }

        private bool IsDataAvailable
        {
            get
            {
                return !((Nodes == null || Nodes.Count == 0) || (Arcs == null || Arcs.Count == 0));
            }
        }

        #endregion

        #region Methods
        
        public void BuildSubGraphs()
        {
            if (!CanCalculate)
            {
                return;
            }

            SubGraphs = new List<SubGraph>();
            SubGraphBuilder subGraphBuilder = new SubGraphBuilder();
            foreach (Node n in Nodes)
            {
                if (n.SubGraph == null)
                {
                    SubGraphs.Add(subGraphBuilder.CreateNew(n));
                }
            }
        }

        public void ResetGraph()
        {
            Arcs = new List<Arc>();
            Nodes = new List<Node>();
            SubGraphs = new List<SubGraph>();
            StartNode = null;
            EndNode = null;
            NeedsCalculation = true;
        }

        public void WarmReset()
        {
            if (SubGraphs != null)
            {
                foreach (SubGraph p in SubGraphs)
                {
                    p._dependenciesCompleted = 0;
                    p.StateCollector.Clear();
                }
            }
            Nodes.ForEach(node => node.ClearStates());
            Duty.DutyCounter = 0;
            NeedsCalculation = true;
        }
  
        public override string ToString()
        {
            string f = "{0};{1};{2};{3};{4}";
            string rf = string.Empty;
            for (int i = 1; i <= ResourceInfo.ResCount; i++)
            {
                rf += ";r" + i.ToString();
            }


            StringBuilder csb = new StringBuilder();
            csb.AppendLine("i;j;c(i,j);dual(i,j);d_task(i,j)" + rf);

            foreach (Arc a in Arcs)
            {
                string rc = string.Empty;
                for (int i = 0; i < ResourceInfo.ResCount; i++)
                {
                    rc += ";" + a.ResourceConsumption[i].ToString();
                }

                csb.AppendLine(String.Format(f,
                    a.Origin.Label, 
                    a.Destination.Label, 
                    a.Cost.ToString(), 
                    a.DualCostValue.ToString(), 
                    a.DriveTask.ToString())
                    + rc);
            }

            return csb.ToString();
        }

        #endregion

    }
}
