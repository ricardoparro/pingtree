using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces.Buyers;
using EpPingtree.Model;

namespace EpPingtree.Datalayer.Repository.Buyers
{
    public class LeadBoughtRepository:BaseRepository, ILeadBoughtRepository
    {
        public LeadBoughtRepository(EprospectsDataContext context )
        {
            Context = context;
        }
        public LeadBoughtRepository(){}


        public void InsertLeadBought(LeadBought leadBought)
        {
            context.LeadBoughts.InsertOnSubmit(leadBought);
            context.SubmitChanges();
        }
    }
}
