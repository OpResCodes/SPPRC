using SPPRC.Model;
using System.Collections.Generic;
using System.Linq;

namespace SPPRC.Algorithm
{
    public class Solver
    {

        public Solver(SolverOptions options, Graph graph)
        {
            Graph = graph;
            GeneratedDuties = new List<Duty>();
            Options = options;
        }

        #region Methods

        public virtual void Calculate()
        {
            if (!Graph.CanCalculate)
            {
                return;
            }
            GeneratedDuties = new List<Duty>();
            GetFirstNodeState(Graph.StartNode);
            var ProcessNodes = Graph.Nodes.OrderBy((x) => x.Id);
            foreach (Node n in ProcessNodes)
            {
                CheckForDominance(n);
                GetStatesFromNode(n);
            }
            GetDuties(Graph.EndNode);
            Graph.NeedsCalculation = false;
        }

        protected void CheckForDominance(Node node)
        {
            int i = 0;
            int j = 0;

            while (i <= node.States.Count() - 2)
            {
                //select nodestate to compare
                NodeState ns1 = node.States[i];

                j = i + 1;
                //compare other nodestates below in list
                while (ns1.IsDominated == false && j <= node.States.Count - 1)
                {
                    //select next nodestate to compare
                    NodeState ns2 = node.States[j];
                    bool b = Options.DominanceCheckFunction(ns1, ns2);
                    /* 3 cases: 
                        * - ns1 dominated and removed from list -> end while
                        * - ns2 dominated and removed from list -> use same j to target next nodestate
                        * - no domination, list untouched -> use j+1 to target next nodestate
                        */

                    if (!b)
                    {
                        j++;
                    }
                }
                //ns1 is dominated or has checked all nodestates below its position in list
                //if ns1 is dominated -> use same i to target next nodestate, else use i+1 to target next nodestate
                if (!ns1.IsDominated)
                {
                    //move to next nodestate in list
                    i++;
                }
            }
        }

        protected void GetFirstNodeState(Node InitialNode)
        {
            NodeState initialstate = new NodeState(InitialNode, 0, Graph.ResourceInfo.IntitalValues, null);
            InitialNode.States.Add(initialstate);
        }

        protected void GetDuties(Node EndNode)
        {
            foreach (NodeState ns in EndNode.States)
            {
                if (ns.VirtualCost < 0)
                {
                    GeneratedDuties.Add(ns.ToDuty());
                }
            }
        }

        protected void GetStatesFromNode(Node fromNode)
        {
            for (int i = 0; i < fromNode.States.Count; i++)
            {
                for (int j = 0; j < fromNode.OutArcs.Count; j++)
                {
                    NodeState newState;
                    if (TryCreateNodeState(fromNode.States[i], fromNode.OutArcs[j], out newState))
                    {
                        newState.AssignToNode();
                    }
                }
            }
        }

        protected bool TryCreateNodeState(NodeState preState, Arc preArc, out NodeState nodeState)
        {
            bool nodeStateValid = true;
            int i = 0;
            double reducedCost;
            double[] rx = new double[Graph.ResourceInfo.ResCount];
            Node forNode = preArc.Destination;
            //Resourcevalidation
            while (nodeStateValid && i <= Graph.ResourceInfo.ResCount - 1)
            {
                rx[i] = Options.ResourceExtensionFunctions[i](preState.Resources[i], preArc.ResourceConsumption[i]);
                nodeStateValid = (
                    rx[i] <= forNode.ResourceConstraints[i].MaxValue && 
                    rx[i] >= forNode.ResourceConstraints[i].MinValue
                    );
                i++;
            }
            //if failed return
            if (!nodeStateValid)
            {
                nodeState = null;
            }
            else
            {
                //calc reduced cost
                reducedCost = preState.VirtualCost + preArc.Cost - preArc.DualCostValue;
                //check if cost-optimal path will end up with negative reduced cost
                nodeStateValid = (forNode.IsLowerBoundSet) ? (reducedCost + forNode.LowerBoundEstimate < 0) : true;
                //construct new NodeState and return if valid
                nodeState = (nodeStateValid) ? new NodeState(forNode, reducedCost, rx, preState) : null;
            }
            return nodeStateValid;
        }

        #endregion

        #region Properties

        public Graph Graph { get; protected set; }

        public List<Duty> GeneratedDuties { get; protected set; }

        public SolverOptions Options { get; set; }

        #endregion

    }



}
