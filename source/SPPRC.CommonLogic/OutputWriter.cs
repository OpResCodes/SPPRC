using SPPRC.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPPRC.Model
{
    public class OutputWriter
    {

        public string WritePath(SubGraph path)
        {
            StringBuilder csb = new StringBuilder();
            csb.AppendFormat("Pathid: {0}, Nodes: ", path.Id.ToString());
            foreach (Node n in path.Nodes)
            {
                if (n == path.Nodes.Last())
                {
                    csb.AppendFormat("[{0}]\n", n.ToString());
                }
                else
                {
                    csb.AppendFormat("[{0}]-", n.ToString());
                }
            }

            csb.AppendLine("Forward Dependencies: " + path.RegisteredForwardDependencies.ToString());
            csb.AppendLine("Backward Dependencies: " + path.Dependencies.ToString());
            csb.Append("Dependency-Chain: ");
            foreach (SubGraph p in path.DependencyList)
            {
                if (p == path.DependencyList.Last())
                {
                    csb.AppendFormat("P{0}\n", p.Id.ToString());
                }
                else
                {
                    csb.AppendFormat("P{0},", p.Id.ToString());
                }
            }
            return csb.ToString();
        }
        
        public void WriteGraph(Graph graph,string filePath)
        {
            FileStream fs;
            StreamWriter sw;
            try
            {
                using (fs =  new FileStream(filePath,FileMode.Create))
                {
                    using (sw = new StreamWriter(fs))
                    {
                        sw.Write(graph.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error in csv export of graph!\n" + ex.Message);
            }
        }

    }
}
