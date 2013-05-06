using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.SendData
{
    public abstract class BaseSendDataRenderer : BaseRenderer
    {

        public abstract string PrepareRequest(object[] requestNode);
        public abstract string SendRequest(string requestString);
    }
}
