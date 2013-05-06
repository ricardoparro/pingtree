using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;

namespace EpPingtree.Tests.UnitTests
{
    public class UnitTestBase<T> : BaseTestWithTestClass<T>
    {
        public UnitTestBase()
            : base(true)
        {

        }
    }
}
