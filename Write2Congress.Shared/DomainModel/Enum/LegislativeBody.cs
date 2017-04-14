﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Enum
{
    public enum LegislativeBody
    {
        [Description("Senate")]
        Senate,
        [Description("House")]
        House,
        [Description("Joint")]
        Joint,
        [Description("Unknown")]
        Unknown
    }
}
