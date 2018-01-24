using SPPRC.Data.Datamodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPPRC.VRP
{
    public class GamsDataWriter
    {

        public void WriteStates(IEnumerable<NodeState> states, string fileName, int iterCount = 1)
        {
            using (var gdx = GetGdxFile(fileName))
            {
                var tourSet = gdx.AddSet("Touren",1);
                var aPar = gdx.AddParameter("aa",2);
                var cPar = gdx.AddParameter("cc", "cost", "Touren");
                foreach (var state in states)
                {
                    string tourLabel = iterCount.ToString();
                    tourSet.AddRecord(tourLabel);
                    var cOfTour = cPar.AddRecord(tourLabel);
                    cOfTour.Value = state.RealCost;

                    var n = state;
                    while (n.MyPredecessor != null)
                    {
                        var a = aPar.AddRecord(n.MyNode.Id.ToString(), tourLabel);
                        a.Value = 1;
                        n = n.MyPredecessor;
                    }

                    iterCount++;
                }
                gdx.Export();
            }
        }

        private GAMS.GAMSDatabase GetGdxFile(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            var dir = info.DirectoryName;
            var workspace = new GAMS.GAMSWorkspace(dir);
            return workspace.AddDatabase(info.Name);
        }
    }
}
