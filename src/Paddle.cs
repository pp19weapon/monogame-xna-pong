using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pong
{
    class Paddle
    {
        public Point pos;
        public int speed;

        public Paddle(int x, int y)
        {
            pos = new Point(x, y);
            speed = 3;
        }
    }
}
