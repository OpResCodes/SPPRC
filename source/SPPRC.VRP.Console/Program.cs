using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPPRC.VRP;
using SPPRC.Data.Datamodel;
using System.IO;
using System.Diagnostics;

namespace SPPRC.VRP.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string gdxFile = args[0];
            CalculatePaths(gdxFile.Trim('\''));
        }


        static void CalculatePaths(string gdx)
        {
            try
            {
                FileInfo info = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,gdx));
                System.Console.WriteLine($"Reading from {info.FullName}");
                var dir = info.DirectoryName;
                string outputFileName = "cg";
                var sw = new Stopwatch();
                var loader = new GamsDataLoader();
                var graph = loader.LoadGraphBase(info.FullName);
                graph.StartNode = graph.Nodes.Find(n => n.Id == 1);
                var solver = new ParallelSolver(graph, new SolverOptions() { UseMultithreading = true });
                System.Console.WriteLine($"Solving SPPRC with {graph.CountArcs} arcs..");
                sw.Start();
                solver.Solve(graph.StartNode);
                sw.Stop();
                var writer = new GamsDataWriter();
                var states = graph.StartNode.States
                    .Where(s => s.VirtualCost < 0)
                    .OrderBy(s => s.VirtualCost)
                    .Take(10);
                writer.WriteStates(states.ToArray(), Path.Combine(dir, outputFileName));
                System.Console.WriteLine("Calculation complete..");
                System.Console.WriteLine($"Calculation time: {sw.ElapsedMilliseconds.ToString("N2")} ms");  
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("ERROR!\n" + ex.Message);
                throw;
            }
        }

        static void TestCalc()
        {
            string path = @"C:\Users\matth\Downloads\VRP_CG\spBase.gdx";
            var loader = new GamsDataLoader();
            var g = loader.LoadGraphBase(path);
            var solver = new Solver(g);
            RandomizeDualValues(g.Arcs);
            solver.Solve(1);
            foreach (var item in g.Nodes.First(n => n.Id ==1).States)
            {
                System.Console.WriteLine(item.ToString());
            }
        }

        static void RandomizeDualValues(IEnumerable<Arc> arcs)
        {
            foreach (var item in arcs)
            {
                if (r.NextDouble() > 0.8)
                {
                    item.DualCostValue = Math.Round(r.NextDouble() * 2000);
                }
            }
        }
        static Random r = new Random(1434);

    }
}
