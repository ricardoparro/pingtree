using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model.Enums;

namespace EpPingtree.Model.Apply.Response
{
    public class SellLeadResponse
    {
        public BuyerEnum.ESellLeadResponse Result { get; set; }

        public FailureReasons ErrorMessage { get; set; }

        public string AcceptedLender { get; set; }

        public string RedirectUrl { get; set; }

        public string Reference { get; set; }
    }
}
