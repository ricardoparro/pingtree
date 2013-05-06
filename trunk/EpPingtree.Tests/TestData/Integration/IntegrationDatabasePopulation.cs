using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;

namespace EpPingtree.Tests.TestData.Integration
{
    public class IntegrationDatabasePopulation : BaseTest
    {
        private int _sellerId;
        private int _buyer1Id;
        private int _buyer2Id;
        private int _buyer3Id;

        public void Insert2Success1RejectedBuyers()
        {
            //Insert Seller
            using (BeginScope())
            {
                EprospectsDataContext context = Resolve<EprospectsDataContext>();

                Seller seller = new Seller();

                seller.Active = true;
                seller.Country = "UK";
                seller.SellerName = "GetCashNow";

                context.Sellers.InsertOnSubmit(seller);
                context.SubmitChanges();
                _sellerId = seller.SellerId;
            }

            //Insert Buyers
            using (BeginScope())
            {
                EprospectsDataContext context = Resolve<EprospectsDataContext>();
                Buyer buyer1 = new Buyer();
                buyer1.Active = true;
                buyer1.Alias = "FAKE1";
                buyer1.Country = "UK";
                buyer1.EmailAddress = "fake1@gmail.com";
                buyer1.FixedAmount = 5;
                buyer1.IntegrationUrl = "fakesuccess.com";
                buyer1.Mobile = "0787788878783";
                buyer1.Name = "FAKE1";
                buyer1.RefKey = "FAKE";
                buyer1.Username = "fakeuser";
                buyer1.Password = "dsf";

                context.Buyers.InsertOnSubmit(buyer1);

                Buyer buyer2 = new Buyer();
                buyer2.Active = true;
                buyer2.Alias = "FAKE2";
                buyer2.Country = "UK";
                buyer2.EmailAddress = "fake2@gmail.com";
                buyer2.FixedAmount = 10;
                buyer2.IntegrationUrl = "fakesuccess.com";
                buyer2.Mobile = "0787788878783";
                buyer2.Name = "FAKE2";
                buyer2.RefKey = "FAKE";
                buyer2.Username = "fakeuser";
                buyer2.Password = "jfdj";

                context.Buyers.InsertOnSubmit(buyer2);

                Buyer buyer3 = new Buyer();
                buyer3.Active = true;
                buyer3.Alias = "FAKE3";
                buyer3.Country = "UK";
                buyer3.EmailAddress = "fake3@gmail.com";
                buyer3.FixedAmount = 15;
                buyer3.IntegrationUrl = "fakerejected.com";
                buyer3.Mobile = "0787788878783";
                buyer3.Name = "FAKE3";
                buyer3.RefKey = "FAKE";
                buyer3.Username = "fakeuser";
                buyer3.Password = "asd";

                context.Buyers.InsertOnSubmit(buyer3);

                context.SubmitChanges();

                _buyer1Id = buyer1.BuyerId;
                _buyer2Id = buyer2.BuyerId;
                _buyer3Id = buyer3.BuyerId;

            }
            //Insert Billings
            using (BeginScope())
            {
                EprospectsDataContext context = Resolve<EprospectsDataContext>();

                Billing billing1 = new Billing();
                billing1.BillingTypeId = 1;
                billing1.BuyerId = _buyer1Id;
                billing1.SellerId = _sellerId;
                billing1.Value = 1;

                context.Billings.InsertOnSubmit(billing1);


                Billing billing2 = new Billing();
                billing2.BillingTypeId = 2;
                billing2.BuyerId = _buyer2Id;
                billing2.SellerId = _sellerId;
                billing2.Value = 1;

                context.Billings.InsertOnSubmit(billing2);


                Billing billing3 = new Billing();
                billing3.BillingTypeId = 2;
                billing3.BuyerId = _buyer3Id;
                billing3.SellerId = _sellerId;
                billing3.Value = 1;

                context.Billings.InsertOnSubmit(billing3);

                context.SubmitChanges();
            }
            

        }
    }
}
