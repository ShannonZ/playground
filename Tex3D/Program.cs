using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tex3D
{
    class Program
    {
        static void Main(string[] args)
        {
            using (RenderWindow win = new RenderWindow(400, 400, "RayCasting"))
            {
                win.Run(60);
            }
        }
    }
}
