using static SPPRC.Algorithm.Core.Delegates;

namespace SPPRC.Algorithm
{
    public sealed class SolverOptions
    {

        public SolverOptions()
        {
            DominanceCheckFunction = DominanceCheck.Simple;
            ResourceExtensionFunction = ResourceExtensionfunctions.Simple;
            DEBUG_MAX_PARALLEL = 0;
        }

        public int DEBUG_MAX_PARALLEL { get; set; }

        public DC_handler DominanceCheckFunction { get; set; }

        public REF_handler ResourceExtensionFunction { get; set; }

    }
}
