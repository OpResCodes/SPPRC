using System.Collections.Generic;

namespace SPPRC.Model
{
    public class Node : IIdentify
    {

        public Node(int NodeId, string newLabel, ResourceConstraint[] constraints, int newTime_id, int newLocation_id)
        {
            Id = NodeId;
            Label = newLabel;
            OutArcs = new List<Arc>();
            InArcs = new List<Arc>();
            States = new List<NodeState>();

            LowerBoundEstimate = 0;
            IsLowerBoundSet = false;
            ResourceConstraints = constraints;
            NodeLocationId = newLocation_id;
            NodeTimeId = newTime_id;
        }

        public Node(int NodeId, ResourceConstraint[] constraints, int newTime_id, int newLocation_id) 
            : this(NodeId, string.Empty, constraints, newTime_id, newLocation_id)
        {
            Label = NodeId.ToString();
        }

        #region Properties

        
        public int Id { get; set; }

        public string Label { get; set; }

        public int NodeLocationId { get; private set; }

        public int NodeTimeId { get; private set; }

        public ResourceConstraint[] ResourceConstraints { get; private set; }

        public double LowerBoundEstimate { get; set; }

        public bool IsLowerBoundSet { get; set; }

        public List<Arc> InArcs { get; private set; }

        public List<Arc> OutArcs { get; private set; }

        public List<NodeState> States { get; set; }

        public SubGraph SubGraph { get; set; } 
        
        public Node[] PredecessorNodes
        {
            get
            {
                return GetNodesFromArcs(InArcs, true);
            }
        }

        public Node[] SuccessorNodes
        {
            get
            {
                return GetNodesFromArcs(OutArcs, false);
            }
        }
        #endregion

        #region Methods

        private Node[] GetNodesFromArcs(List<Arc> list, bool getOrigin)
        {
            List<Node> L = new List<Node>();
            if (InArcs != null && list.Count > 0)
            {
                HashSet<int> ids = new HashSet<int>();
                for (int i = 0; i < list.Count; i++)
                {
                    Node n = (getOrigin) ? list[i].Origin : list[i].Destination;
                    if (!ids.Contains(n.Id))
                    {
                        ids.Add(n.Id);
                        L.Add(n);
                    }
                }
            }
            return L.ToArray();
        }

        public override string ToString()
        {
            return Label;
        }

        public void ClearStates()
        {
            if (States == null || States.Count == 0)
                return;
            foreach (var state in States)
            {
                state.Detach();
            }
        }

        #endregion

    }
}
