using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;
using EpPingtree.Services.NewLead;
using EpPingtree.Tests.TestData.Integration;
using NUnit.Framework;

namespace EpPingtree.Tests.ManualTests
{
    [TestFixture]
    public class InsertTestData :BaseTestWithTestClass<ApplyLeadService>
    {
        private int _sellerId;

        [Test]
        public void InsertDataTest()
        {
            IntegrationDatabasePopulation helper = new IntegrationDatabasePopulation();
            helper.Insert2Success1RejectedBuyers();

        }

        [Test]
        public void DeleteDataTest(){}

    }
}
