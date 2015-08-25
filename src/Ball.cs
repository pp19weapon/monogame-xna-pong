using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pong
{
    class Ball
    {
        public Point pos;
        public int hSpeed, vSpeed;

        public Ball(int x, int y)
        {
            pos = new Point(x, y);

            Random rand = new Random();
            hSpeed = rand.Next(3, 7);
            if (rand.Next(0, 2) == 0)
                hSpeed *= -1;

            rand = new Random();
            vSpeed = rand.Next(3, 7);
            if (rand.Next(0, 2) == 0)
                vSpeed *= -1;
        }
    }
}
