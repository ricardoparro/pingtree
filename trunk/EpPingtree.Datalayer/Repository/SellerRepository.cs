using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Model;

namespace EpPingtree.Datalayer.Repository
{
    public class SellerRepository : BaseRepository, ISellerRepository 
    {
        public SellerRepository(){}
        public SellerRepository(EprospectsDataContext context)
        {
            Context = context;
        }

        public Seller GetSellerByName(string sellerName)
        {
            Seller se = (from seller in context.Sellers
                         where seller.SellerName == sellerName
                         select seller).FirstOrDefault();

            return se;
            
        }
    }
}
