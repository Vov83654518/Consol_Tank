using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Tank_Client_29_03_22_1;
using Print;
using static Tank_Client_29_03_22_1.Koordinat;
using System.Drawing;

namespace Tank_pros
{
    class Tank
    {
        static public string[] map; //карта
        public Point coord; //координаты танка
        public Shot shot; //выстрел танка
        public Koord napravlenie; //положение танка
        public bool distroi; //уничтожен ли танк
        public int unit; //номер танка
        public Tank(string[] new_map, int x, int y, int unit) // танк
        {
            napravlenie = Koord.Up;
            map = new_map;
            coord.X = x;
            coord.Y = y;
            distroi = false;
            this.unit = unit;
            PrintElem.PrintTank(coord.X, coord.Y, napravlenie,map);
            Thread move_p = new Thread(new ThreadStart(Move));
            shot = new Shot();
            move_p.Start();
        }
        public async void Move() // Движение танка
        {
            ConsoleKeyInfo t;
            while (!distroi)
            {
                t = Console.ReadKey(true);
                switch (t.Key)
                {
                    case ConsoleKey.W:
                        if (PrintElem.IsTank(coord.X, coord.Y - 1, Koord.Up,map))
                        {
                            PrintElem.CleneTank(coord.X, coord.Y, napravlenie);
                            coord.Y--;
                            napravlenie = Koord.Up;
                            PrintElem.PrintTank(coord.X, coord.Y, napravlenie,map);
                        }
                        break;
                    case ConsoleKey.S:
                        if (PrintElem.IsTank(coord.X, coord.Y + 1, napravlenie,map))
                        {
                            PrintElem.CleneTank(coord.X, coord.Y, napravlenie);
                            napravlenie = Koord.Doun;
                            coord.Y++;
                            PrintElem.PrintTank(coord.X, coord.Y, napravlenie,map);
                        }
                        break;
                    case ConsoleKey.A:
                        if (PrintElem.IsTank(coord.X - 1, coord.Y, napravlenie, map))
                        {
                            PrintElem.CleneTank(coord.X, coord.Y, napravlenie);
                            napravlenie = Koord.Left;
                            coord.X--;
                            PrintElem.PrintTank(coord.X, coord.Y, napravlenie, map);
                        }
                        break;
                    case ConsoleKey.D:
                        if (PrintElem.IsTank(coord.X + 1, coord.Y, napravlenie, map))
                        {
                            PrintElem.CleneTank(coord.X, coord.Y, napravlenie);
                            napravlenie = Koord.Rait;
                            coord.X++;
                            PrintElem.PrintTank(coord.X, coord.Y, napravlenie,map);
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        shot.Vvod(coord.X, coord.Y, napravlenie, map);
                        break;
                }
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(1);
                    if (Console.KeyAvailable) Console.ReadKey(true);
                }
            }
        }
        public void Distroi() //уничтожение танка
        {
            PrintElem.CleneTank(coord.X, coord.Y, napravlenie);
            distroi = true;
        }
    }
}