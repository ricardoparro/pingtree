using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.ReceiveData
{
    public abstract class BaseReceiveDataRenderer : BaseRenderer
    {
        public abstract TBuyerModel ConvertBuyerResponse<TBuyerModel>(string response);
    }
}
