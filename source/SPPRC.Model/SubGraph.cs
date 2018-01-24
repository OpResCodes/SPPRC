using System;
using System.Collections.Generic;
using System.Linq;

namespace SPPRC.Model
{
    public class SubGraph
    {
        public SubGraph(int PathId)
        {
            Id = PathId;
            Nodes = new List<Node>();
            DependencyList = new List<SubGraph>();
            DependencyList.Add(this);
            StateCollector = new List<NodeState>();
            Dependencies = 0;
            _dependenciesCompleted = 0;
        }

        #region Properties

        public int Id { get; private set; }

        public int Dependencies { get; set; }

        public int RegisteredForwardDependencies
        {
            get
            {
                var handler = CalculationCompleted;
                return (handler == null) ? 0 : handler.GetInvocationList().Count();
            }
        }
        
        public List<SubGraph> DependencyList { get; set; }

        public List<Node> Nodes { get; }

        public List<NodeState> StateCollector { get; }

        #endregion

        #region Methods

        public void OnCalculationComplete()
        {
            CalculationCompleted?.Invoke(this, new EventArgs());
        }

        private void OnReadyToCalculate()
        {
            ReadyToCalculate?.Invoke(this, new EventArgs());
        }

        internal void PredecessorCompleted(object sender, EventArgs e)
        {
            _dependenciesCompleted++;
            if (Dependencies == _dependenciesCompleted)
                OnReadyToCalculate();
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> CalculationCompleted;
        public event EventHandler<EventArgs> ReadyToCalculate;

        #endregion

        #region Fields

        internal int _dependenciesCompleted;

        #endregion

    }
}
