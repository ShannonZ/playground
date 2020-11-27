using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlicerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new Window(800, 600, "LearnOpenTK - Rotate the Cube"))
            {
                window.Run(60.0);
            }
        }
    }
}
