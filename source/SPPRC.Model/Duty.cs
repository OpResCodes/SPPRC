using System.Collections.Generic;
using System.Text;

namespace SPPRC.Model
{
    public class Duty : IEntity
    {
        public Duty(NodeState ns, double c = 0)
        {
            Cost = c;
            DrivingTasks = new List<long>();
            NodeState = ns;
            DutyCounter++;
            Id = DutyCounter;
        }

        #region Properties

        public int Id { get; set; }

        public double Cost { get; set; }

        public double ReducedCost { get; set; }

        public List<long> DrivingTasks { get; set; }

        public NodeState NodeState { get; set; }

        public static int DutyCounter;

        #endregion

        #region Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(NodeState.ToString());
            sb.AppendFormat("Duty Costs: {0}\n", Cost.ToString());
            sb.AppendFormat("Reduced Costs: {0}", ReducedCost.ToString());
            return sb.ToString();
        }
        
        #endregion

    }
}
