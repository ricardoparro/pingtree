using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Integration.Mvc;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Datalayer.Interfaces.AffiliateSystem.Reports;
using EpPingtree.Datalayer.Repository;
using EpPingtree.Model;

namespace EpPingtree.Datalayer
{
    public class BaseRepository : Module, IBaseRepository
    {
        public EprospectsDataContext context;

        public EprospectsDataContext Context { get { return context; } set { context = value; } }

       

        //override Load method
        //protected override void Load(ContainerBuilder builder)
        //{
        //    //say that for any IUsersRepository we need UsersRepository class to be invoked
        //   builder.Register(c => new ReportRepository()).As<IReportRepository>().InstancePerHttpRequest();
        //}
    }
}
