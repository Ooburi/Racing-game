using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame
{
    class Player
    {
        public int Width { get; set; }

        public int Heigth { get; set; }
        public Position Position {get;set;}

        private protected char[][] picture { get; set;}

        public Player(int scale, char look)
        {
            Width = 3*scale;
            Heigth = 4*scale;

            picture = new char[Heigth][];
            for (int i = 0; i < Heigth; i++)
            {
                picture[i] = new char[Width];
            }

            for (int i = 0; i < Heigth; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    picture[i][j]= Paint(i, j) ?  look : ' ';
                }
            }
            Position = new();
        }
        public char[][] Get()
        {
            return picture;
        }
        private bool Paint(int i, int j)
        {
            int wScale = Width / 3;
            int hScale = Heigth / 4;

            if (j / wScale == 1 && i / hScale == 0) return true;
            if(j / wScale == 0 && i / hScale == 1) return true;
            if (j / wScale == 2 && i / hScale == 1) return true;
            if (j / wScale == 1 && i / hScale == 2) return true;
            if (j / wScale == 0 && i / hScale == 3) return true;
            if (j / wScale == 2 && i / hScale == 3) return true;
            return false;
        }
    }
}
