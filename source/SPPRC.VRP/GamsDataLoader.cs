using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAMS;
using SPPRC.Data.Datamodel;
using System.IO;


namespace SPPRC.VRP
{
    public class GamsDataLoader
    {

        public GAMSDatabase OpenGdxfile(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException("path not existant");

            GAMSWorkspaceInfo info = new GAMSWorkspaceInfo();
            GAMSWorkspace ws = new GAMSWorkspace(info);
            return ws.AddDatabaseFromGDX(path);
        }


        public Graph LoadGraphBase(string path)
        {
            using (var gdxFile = OpenGdxfile(path))
            {
                var NodeSet = gdxFile.GetSet("i");
                var DepotSet = gdxFile.GetSet("Depot");
                var distPar = gdxFile.GetParameter("d");
                var timePar = gdxFile.GetParameter("t");
                var servTime = gdxFile.GetParameter("service");//service zeit an jedem fahrzeitpfeil;
                var dualValues = gdxFile.GetParameter("pi");

                HashSet<Node> nodeSet = new HashSet<Node>();
                HashSet<Arc> arcSet = new HashSet<Arc>();
                var resourceInfo = new ResourceInfo()
                {
                    GeneralLimit = new double[] { 180 },
                    IntitalValues = new double[] { 0 },
                    ResCount = 1
                };

                foreach (GAMSSetRecord setElement in NodeSet)
                {
                    var node = new Node(FormatNodeId(setElement.Keys[0]), setElement.Keys[0], resourceInfo.GeneralLimit, 0, 0);
                    nodeSet.Add(node);
                }

                double[,] times = new double[nodeSet.Count + 1, nodeSet.Count + 1];
                double[,] dist = new double[nodeSet.Count + 1, nodeSet.Count + 1];
                double[] dual = new double[nodeSet.Count + 1];

                int StartNodeId = -1;
                foreach (GAMSSetRecord rec in DepotSet)
                {
                    StartNodeId = FormatNodeId(rec.Keys[0]);
                }
                double serviceTime = 0;
                foreach (GAMSParameterRecord sTimeRec in servTime)
                {
                    serviceTime = sTimeRec.Value;
                }

                foreach (GAMSParameterRecord distParValue in distPar)
                {
                    int i = FormatNodeId(distParValue.Keys[0]);
                    int j = FormatNodeId(distParValue.Keys[1]);
                    dist[i, j] = distParValue.Value;
                }
                foreach (GAMSParameterRecord dualParVal in dualValues)
                {
                    int i = FormatNodeId(dualParVal.Keys[0]);
                    dual[i] = dualParVal.Value;
                }

                foreach (GAMSParameterRecord timeParValue in timePar)
                {
                    int i = FormatNodeId(timeParValue.Keys[0]);
                    int j = FormatNodeId(timeParValue.Keys[1]);
                    times[i, j] = timeParValue.Value;
                    if (i != StartNodeId)
                    {
                        times[i, j] += serviceTime;
                    }
                }

                int arcCounter = 0;
                for (int i = 1; i < nodeSet.Count; i++)
                {
                    for (int j = i + 1; j < nodeSet.Count + 1; j++)
                    {
                        if (dist[i, j] <= 10000)
                        {
                            Arc a = new Arc(arcCounter,
                                nodeSet.First(n => n.Id == i),
                                nodeSet.First(n => n.Id == j),
                                dist[i, j],
                                dual[j], new double[] { times[i, j] });
                            arcCounter++;
                            arcSet.Add(a);
                        }
                        if (dist[j, i] <= 10000)
                        {
                            Arc a = new Arc(arcCounter,
                                nodeSet.First(n => n.Id == j),
                                nodeSet.First(n => n.Id == i),
                                dist[j, i],
                                dual[i], new double[] { times[j, i] });
                            arcCounter++;
                            arcSet.Add(a);
                        }
                    }
                }

                Graph g = new Graph()
                {
                    Arcs = new List<Arc>(arcSet),
                    Nodes = new List<Node>(nodeSet),
                    StartNode = nodeSet.First(n => n.Id == StartNodeId),
                    NeedsCalculation = true,
                    ResourceInfo = resourceInfo
                };
                return g;
            }
        }

        int FormatNodeId(string key)
        {
            return Convert.ToInt32(key);
        }
    }
}
