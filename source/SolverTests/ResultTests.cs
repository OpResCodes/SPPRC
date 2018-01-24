using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPPRC.Algorithm;
using SPPRC.Model;
using SPPRC.Sample;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SolverTests
{
    [TestClass]
    public class ResultTests
    {

        private Graph _testgraph;
        private Solver _solver;

        private void Calc()
        {
            GetSampleGraph();
            _solver = new Solver(new SolverOptions(),_testgraph);
            _solver.Calculate();
        }

        void GetSampleGraph()
        {
            var loader = new GraphLoader();
            _testgraph = loader.LoadInputData(new SimpleExampleDataSet());
        }

        private void CalcParallel()
        {
            GetSampleGraph();
            _testgraph.BuildSubGraphs();
            _solver = new ParallelSolver(new SolverOptions(), _testgraph);
            _solver.Calculate();
        }

        public ResultTests()
        {
            CalcParallel();
            //Calc();
        }

        [TestMethod]
        [TestCategory("Expected States")]
        public void NodeStates8()
        {
            Queue<double[]> q = new Queue<double[]>();
            q.Enqueue(new double[] { -840, 90, 120 });
            q.Enqueue(new double[] { -1800, 90, 230 });
            q.Enqueue(new double[] { -1800, 80, 240 });
            q.Enqueue(new double[] { 115, 0, 30 });
            q.Enqueue(new double[] { -1800, 170, 200 });
            q.Enqueue(new double[] { -845, 0, 140 });

            bool tester = true;
            bool finalResult = true;
            List<NodeState> l = _testgraph.EndNode.States;
            Debug.WriteLine("--States--");
            Debug.Indent();
            while(q.Count > 0)
            {
                double[] d = q.Dequeue();
                tester = l.Exists((y)=> y.VirtualCost == d[0] && y.Resources[0] == d[1] && y.Resources[1] == d[2]);
                Debug.WriteLine(String.Format("({0},{1},{2}) : {3}",d[0].ToString(),d[1].ToString(),d[2].ToString(), tester.ToString()));
                if (!tester) finalResult = false;
            }
            Debug.Unindent();
            Debug.WriteLine("--End--");
            Assert.IsTrue(finalResult);  

        }

        [TestMethod]
        [TestCategory("Expected States")]
        public void NodeStates7()
        {
            Queue<double[]> q = new Queue<double[]>();
            q.Enqueue(new double[] { 115, 0, 30 });
            q.Enqueue(new double[] { -1800, 170, 200 });
            q.Enqueue(new double[] { -835, 90, 130 });
            q.Enqueue(new double[] { -845, 0, 140 });

            bool tester = true;
            bool finalResult = true;
            List<NodeState> l = _testgraph.Nodes[6].States;
            Debug.WriteLine("--States--");
            Debug.Indent();
            while (q.Count > 0)
            {
                double[] d = q.Dequeue();
                tester = l.Exists((y) => y.VirtualCost == d[0] && y.Resources[0] == d[1] && y.Resources[1] == d[2]);
                Debug.WriteLine(String.Format("({0},{1},{2}) : {3}", d[0].ToString(), d[1].ToString(), d[2].ToString(), tester.ToString()));
                if (!tester) finalResult = false;
            }
            Debug.Unindent();
            Debug.WriteLine("--End--");
            Assert.IsTrue(finalResult);

        }

        [TestMethod]
        [TestCategory("Expected States")]
        public void NodeStates6()
        {
            Queue<double[]> q = new Queue<double[]>();
            q.Enqueue(new double[] { -845, 0, 150 });
            q.Enqueue(new double[] { 120, 0, 80 });
            q.Enqueue(new double[] { -1800, 180, 200 });
            q.Enqueue(new double[] { -830, 110, 140 });

            bool tester = true;
            bool finalResult = true;
            List<NodeState> l = _testgraph.Nodes[5].States;
            Debug.WriteLine("--States--");
            Debug.Indent();
            while (q.Count > 0)
            {
                double[] d = q.Dequeue();
                tester = l.Exists((y) => y.VirtualCost == d[0] && y.Resources[0] == d[1] && y.Resources[1] == d[2]);
                Debug.WriteLine(String.Format("({0},{1},{2}) : {3}", d[0].ToString(), d[1].ToString(), d[2].ToString(), tester.ToString()));
                if (!tester) finalResult = false;
            }
            Debug.Unindent();
            Debug.WriteLine("--End--");
            Assert.IsTrue(finalResult);

        }

        [TestMethod]
        [TestCategory("Expected States")]
        public void NodeStates5()
        {
            Queue<double[]> q = new Queue<double[]>();
            q.Enqueue(new double[] { 110, 0, 20 });
            q.Enqueue(new double[] { -1805, 170, 190 });
            q.Enqueue(new double[] { -840, 90, 120 });
            q.Enqueue(new double[] { -850, 0, 130 });

            bool tester = true;
            bool finalResult = true;
            List<NodeState> l = _testgraph.Nodes[4].States;
            Debug.WriteLine("--States--");
            Debug.Indent();
            while (q.Count > 0)
            {
                double[] d = q.Dequeue();
                tester = l.Exists((y) => y.VirtualCost == d[0] && y.Resources[0] == d[1] && y.Resources[1] == d[2]);
                Debug.WriteLine(String.Format("({0},{1},{2}) : {3}", d[0].ToString(), d[1].ToString(), d[2].ToString(), tester.ToString()));
                if (!tester) finalResult = false;
            }
            Debug.Unindent();
            Debug.WriteLine("--End--");
            Assert.IsTrue(finalResult);

        }

        [TestMethod]
        [TestCategory("Expected States")]
        public void NodeStates4()
        {
            Queue<double[]> q = new Queue<double[]>();
            q.Enqueue(new double[] { -860, 70, 80 });
            q.Enqueue(new double[] { 110, 0, 20 });

            bool tester = true;
            bool finalResult = true;
            List<NodeState> l = _testgraph.Nodes[3].States;
            Debug.WriteLine("--States--");
            Debug.Indent();
            while (q.Count > 0)
            {
                double[] d = q.Dequeue();
                tester = l.Exists((y) => y.VirtualCost == d[0] && y.Resources[0] == d[1] && y.Resources[1] == d[2]);
                Debug.WriteLine(String.Format("({0},{1},{2}) : {3}", d[0].ToString(), d[1].ToString(), d[2].ToString(), tester.ToString()));
                if (!tester) finalResult = false;
            }
            Debug.Unindent();
            Debug.WriteLine("--End--");
            Assert.IsTrue(finalResult);

        }

        [TestMethod]
        [TestCategory("Expected States")]
        public void NodeStates3()
        {
            Queue<double[]> q = new Queue<double[]>();
            q.Enqueue(new double[] { -855, 80, 90 });
            q.Enqueue(new double[] { 110, 0, 20 });

            bool tester = true;
            bool finalResult = true;
            List<NodeState> l = _testgraph.Nodes[2].States;
            Debug.WriteLine("--States--");
            Debug.Indent();
            while (q.Count > 0)
            {
                double[] d = q.Dequeue();
                tester = l.Exists((y) => y.VirtualCost == d[0] && y.Resources[0] == d[1] && y.Resources[1] == d[2]);
                Debug.WriteLine(String.Format("({0},{1},{2}) : {3}", d[0].ToString(), d[1].ToString(), d[2].ToString(), tester.ToString()));
                if (!tester) finalResult = false;
            }
            Debug.Unindent();
            Debug.WriteLine("--End--");
            Assert.IsTrue(finalResult);

        }

        [TestMethod]
        [TestCategory("Expected States")]
        public void NodeStates2()
        {
            Queue<double[]> q = new Queue<double[]>();
            q.Enqueue(new double[] { 100, 0, 0 });

            bool tester = true;
            bool finalResult = true;
            List<NodeState> l = _testgraph.Nodes[1].States;
            Debug.WriteLine("--States--");
            Debug.Indent();
            while (q.Count > 0)
            {
                double[] d = q.Dequeue();
                tester = l.Exists((y) => y.VirtualCost == d[0] && y.Resources[0] == d[1] && y.Resources[1] == d[2]);
                Debug.WriteLine(String.Format("({0},{1},{2}) : {3}", d[0].ToString(), d[1].ToString(), d[2].ToString(), tester.ToString()));
                if (!tester) finalResult = false;
            }
            Debug.Unindent();
            Debug.WriteLine("--End--");
            Assert.IsTrue(finalResult);

        }

        [TestMethod]
        [TestCategory("Expected States")]
        public void NodeStates1()
        {
            Queue<double[]> q = new Queue<double[]>();
            q.Enqueue(new double[] { 0, 0, 0 });

            bool tester = true;
            bool finalResult = true;
            List<NodeState> l = _testgraph.Nodes[0].States;
            Debug.WriteLine("--States--");
            Debug.Indent();
            while (q.Count > 0)
            {
                double[] d = q.Dequeue();
                tester = l.Exists((y) => y.VirtualCost == d[0] && y.Resources[0] == d[1] && y.Resources[1] == d[2]);
                Debug.WriteLine(String.Format("({0},{1},{2}) : {3}", d[0].ToString(), d[1].ToString(), d[2].ToString(), tester.ToString()));
                if (!tester) finalResult = false;
            }
            Debug.Unindent();
            Debug.WriteLine("--End--");
            Assert.IsTrue(finalResult);

        }


        [TestMethod]
        [TestCategory("Final Duties")]
        public void Duty1()
        {
            var duties = _solver.GeneratedDuties;
            bool check;

            check = duties.Exists((x) => x.ReducedCost == -840 && x.NodeState.Resources[0] == 90 && x.NodeState.Resources[1] == 120);
            Assert.IsTrue(check);
        }

        [TestMethod]
        [TestCategory("Final Duties")]
        public void Duty2()
        {
            var duties = _solver.GeneratedDuties;
            bool check;

            check = duties.Exists((x) => x.ReducedCost == -1800 && x.NodeState.Resources[0] == 90 && x.NodeState.Resources[1] == 230);
            Assert.IsTrue(check);
        }

        [TestMethod]
        [TestCategory("Final Duties")]
        public void Duty3()
        {
            var duties = _solver.GeneratedDuties;
            bool check;

            check = duties.Exists((x) => x.ReducedCost == -1800 && x.NodeState.Resources[0] == 80 && x.NodeState.Resources[1] == 240);
            Assert.IsTrue(check);
        }

        [TestMethod]
        [TestCategory("Final Duties")]
        public void Duty4()
        {
            var duties = _solver.GeneratedDuties;
            bool check;

            check = duties.Exists((x) => x.ReducedCost == -1800 && x.NodeState.Resources[0] == 170 && x.NodeState.Resources[1] == 200);
            Assert.IsTrue(check);
        }

        [TestMethod]
        [TestCategory("Final Duties")]
        public void Duty5()
        {
            var duties = _solver.GeneratedDuties;
            bool check;

            check = duties.Exists((x) => x.ReducedCost == -845 && x.NodeState.Resources[0] == 0 && x.NodeState.Resources[1] == 140);
            Assert.IsTrue(check);
        }

        [TestMethod]
        [TestCategory("States Counting")]
        public void Node1()
        {
            Node n0 = _testgraph.Nodes[0];
            var y = n0.States.Count;
            Assert.AreEqual(y, 1);
        }

        [TestMethod]
        [TestCategory("States Counting")]
        public void Node2()
        {
            Node n0 = _testgraph.Nodes[1];
            var y = n0.States.Count;
            Assert.AreEqual(y, 1);
        }

        [TestMethod]
        [TestCategory("States Counting")]
        public void Node3()
        {
            Node n0 = _testgraph.Nodes[2];
            var y = n0.States.Count;
            Assert.AreEqual(y, 2);
        }

        [TestMethod]
        [TestCategory("States Counting")]
        public void Node4()
        {
            Node n0 = _testgraph.Nodes[3];
            var y = n0.States.Count;
            Assert.AreEqual(y, 2);
        }

        [TestMethod]
        [TestCategory("States Counting")]
        public void Node5()
        {
            Node n0 = _testgraph.Nodes[4];
            var y = n0.States.Count;
            Assert.AreEqual(y, 4);
        }

        [TestMethod]
        [TestCategory("States Counting")]
        public void Node6()
        {
            Node n0 = _testgraph.Nodes[5];
            var y = n0.States.Count;
            Assert.AreEqual(y, 4);
        }

        [TestMethod]
        [TestCategory("States Counting")]
        public void Node7()
        {
            Node n0 = _testgraph.Nodes[6];
            var y = n0.States.Count;
            Assert.AreEqual(y, 4);
        }

        [TestMethod]
        [TestCategory("States Counting")]
        public void Node8()
        {
            Node n0 = _testgraph.Nodes[7];
            var y = n0.States.Count;
            Assert.AreEqual(y, 6);
        }

    }
}
