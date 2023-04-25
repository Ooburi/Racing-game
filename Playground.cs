using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TestGame
{
    class Playground
    {
        public Timer explodeAnimTimer;
        int explodeTicks = 10;
        public int Width { get; set; }
        public int Heigth { get; set; }
        public string Border { get; set; }
        private char[][] Frame { get; set; }
        private char[][] LastFrame { get; set; }
        private int DeltaBorder { get; set; }
        private Player Player { get; set; }
        private List<Enemy> Enemies { get; set; }
        public int Score { get; set; }

        public Playground(int w, int h, string b)
        {
            explodeAnimTimer = new Timer();
            explodeAnimTimer.Interval = 50;
            explodeAnimTimer.Elapsed += Explosion;
            Width = w;
            Heigth = h;
            Border = b;
            Score = 0;
            Init();
        }

        private void Explosion(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (explodeTicks < 0)
                {

                    explodeAnimTimer.Stop();
                    explodeAnimTimer.Dispose();
                    
                }

                Random r = new Random();
                if (explodeTicks > 1)
                {
                    for (int i = 0; i < Heigth; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            Console.ForegroundColor = (ConsoleColor)r.Next(1, 15);
                            if (explodeAnimTimer != null && explodeTicks>2)
                            {
                                Console.SetCursorPosition(j, i);
                                Console.Write(Frame[i][j]);
                            }
                            
                        }
                    }
                }
                
                explodeTicks--;
            } catch
            {
                explodeAnimTimer.Dispose();
            }
        }

        public void AddPlayer(Player player)
        {
            Player = player;
            Player.Position.X = Width/2;
            Player.Position.Y = Heigth-player.Heigth;
            PaintPlayer();
        }
        private void Init()
        {
            Enemies = new List<Enemy>();
            Console.CursorVisible = false;
            Frame = new char[Heigth][];
            LastFrame = new char[Heigth][];
            for (int i = 0; i < Heigth; i++)
            {
                Frame[i] = new char[Width];
                LastFrame[i] = new char[Width];
                for (int j = 0; j < Width; j++)
                {
                    Frame[i][j] = ' ';
                    LastFrame[i][j] = ' ';
                }
            }
            
            DeltaBorder = 0;
            MakeBorders();
            PaintScore();
        }
        private void CollisionDetect()
        {
            foreach(Enemy en in Enemies)
            {
                if (((en.Position.Y + en.Heigth) > Player.Position.Y && en.Position.Y<Player.Position.Y+Player.Heigth) &&
                    ((en.Position.X + en.Width > Player.Position.X && en.Position.X < Player.Position.X) ||
                    (en.Position.X < Player.Position.X + Player.Width && en.Position.X + en.Width > Player.Position.X + Player.Width) || (Player.Position.X == en.Position.X)))
                    throw new Exception("Collision");
            }
        }
        public void PaintPlayer()
        {
            int heightPainted = 0;
            int widthPainted = 0;

            for (int i = Player.Position.Y; i < Heigth; i++)
            {
                for (int j = Player.Position.X; j < Width; j++)
                {
                    if(widthPainted<Player.Width && heightPainted < Player.Heigth)
                    {
                        
                        Frame[i][j] = Player.Get()[heightPainted][widthPainted];
                        widthPainted++;
                    }
                }
                widthPainted = 0;
                heightPainted++;
            }
        }
        public void PaintEnemy(Enemy enemy)
        {
            int heightPainted = 0;
            int widthPainted = 0;
            
                for (int j = enemy.Position.X; j < Width-1; j++)
                {
                        if ((enemy.Position.Y - 1) > 0 && (enemy.Position.Y - 1) < Heigth)
                            Frame[enemy.Position.Y-1][j] = ' ';
                }

            for (int i = enemy.Position.Y; i < Heigth; i++)
            {
                for (int j = enemy.Position.X; j < Width; j++)
                {
                    if (widthPainted < enemy.Width && heightPainted < Player.Heigth)
                    {
                        if(i>0 && i<Heigth)
                        Frame[i][j] = enemy.Get()[heightPainted][widthPainted];

                        widthPainted++;
                    }
                }
                widthPainted = 0;
                heightPainted++;
            }
        }
        public void Step()
        {
            
            if (DeltaBorder < Border.Length-1) DeltaBorder++; else DeltaBorder = 0;
            MakeBorders();

            if (Keyboard.IsKeyDown(KeyCode.Left))
            {
                if(Player.Position.X>1)
                Player.Position.X--;

                for (int i = Player.Position.Y; i < Player.Position.Y+Player.Heigth; i++)
                {
                        Frame[i][ Player.Position.X + Player.Width] = ' ';
                }
            }
            if (Keyboard.IsKeyDown(KeyCode.Right))
            {
                if (Player.Position.X < Width-Player.Width-1)
                    Player.Position.X++;

                for (int i = Player.Position.Y; i < Player.Position.Y + Player.Heigth; i++)
                {
                        Frame[i][Player.Position.X - 1] = ' ';
                }
            }

            
            foreach(Enemy e in Enemies)
            {
                
                e.Position.Y++;
                PaintEnemy(e);
            }
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].Position.Y > Heigth + Enemies[i].Heigth)
                {
                    Enemies.Remove(Enemies[i]);
                    Score++;
                    PaintScore();
                }
            }
            
            PaintPlayer();
            Show();
            CollisionDetect();
            Buferize();
        }
        private void PaintScore()
        {
            string score = "SCORE: " + Score.ToString();
            Console.SetCursorPosition(Width+5, 5);
            for (int i = 0; i < score.Length; i++)
            {
                Console.Write(score[i]);
                Console.SetCursorPosition(Width + 6+i, 5);
            }
        }
        public void SpawnEnemy(int scale, char look)
        {
            Enemy en = new Enemy(scale, look);
            int r = new Random().Next(0, 3);
            switch (r)
            {
                case 0:
                    en.Position.X = 1;
                    break;
                case 1:
                    en.Position.X = (Width / 2) - en.Width/2;
                    break;
                case 2:
                    en.Position.X = Width - en.Width - 1;
                    break;
            }
            en.Position.Y = -en.Heigth;
            Enemies.Add(en);
        }
        private void Buferize()
        {
            for (int i = 0; i < Heigth; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    LastFrame[i][j] = Frame[i][j];
                }
            }
        }
        public void Show()
        {
            for (int i = 0; i < Heigth; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Frame[i][j] != LastFrame[i][j]) 
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write(Frame[i][j]);
                    }
                }
            }
        }
        public void MakeBorders()
        {
            int pos = 0;
            int index;
            while (pos < Heigth)
            {
                for (int j = 0; j < Border.Length; j++)
                {
                    if (pos == Heigth) return;
                    index = (j + DeltaBorder + 1) % (Border.Length);
                    Frame[pos][0] = Border[index];
                    Frame[pos][^1] = Border[index];
                    pos++;
                }
            }
        }
    }
}
internal enum KeyCode : int
{
    Left = 0x25,
    Up,
    Right,
    Down
}

internal static class Keyboard
{
    private const int KeyPressed = 0x8000;
    public static bool IsKeyDown(KeyCode key)
    {
        return (GetKeyState((int)key) & KeyPressed) != 0;
    }
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern short GetKeyState(int key);
}
