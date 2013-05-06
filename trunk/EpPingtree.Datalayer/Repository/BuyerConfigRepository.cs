using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Model;
using EpPingtree.Model.ServiceModel;

namespace EpPingtree.Datalayer.Repository
{
    public class BuyerConfigRepository : BaseRepository, IBuyerConfigRepository
    {
        public BuyerConfigRepository(){}

        public BuyerConfigRepository(EprospectsDataContext context)
        {
            Context = context;
        }

        public List<Buyer> GetAllBuyersByCountry(string country, bool active)
        {
            List<Buyer> buyers = (from buyer in context.Buyers
                           where buyer.Country == country && buyer.Active == active
                           select buyer).OrderBy(a => a.FixedAmount).ToList();

            return buyers;
        }

        public List<BuyerBilling> GetAllBuyersBillingByCountry(string country, bool active, int sellerId)
        {
            List<BuyerBilling> buyerBillings = (from buyer in context.Buyers
                                  join billing in context.Billings on buyer.BuyerId equals billing.BuyerId
                                  where buyer.Country == country && buyer.Active == active && billing.SellerId ==sellerId
                                  select new BuyerBilling
                                             {
                                                 Billing = billing,
                                                 Buyer = buyer
                                             }
                                 ).OrderByDescending(a =>a.Buyer.FixedAmount).ToList();
            return buyerBillings;
        }
    }
}
