﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slowsharp
{
    internal class OptUnaryBase : OptNodeBase
    {
        public string operandId;
        public bool isPrimitiveIncOrDec;
    }
    internal class OptPrefixUnary : OptUnaryBase
    {
    }
    internal class OptPostfixUnary : OptUnaryBase
    {
    }
}
