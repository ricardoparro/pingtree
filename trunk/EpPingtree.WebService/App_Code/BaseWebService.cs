using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Autofac;
using Autofac.Core;
using EpPingtree.Services;

/// <summary>
/// Summary description for BaseWebService
/// </summary>
public class BaseWebService : WebService, IDisposable
{
    public IComponentContext ComponentResolver { get; set; }

    protected TObj Resolve<TObj>()
    {
        return ServiceModule.ResolveReference<TObj>(ComponentResolver);
    }



}
