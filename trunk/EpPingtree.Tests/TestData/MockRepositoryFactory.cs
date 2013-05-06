using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Model;
using Moq;

namespace EpPingtree.Tests.TestData
{
    public class MockRepositoryFactory
    {
        public static Mock<IBuyerConfigRepository> GetBuyersRepository()
        {
            List<Buyer> buyers = ModelPopulateFactory.GetBuyers();
            return GetBuyersRepository(buyers);
        }

        public static Mock<IBuyerConfigRepository> GetBuyersRepository(List<Buyer> buyers)
        {
            Mock<IBuyerConfigRepository> buyersRep = new Mock<IBuyerConfigRepository>();
            buyersRep.Setup(a => a.GetAllBuyersByCountry(It.IsAny<string>(), It.IsAny<bool>())).Returns(buyers);

            return buyersRep;
        }
    }
}
