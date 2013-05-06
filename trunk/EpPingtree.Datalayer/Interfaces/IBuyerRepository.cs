using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Response;

namespace EpPingtree.Datalayer.Interfaces
{
    public interface IBuyerRepository<TModel>
    {
        SellLeadResponse SellLead(Lead leadRequest, string integrationUrl);

        int RequestId { get; set; }
    }
}
