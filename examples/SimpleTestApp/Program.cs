using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florence;

namespace SimpleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var plot = new Florence.WinForms.InteractiveHost();
            SimpleTestCommon.SimpleTestCommon.Run(plot);
        }
    }
}
