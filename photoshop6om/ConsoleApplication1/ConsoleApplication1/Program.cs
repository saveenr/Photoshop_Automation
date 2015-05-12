using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static double rem( double a, double b)
        {
            var t = a/b;
            var u = b-(t*b);
            return u;
        }
        static void Main(string[] args)
        {
            double max = 360.0;
            double step = 45.0;
            for (int i = 0; i < 16; i++)
            {
                double angle = -i*step ;
                System.Console.WriteLine("c#:  {0} mod {1} = {2}  ", angle, step, rem(angle,max) );

            }
            System.Console.ReadKey();
        }
    }
}
