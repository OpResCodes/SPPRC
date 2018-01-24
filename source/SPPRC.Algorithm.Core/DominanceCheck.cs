using SPPRC.Model;

namespace SPPRC.Algorithm
{
    public class DominanceCheck
    {
        /// <summary>
        /// Returns True if domination in either direction was detected
        /// </summary>
        /// <param name="ns1">First NodeState</param>
        /// <param name="ns2">Second NodeState</param>
        /// <returns></returns>
        public static bool Simple(NodeState ns1, NodeState ns2)
        {
            NodeState hasLowerCost;
            NodeState hasHigherCost;
            int i=0;
            bool r = true;

            //determine direction of comparison by cost, or break if equal cost
            if (ns1.VirtualCost < ns2.VirtualCost)
            {
                hasLowerCost = ns1;
                hasHigherCost = ns2;
            }
            else if (ns1.VirtualCost > ns2.VirtualCost)
            {
                hasLowerCost = ns2;
                hasHigherCost = ns1;
            }
            else
            {
                return false;
            }

            //compare for each resource
            while ( (r) && (i < ns1.Resources.Length) )
            {
                r = (hasLowerCost.Resources[i] <= hasHigherCost.Resources[i]);
                i++;
            }

            //if domination detected, delete higherCost Nodestate
            hasHigherCost.IsDominated = r;

            //return if domination was detected
            return r;
        }

    }
}
