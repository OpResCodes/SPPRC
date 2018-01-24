//using SPPRC.Algorithm;
//using SPPRC.Data.Datamodel;
//using SPPRC.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Diagnostics;


//namespace SolverTests
//{
//    [TestClass]
//    public class SolverResultSynchronisation
//    {

//        Graph _graph;
//        Solver _solver;
//        [TestMethod]
//        [TestCategory("Result Synchronisation")]
//        public void CompareCalcResults()
//        {
//            _graph = new Graph();
//            _solver = new Solver(_graph);
//            var uow = new RandomUOW(100);
//            _graph.LoadInputData(uow);
//            _graph.generate_Paths();

//            // solve standard:
//            _solver.Calculate(false);
//            //save duties

//            List<Duty> firstDuties = new List<Duty>();
//            firstDuties.AddRange(_solver.GeneratedDuties);

//            _graph.WarmReset();

//            _solver.Calculate(true);

//            var dsc_par = Duty.dutycounter;

//            Debug.WriteLine(String.Format("nsc_par:{0}, dsc_par:{1}", nsc_par.ToString(), dsc_par.ToString()));
//            if (!(nsc_par == nsc && dsc_par == dsc))
//            { Assert.Fail("Number of States/Duties mismatch."); }

//            foreach (Duty fi in firstDuties)
//            {
//                double rc = fi.ReducedCost;
//                double c = fi.Cost;
//                double[] res = fi.myState.Resources;

//                if (!_solver.GeneratedDuties.Exists((x) => (
//                    x.ReducedCost == rc && 
//                    x.myState.Resources[0] == res[0] &&
//                    x.myState.Resources[1] == res[1] &&
//                    x.Cost == c)))
//                {
//                    Assert.Fail("Duty information mismatch.");
//                }

//            }

//            Assert.IsTrue(true);
//        }

//    }
//}
