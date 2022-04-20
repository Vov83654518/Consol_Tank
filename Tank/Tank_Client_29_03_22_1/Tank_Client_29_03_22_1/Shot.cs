using Print;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using static Tank_Client_29_03_22_1.Koordinat;

namespace Tank_Client_29_03_22_1
{
    class Shot
    {
        public Point coord;
        public Koord napravlenie;
        public bool vzriv;
        public string[] map;
        public Shot() //ввод начальных параметров выстрела
        {
            coord.X = 0;
            coord.Y = 0;
            napravlenie = Koord.Doun;
            vzriv = false;
            this.map = null;
        }
        public void Vvod(int x, int y, Koord k, string[] map) //ввод начальных параметров выстрела
        {
            coord.X = x;
            coord.Y = y;
            napravlenie = k;
            vzriv = false;
            this.map = map;
            Thread shot_p = new Thread(new ThreadStart(Vistrel));
            shot_p.Start();
        }
        public void Vistrel() //выстрел 
        {
            if (!vzriv)
            {
                
                vzriv = true;
                switch (napravlenie)
                {
                    case Koord.Up:
                        coord.X++;
                        coord.Y--;
                        break;
                    case Koord.Doun:
                        coord.X++;
                        coord.Y += 3;
                        break;
                    case Koord.Left:
                        coord.X--;
                        coord.Y++;
                        break;
                    case Koord.Rait:
                        coord.X += 3;
                        coord.Y++;
                        break;
                }
                while (PrintElem.IsBlok(coord.X, coord.Y,map))
                {
                    PrintElem.PrintBlok(coord.X, coord.Y, '█');
                    Thread.Sleep(20);
                    PrintElem.PrintBlok(coord.X, coord.Y, ' ');
                    switch (napravlenie)
                    {
                        case Koord.Up:
                            coord.Y = coord.Y - 1;
                            break;
                        case Koord.Doun:
                            coord.Y = coord.Y + 1;
                            break;
                        case Koord.Left:
                            coord.X = coord.X - 1;
                            break;
                        case Koord.Rait:
                            coord.X = coord.X + 1;
                            break;
                    }
                }
                Thread.Sleep(5);
                vzriv = false;
            }
        }

    }
}
