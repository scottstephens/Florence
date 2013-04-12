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
            var plot = new Florence.WinForms.ImperativeHost();
            plot.Start();

            var x = new double[] { 1.0, 2.0, 3.0 };
			var y = new double[] { 1.0, 2.0, 3.0 };
			var z = new double[] { 0.0, 2.0, 4.0 };
            plot.points(x, y);
			Console.ReadLine();

			plot.lines(x, z, title: "Test Plot 2", x_label:"X2",y_label:"Y2");
						
            Console.ReadLine();

			
			
            plot.Stop();
        }
    }
}
