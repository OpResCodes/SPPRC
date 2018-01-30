using SPPRC.Model;

namespace SPPRC.Algorithm.Core
{
    public class Delegates
    {
        //Dominance-check delegate
        public delegate bool DC_handler(NodeState ns1, NodeState ns2);

        //Resource Extension Function delegate (defined in seperate class)
        public delegate double REF_handler(double x, double y);
    }
}
