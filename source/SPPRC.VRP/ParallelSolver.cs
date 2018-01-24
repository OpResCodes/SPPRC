using SPPRC.Data.Datamodel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPPRC.VRP
{
    public class ParallelSolver
    {
        public ParallelSolver(Graph graph, SolverOptions options)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");
            if (options == null)
                throw new ArgumentNullException("options");

            _options = options;
            _graph = graph;
            _waitingQueue = new ConcurrentQueue<Node>();
        }

        public void Solve(int startNodeId)
        {
            Node startNode = _graph.Nodes.First(n => n.Id == startNodeId);
            Solve(startNode);
        }

        bool ExpandState(NodeState baseState, Arc expansionArc)
        {
            Node target = expansionArc.Destination;
            var vc = baseState.VirtualCost + expansionArc.Cost - expansionArc.DualCostValue;
            var res = ExtendResourceConsumption(baseState.Resources, expansionArc.ResourceConsumption);
            var expandedState = new NodeState(target, vc, res, baseState);
            expandedState.RealCost = baseState.RealCost + expansionArc.Cost;
            bool IsExpanded;

            if (IsFeasible(expandedState, target) && !IsDominated(expandedState, target.States))
            {
                expandedState.AssignToNode();
                IsExpanded = true;
            }
            else
            {
                expandedState.Detach();
                IsExpanded = false;
            }
            return IsExpanded;
        }

        NodeState CreateChildState(NodeState baseState, Arc expansionArc)
        {
            Node target = expansionArc.Destination;
            var vc = baseState.VirtualCost + expansionArc.Cost - expansionArc.DualCostValue;
            var res = ExtendResourceConsumption(baseState.Resources, expansionArc.ResourceConsumption);
            var expandedState = new NodeState(target, vc, res, baseState);
            expandedState.RealCost = baseState.RealCost + expansionArc.Cost;
            return expandedState;
        }

        bool IsFeasible(NodeState nsTest, Node nodeRef)
        {
            for (int i = 0; i < nodeRef.ResourceLimit.Length; i++)
            {
                if (nsTest.Resources[i] > nodeRef.ResourceLimit[i])
                {
                    return false;
                }
            }
            return true;
        }

        bool IsDominated(NodeState nsTest, IEnumerable<NodeState> nsReferenceCollection)
        {
            foreach (var refState in nsReferenceCollection)
            {
                if (IsDominated(nsTest, refState))
                {
                    return true;
                }
            }
            return false;
        }

        bool IsDominated(NodeState nsTest, NodeState nsReference)
        {
            //test cost
            if (nsTest.VirtualCost <= nsReference.VirtualCost)
            {
                return false;
            }
            //test resources
            for (int i = 0; i < nsTest.Resources.Length; i++)
            {
                if (nsTest.Resources[i] < nsReference.Resources[i])
                {
                    return false;
                }
            }
            //cost is greater and no resource is lower/equal
            return true;
        }

        public void Solve(Node startNode)
        {
            if (startNode == null)
                throw new ArgumentException("startNode");

            Debug.WriteLine("Solving.");

            _depotNodeId = startNode.Id;
            InitializeStates(startNode);
            _waitingQueue = new ConcurrentQueue<Node>();
            _waitingQueue.Enqueue(startNode);
            while (!_waitingQueue.IsEmpty)
            {
                Node nNxt;
                if (_waitingQueue.TryDequeue(out nNxt))
                {
                    ProcessNode(nNxt);
                    //Debug.WriteLine($"Processed: {nNxt.Id}");
                    //DumpStates();
                }
            }
            EliminateDominatedEndStates(startNode);
            Debug.WriteLine("Instance solved.");
            Debug.WriteLine($"Graph has {_graph.CountArcs.ToString("N0")} arcs and {_graph.CountNodes.ToString("N0")} nodes.");
        }

        private void EliminateDominatedEndStates(Node node)
        {
            //new states are not dominated, but processed (old) states could be dominated by new states
            var unprocessedStates = node.States.Where(s => !s.IsProcessed).ToList();

            if (unprocessedStates.Count > 0)
            {
                //check if unprocessed states dominate each other
                if (unprocessedStates.Count > 1)
                {
                    foreach (var state in unprocessedStates)
                    {
                        if (IsDominated(state, unprocessedStates))
                        {
                            state.IsDominated = true;
                        }
                    }
                }
            }
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
            //new states are not dominated, but processed (old) states could be dominated by new states
            var unprocessedStates = node.States.Where(s => !s.IsProcessed).ToList();
            var processedStates = node.States.Where(s => s.IsProcessed).ToArray();


            if (unprocessedStates.Count > 0)
            {
                //check if unprocessed states dominate each other
                if (unprocessedStates.Count > 1)
                {
                    foreach (var state in unprocessedStates)
                    {
                        if (IsDominated(state, unprocessedStates))
                        {
                            state.IsDominated = true;
                        }
                    }
                    unprocessedStates = unprocessedStates.Where(up => !up.IsDominated).ToList();
                }

                //dominance test for processed states:
                for (int i = 0; i < processedStates.Length; i++)
                {
                    if (IsDominated(processedStates[i], unprocessedStates))
                    {
                        processedStates[i].IsDominated = true;
                        //oldStates[i].Detach();
                    }
                }
                //expand states in parallel
                List<Task<HashSet<NodeState>>> expansionTasks = new List<Task<HashSet<NodeState>>>();
                int j = 0;
                int idx;
                List<NodeState> children = new List<NodeState>(unprocessedStates.Count * node.OutArcs.Count);
                do
                {
                    while (j < unprocessedStates.Count && expansionTasks.Count < _options.NumberOfThreads)
                    {
                        var taskData = new ExpansionTaskData(unprocessedStates[j], node.OutArcs);
                        expansionTasks.Add(MultiExpandState(taskData));
                        j++;
                    }
                    idx = Task.WaitAny(expansionTasks.ToArray());
                    var result = expansionTasks[idx].Result;
                    children.AddRange(result);
                    expansionTasks.RemoveAt(idx);
                } while (expansionTasks.Count > 0);
                //add created states now
                foreach (NodeState state in children)
                {
                    state.AssignToNode();
                    TryAddNodeToQueue(state.MyNode);
                }
            }
        }

        Task<HashSet<NodeState>> MultiExpandState(ExpansionTaskData data)
        {
            return Task.Factory.StartNew((arg) =>
            {
                var etd = (ExpansionTaskData) arg;
                HashSet<NodeState> children = new HashSet<NodeState>();
                for (int j = 0; j < etd.targetArcs.Count; j++)
                {
                    Node destination = etd.targetArcs[j].Destination;
                    if (etd.state.HasVisitedNode(destination.Id) && destination.Id != _depotNodeId)
                        continue;
                    var childState = CreateChildState(etd.state, etd.targetArcs[j]);
                    if (IsFeasible(childState, destination) && !IsDominated(childState, destination.States))
                    {
                        children.Add(childState);
                        ///node must be enqueued..
                    }
                }
                etd.state.IsProcessed = true;
                return children;
            }, data);
        }

        private class ExpansionTaskData
        {
            public ExpansionTaskData(NodeState nodeState, List<Arc> arcs)
            {
                state = nodeState;
                targetArcs = arcs;
            }
            internal NodeState state;
            internal List<Arc> targetArcs;
        }

        void TryAddNodeToQueue(Node node)
        {
            if (!_waitingQueue.Contains(node) && _depotNodeId != node.Id)
            {
                _waitingQueue.Enqueue(node);
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

        private void DumpStates()
        {
            Debug.WriteLine("------------------------------");
            foreach (var node in _graph.Nodes)
            {
                Debug.WriteLine($" Node {node.Id}");
                foreach (var state in node.States)
                {
                    Debug.WriteLine($"  {state.ToString()}");
                }
            }
        }

        private ConcurrentQueue<Node> _waitingQueue;

        private Graph _graph;
        private SolverOptions _options;

        private int _depotNodeId;
    }
}
