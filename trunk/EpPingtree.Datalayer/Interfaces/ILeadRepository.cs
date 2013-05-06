using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;

namespace EpPingtree.Datalayer.Interfaces
{
    public interface ILeadRepository
    {
        void InsertLead(Lead lead);
    }
}
