﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;

namespace EpPingtree.Datalayer.Interfaces.Buyers
{
    public interface ILeadBoughtRepository
    {
        void InsertLeadBought(LeadBought leadBought);
    }
}
