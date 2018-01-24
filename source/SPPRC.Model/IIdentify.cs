using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPPRC.Model
{
    public interface IIdentify : IEntity
    {
        string Label { get; set; }
    }
}
