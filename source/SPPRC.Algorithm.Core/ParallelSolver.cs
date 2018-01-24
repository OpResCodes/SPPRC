using SPPRC.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SPPRC.Algorithm
{
    public class ParallelSolver : Solver
    {

        public ParallelSolver(SolverOptions options, Graph graph) : base(options,graph) { }

        public override void Calculate()
        {

            if (!Graph.CanCalculateParallel)
            {
                return;
            }

            GeneratedDuties = new List<Duty>();
            _calculationTasks = new List<Task<SubGraph>>();
            //set event handlers
            SetCalculationNotifications(true);
            GetFirstNodeState(Graph.StartNode);
            //Start first Path
            SubGraph firstPath = Graph.SubGraphs.Single((x) => x.Nodes.Contains(Graph.StartNode));
            _calculationTasks.Add(GetCalculationTask(firstPath));
            //WaitAllOneByOne
            while (_calculationTasks.Count > 0)
            {
                //logger.Info("Tasks: " + CalculationTasks.Count.ToString(),"Solver");
                Options.DEBUG_MAX_PARALLEL = Math.Max(_calculationTasks.Count, Options.DEBUG_MAX_PARALLEL);
                int idx = Task.WaitAny(_calculationTasks.ToArray());
                if (_calculationTasks[idx].Exception != null)
                    HandleAe(_calculationTasks[idx].Exception);

                SubGraph completedPath = _calculationTasks[idx].Result;
                _calculationTasks.RemoveAt(idx);
                //reduce results: [warum das hier und nicht schon im pfad processing?]
                for (int i = 0; i < completedPath.StateCollector.Count; i++)
                {
                    completedPath.StateCollector[i].AssignToNode();
                }
                //notify completion
                completedPath.OnCalculationComplete();
            }
            //Last Node
            GetDuties(Graph.EndNode);
            //detach eventhandlers
            SetCalculationNotifications(false);
            Graph.NeedsCalculation = false;
        }

        private void SetCalculationNotifications(bool on)
        {
            if (on && !_concurrencyEventsSet)
            {
                foreach (SubGraph p in Graph.SubGraphs)
                {
                    p.ReadyToCalculate += EnqueuePathCalculation;
                }
                _concurrencyEventsSet = true;
            }
            else if (!on && _concurrencyEventsSet)
            {
                foreach (SubGraph p in Graph.SubGraphs)
                {
                    p.ReadyToCalculate -= EnqueuePathCalculation;
                }
                _concurrencyEventsSet = false;
            }
        }

        private Task<SubGraph> GetCalculationTask(SubGraph p)
        {
            Task<SubGraph> calculationTask = Task.Factory.StartNew((arg) =>
            {
                SubGraph subGraph = (SubGraph) arg;
                for (int i = 0; i < subGraph.Nodes.Count; i++)
                {
                    CheckForDominance(subGraph.Nodes[i]);
                    GenerateStateListFromNode(subGraph.Nodes[i], subGraph);
                }
                return subGraph;
            }, p);
            return calculationTask;
        }

        private void GenerateStateListFromNode(Node fromNode, SubGraph path)
        {
            for (int i = 0; i < fromNode.States.Count; i++)
            {
                for (int j = 0; j < fromNode.OutArcs.Count; j++)
                {
                    NodeState newState;
                    if (TryCreateNodeState(fromNode.States[i], fromNode.OutArcs[j], out newState))
                    {
                        //assign the state to its node or collect the state for later assignment (due to concurrency)
                        if (path.Nodes.Contains(newState.MyNode))
                            newState.AssignToNode();
                        else
                            path.StateCollector.Add(newState);
                    }
                }
            }
        }

        private void EnqueuePathCalculation(object sender, EventArgs e)
        {
            var calcTask = GetCalculationTask((SubGraph) sender);
            _calculationTasks.Add(calcTask);
        }

        private void HandleAe(AggregateException ae)
        {
            ae = ae.Flatten();
            foreach (Exception item in ae.InnerExceptions)
            {
                Debug.WriteLine(ae.Message);
                Debug.WriteLine(ae.StackTrace);
            }
        }

        #region Fields

        private List<Task<SubGraph>> _calculationTasks;
        private bool _concurrencyEventsSet = false;

        #endregion
    }
}
