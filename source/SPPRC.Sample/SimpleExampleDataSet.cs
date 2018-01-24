using SPPRC.Model;
using System.Collections.Generic;

namespace SPPRC.Sample
{
    public class SimpleExampleDataSet : IUnitOfWork
    {

        private bool commited;
        private SampleRepository<Arc> _arcs;
        private SampleRepository<Node> _nodes;
        private SampleRepository<Duty> _duties;
        private ResourceInfo _rsinfo;

        
        public SimpleExampleDataSet()
        {
            commited = false;
            SimpleExampleData sed = new SimpleExampleData();
            _rsinfo = sed.rsinfo;
            _nodes = new SampleRepository<Node>(sed.nodes);
            _arcs = new SampleRepository<Arc>(sed.arcs);
            _duties = new SampleRepository<Duty>(new List<Duty>());
        }

        public bool IsCommited => commited;


        public IRepository<Arc> ArcRepository => _arcs;

        public IRepository<Node> NodeRepository => _nodes;

        public IRepository<Duty> DutyRepository => _duties;

        public ResourceInfo ResourceInformation => _rsinfo;

        public void Commit()
        {
            commited = true;
        } 

    }
}
