using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;
using EpPingtree.Model.ServiceModel;

namespace EpPingtree.Datalayer.Interfaces
{
    public interface IBuyerConfigRepository
    {
        List<Buyer> GetAllBuyersByCountry(string country, bool active);

        List<BuyerBilling> GetAllBuyersBillingByCountry(string country, bool active, int sellerId);
    }
}
