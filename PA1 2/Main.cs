using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA1
{
    class main
    {
        static void Main(string[] args)
        {
            Driver d = new Driver();
            d.run();

            Console.WriteLine("Please press any key to quit. :D");
            Console.ReadLine();
        }
    }
}
