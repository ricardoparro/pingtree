using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Model;
using EpPingtree.Services;
using Moq;

namespace EpPingtree.Tests
{
   
        public class MoqContainer : IDisposable
    {
        private bool _autoMock;
        private readonly Type _typeTesting;
        private Dictionary<Type, object> _injectedObjects = null;
        private Dictionary<Type, object> _neverMockedTypes = null;

        private Dictionary<Type, object> _objectsUsed = null;

        public MoqContainer() : this(false, null)
        {
            //This is used if don't want to use mocks ie Integration Testing
        }

        public MoqContainer(bool autoMock, Type typeTesting)
        {
            _autoMock = autoMock;
            _typeTesting = typeTesting;

            Initialise();
        }

        public void RebuildDependicies()
        {
            Initialise();
        }

        private void Initialise()
        {
            var builder = new ContainerBuilder();
            ServiceModule.BuildDependencies(builder, false);

            //Cache the container builder
            DependencyResolver = builder.Build();
            /************************************/

            _injectedObjects = new Dictionary<Type, object>();
            _neverMockedTypes = new Dictionary<Type, object>();

            if (_autoMock)
            {
                _neverMockedTypes.Add(_typeTesting, null);
            }

            foreach (IComponentRegistration componentRegistration in DependencyResolver.ComponentRegistry.Registrations)
            {
                //Add handler so can return any injected Moq's
                componentRegistration.Activating += componentRegistration_Activating;
            }

            _objectsUsed = new Dictionary<Type, object>();
        }

        public void AddNeverMockedType<T>()
        {
            Type neverMockT = typeof (T);
            
            if (!_neverMockedTypes.ContainsKey(neverMockT))
                _neverMockedTypes.Add(neverMockT, null);
        }

        public IContainer DependencyResolver { get; private set; }

        public void InjectMock(object o)
        {
            if (o as Mock != null)
                o = (o as Mock).Object;

            Type t = o.GetType();

            if (o is BaseService || t.FullName == "EpPingtree.Service.")
            {
                //Current Type is a Proxy 
                //This is a concrete type, need to add the BaseType to the Injected Object
                Type baseType = t.BaseType;

                if (_injectedObjects.ContainsKey(baseType))
                    _injectedObjects[baseType] = o;
                else
                    _injectedObjects.Add(baseType, o);
            }
            else if (o is EprospectsDataContext)
            {
                if (_injectedObjects.ContainsKey(t))
                    _injectedObjects[t] = o;
                else
                    _injectedObjects.Add(t, o);
            }
            else
            {
                Type[] interfaces = t.GetInterfaces();

                if (interfaces.Length > 0)
                {
                    //Only add the first interface in case have implemented 2
                    Type type = interfaces[0];

                    if (type.FullName.StartsWith("EpPingtree.Datalayer."))
                    {
                        //Inject the mock as the implemented interface
                        if (_injectedObjects.ContainsKey(type))
                            _injectedObjects[type] = o;
                        else
                            _injectedObjects.Add(type, o);
                    }
                }
            }
        }

        public Mock<T> ConstructMoq<T>() where T : class
        {
            Type t = typeof (T);
            return (Mock<T>) ConstructMoq(t);
        }

        private Mock ConstructMoq(Type t)
        {
            ConstructorInfo constructor = t.GetConstructors().First();
            ParameterInfo[] constructP = constructor.GetParameters();

            List<object> constructPList = new List<object>();

            foreach (ParameterInfo parameterInfo in constructP)
            {
                Type pType = parameterInfo.ParameterType;

                //This will call the componentRegistration_Activating to resolve the reference and auto moc the repo if necessary
                object repo = DependencyResolver.Resolve(pType);
                constructPList.Add(repo);
            }

            Type mockType = typeof(Mock<>).MakeGenericType(t);
            Mock serviceMock = (Mock) Activator.CreateInstance(mockType, constructPList.ToArray());
            return serviceMock;
        }

        public void componentRegistration_Activating(object sender, ActivatingEventArgs<object> e)
        {
            Type t = e.Instance.GetType();

            try
            {

                if (_autoMock && _neverMockedTypes.ContainsKey(t))
                {
                    return;
                }

                if (e.Instance is BaseService || t.FullName == "EpPingtree.Service.")
                {
                    if (_injectedObjects.ContainsKey(t))
                        e.Instance = _injectedObjects[t];

                    if (_autoMock && !_neverMockedTypes.ContainsKey(t))
                    {
                        Mock m = ConstructMoq(t);
                        e.Instance = m.Object;
                    }

                    return;
                }

                if (e.Instance is EprospectsDataContext)
                {
                    //Let Autofac make this itself

                    if (_injectedObjects.ContainsKey(t))
                        e.Instance = _injectedObjects[t];
                    else
                        return;
                }

                //It is a DataLayer obj we are after
                Type[] interfaces = t.GetInterfaces();

                foreach (Type implementedInterface in interfaces)
                {
                    object o = GetInterfaceTypeIfMocked(implementedInterface);

                    if (o != null)
                    {
                        e.Instance = o;
                        return;
                    }

                    if (_autoMock && _neverMockedTypes.ContainsKey(implementedInterface))
                    {
                        //Let Autofac generate default
                        return;
                    }

                    if (_autoMock)
                    {
                        TypedService service = new TypedService(implementedInterface);
                        int regFor = DependencyResolver.ComponentRegistry.RegistrationsFor(service).Count();

                        if (regFor == 1)
                        {
                            //Create a anonymous mock of the interface type then
                            Type mockType = typeof(Mock<>).MakeGenericType(implementedInterface);
                            Mock mockRepo = (Mock)Activator.CreateInstance(mockType);
                            e.Instance = mockRepo.Object;
                        }

                    }
                }
            }
            finally
            {
                _objectsUsed[t] = e.Instance;
            }
        }

        public object GetInterfaceTypeIfMocked(Type interfaceT)
        {
            if (_injectedObjects.ContainsKey(interfaceT))
                return _injectedObjects[interfaceT];

            return null;
        }

        public ConcreteT GetObjectUsedForConcreteType<ConcreteT>() where ConcreteT : class
        {
            Type t = typeof(ConcreteT);

            if (_objectsUsed.ContainsKey(t))
                return (ConcreteT) _objectsUsed[t];

            return null;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            //Unregister all the handlers on the static
            foreach (IComponentRegistration componentRegistration in DependencyResolver.ComponentRegistry.Registrations)
            {
                componentRegistration.Activating -= componentRegistration_Activating;
            }
        }

        #endregion
    }
}
