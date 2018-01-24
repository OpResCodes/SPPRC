using SPPRC.Model;

namespace SPPRC.Model
{
    public interface IUnitOfWork {
        IRepository<Arc> ArcRepository { get; }
        IRepository<Node> NodeRepository { get; }
        IRepository<Duty> DutyRepository { get; }
        ResourceInfo ResourceInformation { get; }
        void Commit();
    }

}
