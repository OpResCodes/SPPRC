using static SPPRC.Algorithm.Core.Delegates;

namespace SPPRC.Algorithm
{
    public sealed class SolverOptions
    {

        public SolverOptions(REF_handler[] resourceExtensionFunctions)
        {
            ResourceExtensionFunctions = resourceExtensionFunctions;
            DominanceCheckFunction = DominanceCheck.Simple;
            DEBUG_MAX_PARALLEL = 0;
        }

        public SolverOptions(int numberOfResources)
        {
            DominanceCheckFunction = DominanceCheck.Simple;
            DEBUG_MAX_PARALLEL = 0;
            ResourceExtensionFunctions = new REF_handler[numberOfResources];
            for (int i = 0; i < numberOfResources; i++)
            {
                ResourceExtensionFunctions[i] = ResourceExtensionfunctions.Simple;
            }
        }

        public int DEBUG_MAX_PARALLEL { get; set; }

        public DC_handler DominanceCheckFunction { get; set; }

        public REF_handler[] ResourceExtensionFunctions { get; set; }

    }
}
