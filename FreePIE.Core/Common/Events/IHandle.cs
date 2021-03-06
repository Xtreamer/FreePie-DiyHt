﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreePIE.Core.Common.Events
{
    public interface IHandle<T> where T : class
    {
        void Handle(T message);
    }
}
