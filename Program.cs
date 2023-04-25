using System;
using System.Threading;
using System.Timers;

namespace TestGame
{
    class Program
    {
        static Playground playground;
        static double speed = 900;
        static string sc = "";
        static System.Timers.Timer timer = new System.Timers.Timer();
        static System.Timers.Timer timer2 = new System.Timers.Timer();
        static System.Timers.Timer spawnTimer = new System.Timers.Timer();

        static int scale = 2;
        static void Main(string[] args)
        {
            Console.WindowHeight = 45;
            Console.WindowWidth = 100;
            

            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\n\n\n\n\t\tWElcome to my game\n");
            Console.WriteLine("Set scale to 1, 2, or 3");
            Console.ForegroundColor = ConsoleColor.White;
             sc = Console.ReadLine();
            try
            {
                scale = Convert.ToInt32(sc);
                if (scale <= 1) scale = 1;
                if (scale == 2) scale = 2;
                if (scale > 2) scale = 3;
            }
            catch
            {
                scale = 2;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\t\tScale set to "+scale);
            Console.WriteLine("\t\tinput any symbols which will be used to draw units");
            Console.ForegroundColor = ConsoleColor.White;
            sc = Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            if (sc.Length<3 || sc.Length > 40)
            {
                Console.WriteLine("\n\t\twrong symbols, default applied");
                sc = "♦▌▀▄";
                Console.WriteLine("\nYour enemies: "+sc);
            }
            Console.WriteLine("\n\t\tNow press ENTER!!!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            Start();
            while(true)
            Console.ReadLine();

        }
        private static void Start()
        {
            speed = 900;
            Console.Clear();
            timer.Elapsed += Timer_Elapsed;
            timer2.Elapsed += Timer2_Elapsed;
            spawnTimer.Elapsed += SpawnTimer_Elapsed;
            timer2.Interval = 5000;
            spawnTimer.Interval = 2500;

            playground = new Playground(45, 35, "▐ ▌ ");

            playground.AddPlayer(new Player(scale, '╫'));

            timer.Start();
            timer2.Start();
            spawnTimer.Start();
        }

        private static void SpawnTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            playground.SpawnEnemy(scale, sc[new Random().Next(0,sc.Length)]);
            if(spawnTimer.Interval>1300)
            spawnTimer.Interval-= 5;
        }

        private static void Timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            if(speed<994)
            speed += 5;
            timer.Interval = 1000-speed;
        }

        private static  void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                playground.Step();
            }
            catch
            {
                timer.Stop();
                timer2.Stop();
                spawnTimer.Stop();
                playground.explodeAnimTimer.Start();
                playground.explodeAnimTimer.Disposed += disposed;
            }
        }

        private static void disposed(object sender, EventArgs e)
        {
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Beep();
            
            Console.Clear();
            Console.WriteLine("\n\n\n\n\n\t\tGAME OVER\n\t\tYour SCORE:" + playground.Score);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}


