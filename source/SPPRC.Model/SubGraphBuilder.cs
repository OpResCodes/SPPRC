using Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPPRC.Model
{
    internal class SubGraphBuilder
    {
        
        public SubGraph CreateNew(Node startNode)
        {
            _pcount++;
            SubGraph P = new SubGraph(_pcount);

            //add Startnode to path
            AddNode(startNode, P);

            //find Paths of direct predecessor nodes of Startnode
            Node[] predecessorNodes = startNode.PredecessorNodes;
            var PredecessorSubGraphs = predecessorNodes
                .Where(node => node.SubGraph != null)
                .Select(node => node.SubGraph).Distinct();

            //set each detected predecessor path to be observed by me
            foreach (SubGraph prePath in PredecessorSubGraphs)
            {
                SetupDependency(prePath, P);
            }
            _dependencies = P.DependencyList;

            //try to continue path
            Node n = startNode;
            bool searchnext;
            do
            {
                searchnext = TryGetNext(n, out n);
                if (searchnext)
                {
                    AddNode(n, P);
                }
            } while (searchnext);
            return P;
        }

        private bool TryGetNext(Node baseNode, out Node nextNode)
        {
            nextNode = baseNode;

            //Check if exact one successor meets the criterias to resume the path
            //if more than one successor meets the crterias the path ends here
            //if no successor meets the criterias the path ends here

            //successors
            var successors = baseNode.OutArcs.Select((y) => y.Destination).Distinct();
            //without path
            var freeNodes = successors.Where((x) => x.SubGraph == null);
            bool getNext = freeNodes.Count() > 0;
            if (getNext)
            {
                var unique_successors = EvaluateSuccessorsInParallel(freeNodes);
                //if exactly one sucessor is found, continue pathbuilding..
                getNext = unique_successors.Count() == 1;
                if (getNext)
                {
                    nextNode = unique_successors.First();
                }
            }       
            return getNext;
        }

        private List<Node> EvaluateSuccessorsInParallel(IEnumerable<Node> freeNodes)
        {
            List<Node> resultlist = new List<Node>();
            List<Task<Node>> tasklist = new List<Task<Node>>();

            foreach (Node potentialNext in freeNodes)
            {
                Task<Node> tnew = Task.Factory.StartNew(
                    (arg) =>
                    {
                        Node nodeToSearch = (Node) arg;
                        if (PathCompatibility(nodeToSearch))
                            return nodeToSearch;
                        else
                            return null;
                    }, potentialNext);
                tasklist.Add(tnew);
            }
            //WaitAllOneByOne
            while (tasklist.Count > 0)
            {
                //get next finished task
                int i = Task.WaitAny(tasklist.ToArray());

                try
                {
                    Node resultnode = tasklist[i].Result;
                    if (resultnode != null) resultlist.Add(resultnode);
                }
                catch (AggregateException ae)
                {
                    ae = ae.Flatten();
                    foreach (Exception ex in ae.InnerExceptions)
                    {
                        logger.write("Exception in getnext:\n" + ex.Message);
                    }
                }
                //remove finished task from todo
                tasklist.RemoveAt(i);
            }
            return resultlist;
        }

        private bool PathCompatibility(Node nodeToSearch)
        {
            //predecessors of node
            var predecessorNodes = nodeToSearch.InArcs.Select((x) => x.Origin);
            //predecessors of node found in the dependencylist, i.e. me or the paths I depend on
            var predecessorNodes_inPath = predecessorNodes.Where((z) => IsNodeInSubGraphs(z, _dependencies));
            //if all predecessors are in paths of dependencylist we can use it as next node
            if (predecessorNodes.Count() == predecessorNodes_inPath.Count())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsNodeInSubGraphs(Node n, List<SubGraph> pathlist)
        {
            bool r = false;
            int i = 0;
            while ((!r) && (i < pathlist.Count))
            {
                r = pathlist[i].Nodes.Contains(n);
                i++;
            }
            return r;
        }

        internal void AddNode(Node n, SubGraph P)
        {
            n.SubGraph = P;
            P.Nodes.Add(n);
        }

        internal void SetupDependency(SubGraph pathToObserve, SubGraph ObservingPath)
        {
            //listen to observed paths Completion event
            pathToObserve.CalculationCompleted += ObservingPath.PredecessorCompleted;
            //update number of observed paths
            ObservingPath.Dependencies++;
            //add complete dependency of observed path to observingPath dependency
            if (pathToObserve.DependencyList.Any())
            {
                ObservingPath.DependencyList = pathToObserve.DependencyList.Union(ObservingPath.DependencyList).ToList();
            }
        }

        #region Fields

        private List<SubGraph> _dependencies;
        private int _pcount = 0; 

        #endregion

    }
}
