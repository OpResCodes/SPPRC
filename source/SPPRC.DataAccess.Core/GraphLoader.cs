using SPPRC.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPPRC.Model
{
    public class GraphLoader
    {

        public Graph LoadInputData(IUnitOfWork unitOfWork)
        {
            Graph g = new Graph();
            try
            {
                var NodeRepository = unitOfWork.NodeRepository;
                var ArcRepository = unitOfWork.ArcRepository;
                g.Nodes = NodeRepository.GetAll();
                g.Arcs = ArcRepository.GetAll();
                g.ResourceInfo = unitOfWork.ResourceInformation;
                g.StartNode = g.Nodes.First();
                g.EndNode = g.Nodes.Last();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to load data!\n" + ex.Message);
                g.ResetGraph();
            }
            return g;
        }
    }
}
