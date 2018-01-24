using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GAMS;
using SPPRC.VRP;


namespace SPPRC.VRP.Tests
{
    [TestClass]
    public class LoadingTests
    {

        string path = @"C:\Users\matth\Downloads\VRP_CG\spBase.gdx";

        [TestMethod]
        public void CanLoadGdx()
        {
            var loader = new GamsDataLoader();
            using (var gdx = loader.OpenGdxfile(path))
            {
                Assert.AreEqual(true, true);
            }
        }

        [TestMethod]
        public void CanCreateGraph()
        {
            var loader = new GamsDataLoader();
            var graph = loader.LoadGraphBase(path);
            Assert.AreNotEqual(0, graph.Arcs.Count);
            Assert.AreNotEqual(0, graph.Nodes.Count);
            Assert.AreNotEqual(null, graph.StartNode);
        }


    }
}
