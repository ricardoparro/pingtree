using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Datalayer.Interfaces
{
    public interface IConfigRepository
    {
        bool IsLive { get; }
    }
}
