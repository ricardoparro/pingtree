using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Integration.Mvc;
using EpPingtree.Datalayer;
using EpPingtree.Model;
using Module = Autofac.Module;

namespace EpPingtree.Services
{
    public class ServiceModule : Module
    {
        #region Build Dependencies

        public static void BuildDependencies(ContainerBuilder builder, bool isWebApp)
        {
            RegisterServices(builder, isWebApp);
            RegisterRepositories(builder, isWebApp);
        }

        private static void RegisterServices(ContainerBuilder builder, bool isWebApp)
        {
            Assembly serviceAssembly = Assembly.GetAssembly(typeof(BaseService));

            //Resolve the Component Context
            var serviceDependency = builder.RegisterAssemblyTypes(serviceAssembly).PropertiesAutowired();

            if (isWebApp)
                serviceDependency.InstancePerHttpRequest();
            else
                serviceDependency.InstancePerLifetimeScope();
        }

        private static void RegisterRepositories(ContainerBuilder builder, bool isWebApp)
        {
            //Register EprospectsDataContext, it will be once instance per Request or nested tree in the service
            ConnectionStringSettings connection = ConfigurationManager.ConnectionStrings["EpConnectionString"];

            if (connection != null)
            {
                string connectionStr = connection.ConnectionString;
                var context = builder.Register(a => new EprospectsDataContext(connectionStr));

                if (isWebApp)
                    context.InstancePerHttpRequest();
                else
                    context.InstancePerLifetimeScope();
            }

            Assembly repository = Assembly.GetAssembly(typeof(BaseRepository));

            //Register the most general first
            //Autofac takes the settings for the last time it was registered
            var dlBuilder = builder.RegisterAssemblyTypes(repository)
                .Where(t => t.FullName.StartsWith("EpPingtree.Datalayer."))
                .AsImplementedInterfaces()
                .PropertiesAutowired(); //EprospectsDataContext for this

            if (isWebApp)
                dlBuilder.InstancePerHttpRequest();
            else
                dlBuilder.InstancePerLifetimeScope();

            //Next register online buyers as implemented interfaces but instance per dependancy as more general than renderers below
            builder.RegisterAssemblyTypes(repository)
                        .Where(t => t.FullName.StartsWith("EpPingtree.Datalayer.ExternalsRepository.Buyers."))
                        .AsImplementedInterfaces()
                        .InstancePerDependency();  //This NEEDS to be one per dependancy so PDB Tier 1&2

            //Register renderers as the actual types
            builder.RegisterAssemblyTypes(repository)
                        .Where(t => t.FullName.StartsWith("EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers"))
                        .InstancePerDependency();  //This NEEDS to be one per dependancy so don't share data from one to the next

        }

        #endregion

        #region Resolve Dependencies

        public static TReturnType ResolveReference<TReturnType>(IComponentContext container)
        {
            return container.Resolve<TReturnType>();
        }

        #endregion
    }
}
