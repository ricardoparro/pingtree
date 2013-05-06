using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EpPingtree.Services;
using Moq;

namespace EpPingtree.Tests
{

    public class BaseTest : IDisposable
    {
        protected static MoqContainer MockContainer { get; private set; }
        protected ILifetimeScope LifetimeScope;

        public BaseTest()
        {
            MockContainer = new MoqContainer();
            LifetimeScope = MockContainer.DependencyResolver.BeginLifetimeScope();
        }

        public BaseTest(bool autoMock, Type typeTesting)
        {
            MockContainer = new MoqContainer(autoMock, typeTesting);
            LifetimeScope = MockContainer.DependencyResolver.BeginLifetimeScope();
        }

        protected ILifetimeScope BeginScope()
        {
            LifetimeScope = MockContainer.DependencyResolver.BeginLifetimeScope();
            return LifetimeScope;
        }

        protected T Resolve<T>()
        {
            return ServiceModule.ResolveReference<T>(LifetimeScope);
        }

        protected void InjectMock(object o)
        {
            MockContainer.InjectMock(o);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            MockContainer.Dispose();
        }

        #endregion
    }
}

