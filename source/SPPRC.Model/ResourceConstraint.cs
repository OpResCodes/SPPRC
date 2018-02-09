using System;
using System.Collections.Generic;
using System.Text;

namespace SPPRC.Model
{
    public struct ResourceConstraint
    {
        public ResourceConstraint(double minimum, double maximum)
        {
            MaxValue = maximum;
            MinValue = minimum;
        }

        public double MaxValue { get; }

        public double MinValue { get; }

    }
}
