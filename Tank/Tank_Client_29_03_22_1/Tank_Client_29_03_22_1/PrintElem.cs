using System;
using System.Collections.Generic;
using System.Text;
using Tank_Client_29_03_22_1;
using static Tank_Client_29_03_22_1.Koordinat;

namespace Print
{
    public static class PrintElem
    {

        static public void PrintBlok(int x, int y, char c) //вывод клетки
        {
            Console.SetCursorPosition(x * 2, y);
            Console.Write(c);
            Console.SetCursorPosition(x * 2 + 1, y);
            Console.Write(c);
        }
        static public bool IsBlok(int x, int y, string[] map) // проверка возможности вывода блока
        {
            return (map[y][x * 2] == ' ' && map[y][x * 2 + 1] == ' ');
        }
        static public bool IsTank(int x, int y, Koord k, string[] map) //проверка возможности вывести танк
        {
            switch (k)
            {
                case Koord.Up:
                    return (IsBlok(x + 1, y, map) && IsBlok(x, y + 1, map) && IsBlok(x + 1, y + 1, map) && IsBlok(x + 2, y + 1, map) && IsBlok(x, y + 2, map) && IsBlok(x + 2, y + 2, map));
                    break;
                case Koord.Doun:
                    return (IsBlok(x + 1, y + 2, map) && IsBlok(x, y + 1, map) && IsBlok(x + 1, y + 1, map) && IsBlok(x + 2, y + 1, map) && IsBlok(x, y, map) && IsBlok(x + 2, y, map));
                    break;
                case Koord.Left:
                    return (IsBlok(x + 1, y, map) && IsBlok(x + 2, y, map) && IsBlok(x, y + 1, map) && IsBlok(x + 1, y + 1, map) && IsBlok(x + 1, y + 2, map) && IsBlok(x + 2, y + 2, map));
                    break;
                case Koord.Rait:
                    return (IsBlok(x, y, map) && IsBlok(x + 1, y, map) && IsBlok(x + 1, y + 1, map) && IsBlok(x + 2, y + 1, map) && IsBlok(x, y + 2, map) && IsBlok(x + 1, y + 2, map));
                    break;
            }
            return false;
        }
        static public void PrintTank(int x, int y, Koord k, string[] map) //вывод танка
        {
            if (IsTank(x, y, k, map))
            {
                switch (k)
                {
                    case Koord.Up:
                        PrintBlok(x + 1, y, '█');
                        PrintBlok(x, y + 1, '█');
                        PrintBlok(x + 1, y + 1, '█');
                        PrintBlok(x + 2, y + 1, '█');
                        PrintBlok(x, y + 2, '█');
                        PrintBlok(x + 2, y + 2, '█');
                        break;
                    case Koord.Doun:
                        PrintBlok(x + 1, y + 2, '█');
                        PrintBlok(x, y + 1, '█');
                        PrintBlok(x + 1, y + 1, '█');
                        PrintBlok(x + 2, y + 1, '█');
                        PrintBlok(x, y, '█');
                        PrintBlok(x + 2, y, '█');
                        break;
                    case Koord.Left:
                        PrintBlok(x, y + 1, '█');
                        PrintBlok(x + 1, y, '█');
                        PrintBlok(x + 1, y + 1, '█');
                        PrintBlok(x + 1, y + 2, '█');
                        PrintBlok(x + 2, y, '█');
                        PrintBlok(x + 2, y + 2, '█');
                        break;
                    case Koord.Rait:
                        PrintBlok(x, y, '█');
                        PrintBlok(x, y + 2, '█');
                        PrintBlok(x + 1, y, '█');
                        PrintBlok(x + 1, y + 1, '█');
                        PrintBlok(x + 1, y + 2, '█');
                        PrintBlok(x + 2, y + 1, '█');
                        break;
                }
                Console.SetCursorPosition(0, 0);
            }
        }
        static public void CleneTank(int x, int y, Koord k) // убрать танк
        {
            switch (k)
            {
                case Koord.Up:
                    PrintBlok(x + 1, y, ' ');
                    PrintBlok(x, y + 1, ' ');
                    PrintBlok(x + 1, y + 1, ' ');
                    PrintBlok(x + 2, y + 1, ' ');
                    PrintBlok(x, y + 2, ' ');
                    PrintBlok(x + 2, y + 2, ' ');
                    break;
                case Koord.Doun:
                    PrintBlok(x + 1, y + 2, ' ');
                    PrintBlok(x, y + 1, ' ');
                    PrintBlok(x + 1, y + 1, ' ');
                    PrintBlok(x + 2, y + 1, ' ');
                    PrintBlok(x, y, ' ');
                    PrintBlok(x + 2, y, ' ');
                    break;
                case Koord.Left:
                    PrintBlok(x, y + 1, ' ');
                    PrintBlok(x + 1, y, ' ');
                    PrintBlok(x + 1, y + 1, ' ');
                    PrintBlok(x + 1, y + 2, ' ');
                    PrintBlok(x + 2, y, ' ');
                    PrintBlok(x + 2, y + 2, ' ');
                    break;
                case Koord.Rait:
                    PrintBlok(x, y, ' ');
                    PrintBlok(x, y + 2, ' ');
                    PrintBlok(x + 1, y, ' ');
                    PrintBlok(x + 1, y + 1, ' ');
                    PrintBlok(x + 1, y + 2, ' ');
                    PrintBlok(x + 2, y + 1, ' ');
                    break;
            }
            Console.SetCursorPosition(0, 0);
        }
    }
}
