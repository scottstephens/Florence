using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Florence;

namespace SimpleTestAppGtkSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var plot = new Florence.GtkSharp.InteractiveHost();
            SimpleTestCommon.SimpleTestCommon.Run(plot);
        }
    }
}
