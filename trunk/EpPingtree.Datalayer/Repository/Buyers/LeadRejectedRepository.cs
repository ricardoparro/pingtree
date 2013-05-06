using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces.Buyers;
using EpPingtree.Model;

namespace EpPingtree.Datalayer.Repository.Buyers
{
    public class LeadRejectedRepository:BaseRepository, ILeadRejectedRepository
    {
        public LeadRejectedRepository(){}
        public LeadRejectedRepository(EprospectsDataContext context)
        {
            Context = context;
        }

        public void InsertLeadRejected(LeadRejected leadRejected)
        {
            context.LeadRejecteds.InsertOnSubmit(leadRejected);
            context.SubmitChanges();
        }
    }
}
