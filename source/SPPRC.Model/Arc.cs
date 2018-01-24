using System;

namespace SPPRC.Model
{
    public class Arc : IEntity
    {

        public Arc(int ArcId, Node o, Node d, double c, double rc, double[] resCon)
        {
            Id = ArcId;
            Origin = o;
            Destination = d;
            o.OutArcs.Add(this);
            d.InArcs.Add(this);
            Cost = c;
            DualCostValue = rc;
            ResourceConsumption = resCon;
            DriveTask = -1;
        }

        public Arc(int ArcId, Node o, Node d, double c, double rc, double[] resCon, int dTask) : this(ArcId, o, d, c, rc, resCon)
        {
            DriveTask = dTask;
        }

        #region Properties

        public int Id { get; set; }

        public Node Origin { get; private set; }

        public Node Destination { get; private set; }

        public double Cost { get; private set; }

        public double DualCostValue { get; set; }

        public double[] ResourceConsumption { get; private set; }

        public int DriveTask { get; private set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return String.Format("ID {0}: ({1})-({2})",
                Id.ToString(), Origin.Label.ToString(),Destination.Label.ToString());
        }

        #endregion

    }
}
