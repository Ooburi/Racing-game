using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame
{
    class Enemy : Player
    {
        public Enemy(int scale, char look) : base(scale,look)
        {
            char[] temp;
            for(int i =0; i< Heigth / 2; i++)
            {
                temp = picture[i];
                picture[i] = picture[^(i+1)];
                picture[^(i+1)] = temp;
            }
        }
    }
}
