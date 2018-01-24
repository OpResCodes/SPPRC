using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace SPPRC.VRP.Tests
{
    [TestClass]
    public class SampleGraph
    {
        [TestMethod]
        public void TestSmallSample()
        {
            var gb = new TestGraphBuilder();
            var graph = gb.Build();
            var solver = new Solver(graph);
            solver.Solve(1);
            foreach (var item in graph.StartNode.States)
            {
                Trace.WriteLine(item.ToString());
            }
            Assert.AreNotEqual(0, graph.StartNode.States.Count);
        }

        [TestMethod]
        public void TestSmallSampleParallel()
        {
            var gb = new TestGraphBuilder();
            var graph = gb.Build();
            var solver = new ParallelSolver(graph, new SolverOptions() { UseMultithreading = true });
            solver.Solve(1);
            foreach (var item in graph.StartNode.States)
            {
                Trace.WriteLine(item.ToString());
            }
            Assert.AreNotEqual(0, graph.StartNode.States.Count);
        }
    }
}
