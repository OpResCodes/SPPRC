using Evaluation;
using SPPRC.Algorithm;
using SPPRC.Model;
using SPPRC.Model;
using System;

namespace ConsoleUI
{
    class Program
    {

        private static int _n;

        static void Main(string[] args)
        {
            _n = 600;
            Console.WriteLine("Welcome to the SPPRC 2013 Project.");
            ComparisonTest(3);
            //SimpleDataTest();
            //ParallelTest();
            Console.WriteLine("Test finished.");
            Console.ReadLine();
        }

        private static void ComparisonTest(int trials_set)
        {
            //GraphLoader loader = new GraphLoader();
            //Graph testgraph = loader.LoadInputData(new RandomUOW(_n, 1434));

            //Solver SPPRCSolver = new Solver(new SolverOptions(), testgraph);
            //ParallelSolver SPPRCSolverParallel = new ParallelSolver(new SolverOptions(), testgraph);
            //Timekeeper tk = new Timekeeper();
            //int trials = trials_set;
            //string TimeFormat = @"hh\:mm\:ss\:fff";
            //testgraph.BuildSubGraphs();
            //logger.Info("Data Loading finished.", "ComparisonTest()");
            //logger.Info(String.Format("Nodes:{0}, Arcs:{1}, Paths: {2}",
            //testgraph.CountNodes.ToString(), testgraph.CountArcs.ToString(), testgraph.CountPaths.ToString()), "ComparisonTest()");
            //for (int i = 0; i < trials; i++)
            //{
            //    logger.Info("Algorithm 1", "ComparisonTest()");
            //    TimeSpan CalcTime = tk.Measure(() => SPPRCSolver.Calculate());
            //    logger.Info("Duties: " + SPPRCSolver.GeneratedDuties.Count.ToString(), "ComparisonTest()");
            //    logger.Info("Node States: " + testgraph.CountStates.ToString("N0"), "ComparisonTest()");
            //    logger.Info("Calculation Time: " + CalcTime.ToString(TimeFormat), "ComparisonTest()");

            //    testgraph.WarmReset();

            //    logger.Info("Algorithm 2", "ComparisonTest()");
            //    TimeSpan CalcTime2 = tk.Measure(() => SPPRCSolverParallel.Calculate());
            //    logger.Info("Duties: " + SPPRCSolverParallel.GeneratedDuties.Count.ToString(), "ComparisonTest()");
            //    logger.Info("Node States: " + testgraph.CountStates.ToString("N0"), "ComparisonTest()");
            //    logger.Info("Calculation Time: " + CalcTime2.ToString(TimeFormat), "ComparisonTest()");
            //    logger.Info("Maximum Parallel Path Calculation: " + SPPRCSolverParallel.Options.DEBUG_MAX_PARALLEL.ToString(), "ComparisonTest()");
            //    testgraph.WarmReset();
            //}
        }

        private static void ParallelTest()
        {
            //GraphLoader loader = new GraphLoader();
            //Graph testgraph = loader.LoadInputData(new RandomUOW(_n, 1434));

            //ParallelSolver SPPRCSolver = new ParallelSolver(new SolverOptions(), testgraph);
            //Timekeeper tk = new Timekeeper();
            //string TimeFormat = @"hh\:mm\:ss\:fff";
            //testgraph.BuildSubGraphs();
            //logger.Info("Data Loading finished.", "ParallelTest()");
            //logger.Info("Algorithm 2", "ParallelTest()");
            //TimeSpan CalcTime2 = tk.Measure(() => SPPRCSolver.Calculate());
            //logger.Info("Duties: " + SPPRCSolver.GeneratedDuties.Count.ToString(), "ParallelTest()");
            //logger.Info("Node States: " + testgraph.CountStates.ToString("N0"), "ParallelTest()");
            //logger.Info(String.Format("Nodes:{0}, Arcs:{1}, Paths: {2}",
            //    testgraph.CountNodes.ToString(), testgraph.CountArcs.ToString(), testgraph.CountPaths.ToString()), "ParallelTest()");
            //logger.Info("Calculation Time: " + CalcTime2.ToString(TimeFormat), "ParallelTest()");
            //logger.Info("Maximum Parallel Path Calculation: " + SPPRCSolver.Options.DEBUG_MAX_PARALLEL.ToString(), "ParallelTest()");

        }

        private static void SimpleDataTest()
        {
            //GraphLoader loader = new GraphLoader();
            //Graph testgraph = loader.LoadInputData(new SimpleExampleUOW());
            //ParallelSolver solver = new ParallelSolver(new SolverOptions(), testgraph);
            //Timekeeper tk = new Timekeeper();
            //string TimeFormat = @"hh\:mm\:ss\:fff";
            //testgraph.BuildSubGraphs();
            //var timer = tk.Measure(() => solver.Calculate());
            //logger.Info(timer.ToString(TimeFormat), "SimpleDataTest");
        }
    }
}
