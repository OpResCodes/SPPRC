using System;

namespace SPPRC.Algorithm
{
    public class ResourceExtensionfunctions
    {
        public static double Simple(double x, double y)
        {
            double z = x + y;
            return Math.Max(0, z);
        }

    }
}
