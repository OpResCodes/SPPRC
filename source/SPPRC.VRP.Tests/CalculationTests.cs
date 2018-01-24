using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPPRC.VRP;
using System.Diagnostics;
using SPPRC.Data.Datamodel;
using System.Collections.Generic;

namespace SPPRC.VRP.Tests
{
    [TestClass]
    public class CalculationTests
    {
        string path = @"C:\Users\matth\Downloads\VRP_CG\spBase.gdx";
        [TestMethod]
        public void CanCalculate()
        {
            var loader = new GamsDataLoader();
            var graph = loader.LoadGraphBase(path);
            Solver solver = new Solver(graph);

            RandomizeDualValues(graph.Arcs);

            var sn = graph.Nodes.Find(n => n.Id == 1);
            solver.Solve(sn);
            foreach (var item in sn.States)
            {
                Trace.WriteLine(item.ToString());
            }
            Assert.AreNotEqual(0, sn.States.Count);
        }

        [TestMethod]
        public void CanCalculateParallel()
        {

        }

        void RandomizeDualValues(IEnumerable<Arc> arcs)
        {
            Random r = new Random();
            foreach (var item in arcs)
            {
                if (r.NextDouble() > 0.8)
                {
                    item.DualCostValue = Math.Round(r.NextDouble() * 200);
                }
            }
        }
    }
}
