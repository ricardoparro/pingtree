using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EpPingtree.Services;
using log4net;

namespace EpPingtree.Services
{
    public class BaseService
    {
        public IComponentContext ComponentResolver { private get; set; }
        
        
        protected TObj Resolve<TObj>()
        {
            return ServiceModule.ResolveReference<TObj>(ComponentResolver);
        }
    }
}
