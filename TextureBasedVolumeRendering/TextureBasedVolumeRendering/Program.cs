using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextureBasedVolumeRendering
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var render=new RenderWindow(800,600,"Render"))
            {
                render.Run(60);
            }
        }
    }
}
