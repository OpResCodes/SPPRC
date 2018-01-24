using System.Collections.Generic;
using System.Linq;

namespace SPPRC.Model
{
    public class NodeState
    {
        public NodeState(
            Node TargetNode,
            double NodeStateCosts,
            double[] ResourceConsumption,
            NodeState LinkToPredecessor)
        {
            MyNode = TargetNode;
            MyPredecessor = LinkToPredecessor;
            Resources = ResourceConsumption;
            VirtualCost = NodeStateCosts;
            IsDominated = false;
            IsProcessed = false;
            MySuccessors = new List<NodeState>();
        }

        #region Properties

        public double VirtualCost { get; private set; }

        public double RealCost { get; set; }
        
        public double[] Resources { get; private set; }

        public bool IsDominated
        {
            get { return _isDominated; }
            set
            {
                if (_isDominated != value)
                {
                    _isDominated = value;
                    if (_isDominated)
                    {
                        //If dominated by another node this reference type should be gc'ed
                        Detach();
                    }
                }
            }
        }

        public bool IsProcessed { get; set; }

        public Node MyNode { get; private set; }

        public NodeState MyPredecessor { get; set; }

        public List<NodeState> MySuccessors { get; set; }

        #endregion

        #region Methods

        public void Detach()
        {
            if (MyNode.States.Contains(this))
            {
                MyNode.States.Remove(this);
            }
            MyPredecessor = null;
            MyNode = null;
            foreach (var successors in MySuccessors)
            {
                successors.Detach();
            }
            MySuccessors.Clear();
        }

        public void AssignToNode()
        {
            MyNode.States.Add(this);
        }

        public bool HasVisitedNode(int nodeId)
        {
            if (MyNode.Id == nodeId)
            {
                return true;
            }
            else if (MyPredecessor == null || MyPredecessor == this)
            {
                return false;
            }
            else
            {
                return MyPredecessor.HasVisitedNode(nodeId);
            }
        }

        private string PrintPath(NodeState ns)
        {
            if (ns.MyPredecessor != null)
            {
                return $"{PrintPath(ns.MyPredecessor)}-[{ns.MyNode.Label}]";
            }
            else
            {
                return $"[{ns.MyNode.Label}]";
            }
        }

        public Duty ToDuty()
        {
            Duty myDuty = new Duty(this);
            NodeState nodeState = this;

            do
            {
                //find arc from predecessor state
                Arc a = nodeState.MyPredecessor.MyNode.OutArcs.Where((x) => x.Destination == nodeState.MyNode).First();
                //add arc-cost to duty
                myDuty.Cost += a.Cost;
                //driving tasks
                if (a.DriveTask > -1) { myDuty.DrivingTasks.Add(a.DriveTask); }
                //next nodestate
                nodeState = nodeState.MyPredecessor;
            } while (nodeState.MyPredecessor != null);

            //Reduced cost of Duty
            myDuty.ReducedCost = this.VirtualCost;

            return myDuty;
        }

        public override string ToString()
        {
            System.Text.StringBuilder csb = new System.Text.StringBuilder();

            csb.AppendLine("Path: " + PrintPath(this));
            csb.Append("rc = " + this.VirtualCost.ToString() + ", ");
            csb.Append("Resources: (");
            for (int i = 0; i <= Resources.GetUpperBound(0); i++)
            {
                csb.Append(Resources[i].ToString() + ",");
            }
            csb.Remove(csb.Length - 1, 1);
            csb.Append(")\n");
            return csb.ToString();
        }

        #endregion

        #region Fields

        bool _isDominated;

        #endregion

    }

}
