using SPPRC.Data.Datamodel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SPPRC.VRP
{
    public class Solver
    {
        public Solver(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");

            _graph = graph;
            _waitingQueue = new Queue<Node>();
        }

        public void Solve(int startNodeId)
        {
            Node startNode = _graph.Nodes.First(n => n.Id == startNodeId);
            Solve(startNode);
        }

        public void Solve(Node startNode)
        {
            if (startNode == null)
                throw new ArgumentException("startNode");

            Debug.WriteLine("Solving.");

            _depotNodeId = startNode.Id;
            InitializeStates(startNode);
            _waitingQueue.Clear();
            _waitingQueue.Enqueue(startNode);
            while (_waitingQueue.Count > 0)
            {
                ProcessNode(_waitingQueue.Dequeue());
            }
            Debug.WriteLine("Instance solved.");
            Debug.WriteLine($"Graph has {_graph.CountArcs.ToString("N0")} arcs and {_graph.CountNodes.ToString("N0")} nodes.");
        }

        private void InitializeStates(Node startnode)
        {
            var initialState = new NodeState(
                TargetNode: startnode, 
                NodeStateCosts: 0, 
                ResourceConsumption: new double[] { 0 }, 
                LinkToPredecessor: null
                );
            initialState.AssignToNode();
        }

        private void ProcessNode(Node node)
        {
            //Debug.WriteLine($"Processing Node {node.Id}. Labels: {node.States.Count().ToString("N0")}");
            var currentStates = node.States.Where(s => !s.IsProcessed).ToArray();
            foreach (var state in currentStates)
            {
                if (!state.IsDominated)
                {
                    foreach (Arc a in node.OutArcs)
                    {
                        TryCreateLabel(state, a);
                    }
                    state.IsProcessed = true;
                }
            }
        }

        private void TryCreateLabel(NodeState baseLabel, Arc arc)
        {
            var target = arc.Destination;
            if (baseLabel.IsDominated)
                return;
            if (baseLabel.HasVisitedNode(target.Id) && (target.Id != _depotNodeId))
                return;

            var resCons = ExtendResourceConsumption(baseLabel.Resources, arc.ResourceConsumption);
            if (IsFeasibleResourceConsumption(resCons,target))
            {
                var rc = baseLabel.VirtualCost + arc.Cost - arc.DualCostValue;
                var extensionLabel = new NodeState(target, rc, resCons, baseLabel);

                var tempList = target.States.ToArray();
                foreach (NodeState existingNodestate in tempList)
                {
                    if (IsDominated(extensionLabel,existingNodestate))
                    {
                        if (extensionLabel.IsDominated)
                        {
                            break;
                        }
                    }
                }
                if (!extensionLabel.IsDominated)
                {
                    extensionLabel.AssignToNode();
                    baseLabel.MySuccessors.Add(extensionLabel);
                    extensionLabel.RealCost = baseLabel.RealCost + arc.Cost;
                    if (!_waitingQueue.Contains(target) && _depotNodeId != target.Id)
                    {
                        _waitingQueue.Enqueue(target);
                    }
                }
            }            
        }

        private double[] ExtendResourceConsumption(double[] labelResource, double[] arcResources)
        {
            double[] v = new double[labelResource.Length];
            for (int i = 0; i < labelResource.Length; i++)
            {
                v[i] = labelResource[i] + arcResources[i];
            }
            return v;
        }

        private bool IsFeasibleResourceConsumption(double[] value, Node node)
        {
            bool feasible = true;            
            for (int i = 0; i < value.Length; i++)
            {
                feasible = value[i] <= node.ResourceLimit[i];
                if (!feasible)
                    break;
            }
            return feasible;
        }

        private bool IsDominated(NodeState ns1, NodeState ns2)
        {
            NodeState hasLowerCost;
            NodeState hasHigherCost;
            int i = 0;
            bool r = true;

            //determine direction of comparison by cost, or break if equal cost
            if (ns1.VirtualCost < ns2.VirtualCost)
            {
                hasLowerCost = ns1;
                hasHigherCost = ns2;
            }
            else if (ns1.VirtualCost > ns2.VirtualCost)
            {
                hasLowerCost = ns2;
                hasHigherCost = ns1;
            }
            else
            {
                return false;
            }

            //compare for each resource
            while ((r) && (i < ns1.Resources.Length))
            {
                r = (hasLowerCost.Resources[i] <= hasHigherCost.Resources[i]);
                i++;
            }

            //if domination detected, delete higherCost Nodestate
            hasHigherCost.IsDominated = r;

            //return if domination was detected
            return r;
        }

        private Queue<Node> _waitingQueue;

        private Graph _graph;

        private int _depotNodeId;
    }

}
