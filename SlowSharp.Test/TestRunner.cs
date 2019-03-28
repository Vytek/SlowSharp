﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Slowsharp.Test
{
    public class TestRunner
    {
        public static object Run(string code)
        {
            return CScript.Run(@"
using System;

public class Foo {

public static object Main(string[] args) {
"
+ code +
@"
}
}");
        }

        public static object Run(string classBody, string body)
        {
            return CScript.Run(@"
using System;

public class Foo {"
+ classBody +
@"
public static object Main(string[] args) {
"
+ body +
@"
}
}");
        }
    }
}
