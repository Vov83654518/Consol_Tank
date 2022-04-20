using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Koordinat_pros;
using static Koordinat_pros.Koordinat;

namespace Kontra_pros
{
    class Kontra    // танк 
    {
        public Koord napravlenie;   //положение танка
        public bool destr = false;  //уничтожен ли танк
        public int unit;    //номер танка
        public string[] map;    //карта
        public Point coord; //координаты танка
        public Kontra(int x, int y, string k,int unit_new, string[] map) {  //определение изначального положения танка
            coord.X = x;
            coord.Y = y;
            napravlenie = StringToKoord(k);
            unit = unit_new;
            this.map = map;
        }
        public bool Contact(int x, int y)   //проверка поподания в танк
        {
            return ((coord.X - 1 == x || coord.X + 3 == x || coord.X == x || coord.X + 1 == x || coord.X + 2 == x) &&
                (coord.Y - 1 == y || coord.Y + 3 == y || coord.Y == y || coord.Y + 1 == y || coord.Y + 2 == y));
        }
        public bool IsPopal(int x, int y) //уничтожение танка
        {
            if (destr) return false;
            if (Contact(x, y))
                destr = true;
            return Contact(x, y);
        }
    }
}
