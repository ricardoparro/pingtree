using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EpPingtree.Model;

namespace EpPingtree.Tests
{
    public class BaseTestWithTestClass<T> : BaseTest
    {
        public BaseTestWithTestClass()
        {

        }

        public BaseTestWithTestClass(bool autoMock)
            : base(autoMock, typeof(T))
        {
        }

        /// <summary>
        /// Is a new instance each time you call so can reconstruct with the new mocks
        /// </summary>
        protected T ClassToTest
        {
            get
            {
                T value = Resolve<T>();
                return value;
            }
        }

        /// <summary>
        /// Get a EpPingtreeContext that will not be disposed
        /// </summary>
        /// <returns></returns>
        protected EprospectsDataContext GetDataContextNotToBeDisposed()
        {
            string connection = ConfigurationManager.ConnectionStrings["EprospectsConnectionString"].ToString();
            EprospectsDataContext context = new EprospectsDataContext(connection);

            return context;
        }

        protected EprospectsDataContext GetDataContext()
        {
            return Resolve<EprospectsDataContext>();
        }
    }
}
