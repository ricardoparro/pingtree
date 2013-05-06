using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model.Apply.Request;
using EpPingtree.Model.Apply.Response;

namespace EpPingtree.Services.Interfaces
{
    public interface IApplyLeadService
    {
        SellLeadResponse ApplyLead(LeadRequest leadRequest);

    }
}
