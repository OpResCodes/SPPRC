using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPPRC.VRP
{
    public class SolverOptions
    {

        public SolverOptions()
        {
            NumberOfThreads = System.Environment.ProcessorCount;
        }

        public bool UseMultithreading { get; set; }

        public int NumberOfThreads { get; set; }

    }
}
