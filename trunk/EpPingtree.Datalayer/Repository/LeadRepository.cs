using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Model;

namespace EpPingtree.Datalayer.Repository
{
    public class LeadRepository : BaseRepository, ILeadRepository 
    {
        public LeadRepository(){}
        public LeadRepository(EprospectsDataContext context)
        {
            Context = context;
        }
        public void InsertLead(Lead lead)
        {
            context.Leads.InsertOnSubmit(lead);

            context.SubmitChanges();
        }
    }
}
