﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Core.EventSystem
{
    internal abstract class Event : IEvent
    {
        public abstract EventArgs GetArgs();
    }
}
