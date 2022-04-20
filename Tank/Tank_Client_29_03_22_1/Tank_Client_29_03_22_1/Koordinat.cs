using System;
using System.Collections.Generic;
using System.Text;

namespace Tank_Client_29_03_22_1
{
    static public class Koordinat
    {
        static public string KoordToString(Koord napr) //конвертация положения танка в строку
        {
            switch (napr)
            {
                case Koord.Up:
                    return "Up";
                case Koord.Doun:
                    return "Doun";
                case Koord.Left:
                    return "Left";
                case Koord.Rait:
                    return "Rait";
            }
            return "";
        }
        static public Koord StringToKoord(string s) //конвертация строки в положение танка 
        {
            switch (s)
            {
                case "Up":
                    return Koord.Up;
                case "Doun":
                    return Koord.Doun;
                case "Left":
                    return Koord.Left;
                case "Rait":
                    return Koord.Rait;
            }
            return Koord.Doun;
        }
        public enum Koord //положение танка
        {
            Left,
            Rait,
            Up,
            Doun
        }
    }
}
