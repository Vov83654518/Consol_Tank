using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tank_pros;
using Kontra_pros;
using Print;
using static Tank_Client_29_03_22_1.Koordinat;

namespace Tank_Client_29_03_22_1
{
    class Program
    {
        static int remotePort; // Порт для отправки сообщений
        static IPAddress ipAddress; // IP адрес сервера
        static Socket listeningSocket; // Сокет
        static public string[] map = null; // Карта
        static public Kontra[] kontri = new Kontra[4]; //Танки соперники
        static public Tank t = null; // Свой танк
        static public int plaer = 0; // Количество игроков
        static void Main(string[] args)
        {
            Console.SetWindowSize(40 * 2, 40 + 1);
            ipAddress = IPAddress.Parse("127.0.0.1");   //IP и порт сервера
            remotePort = 1;
            try
            {
                Console.WriteLine("Нажмите лубую клавишу чтобы начать игру");   //Начало работы
                Console.ReadKey(true);
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // Создание сокета
                Task listeningTask = new Task(Listen); // Создание потока
                listeningTask.Start(); // Запуск потока

                Thread.Sleep(1000);

                byte[] data1 = Encoding.Unicode.GetBytes("Map"); //Запрос карты
                EndPoint remotePoint1 = new IPEndPoint(ipAddress, remotePort);
                listeningSocket.SendTo(data1, remotePoint1);

                Thread.Sleep(1000);
                
                while (!t.distroi)
                {
                    Console.SetCursorPosition(0, 0);
                    char message = Console.ReadKey(true).KeyChar;   //Отправка положения танка серверу
                    byte[] data = Encoding.Unicode.GetBytes(t.unit.ToString() + '\n' + t.coord.X.ToString() + '\n' + t.coord.Y.ToString() + '\n' + KoordToString(t.napravlenie) + '\n' + message.ToString());
                    EndPoint remotePoint = new IPEndPoint(ipAddress, remotePort);
                    listeningSocket.SendTo(data, remotePoint);
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.SetCursorPosition(0, 0);    //конец игры
                Console.Clear();
                Console.WriteLine("End");
                Thread.Sleep(100);
            }
            finally
            {
                Close();
            }
        }
        private static void Listen()    //прием сообщения сервера
        {
            try
            {
                IPEndPoint localIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0); // Прослушиваем по адресу
                listeningSocket.Bind(localIP);

                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[10000];

                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (listeningSocket.Available > 0);
                    Console.SetCursorPosition(0, 0);

                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;

                    if (builder.ToString().Split('\n')[builder.ToString().Split('\n').Length - 4] == "Map") //Считывание начальных параметров
                    {
                        if (map == null)
                        {
                            map = builder.ToString().Split('\n');
                            int x = Convert.ToInt32(map[map.Length - 2]), y = Convert.ToInt32(map[map.Length - 1]);
                            var unit = Convert.ToInt32(map[map.Length - 3]);
                            map[map.Length - 1] = null;
                            map[map.Length - 2] = null;
                            map[map.Length - 3] = null;
                            map[map.Length - 4] = null;
                            foreach (var s in map) Console.WriteLine(s);
                            t = new Tank(map, x, y, unit);
                            t.unit = unit;
                            Task shot_tp = new Task(Shot_t);
                            shot_tp.Start();
                        }
                    }
                    else
                    {
                        var soobh = builder.ToString().Split('\n');
                        if (soobh[soobh.Length - 1] == "Distroid") //прием сообщения об уничтожения танка
                        {
                            if (t.unit == Convert.ToInt32(soobh[0]))    //Уничтожен свой танк
                            {
                                t.Distroi();
                                Console.Clear();
                                Console.WriteLine("End");
                                return;
                            }
                            else foreach (var kont in kontri) if (kont.unit == Convert.ToInt32(soobh[0])) kont.destr = true; //уничтожен другой танк
                        }
                        else 
                        {
                            bool mv = false; //прием сообщения об передвижениях других танках
                            for (int i = 0; i < plaer; ++i) if (kontri[i].unit == Convert.ToInt32(soobh[0]))
                                {
                                    kontri[i].Move(Convert.ToInt32(soobh[1]), Convert.ToInt32(soobh[2]), soobh[3]);
                                    mv = true;
                                }
                            if (!mv)
                            {
                                kontri[plaer] = new Kontra(Convert.ToInt32(soobh[1]), Convert.ToInt32(soobh[2]), soobh[3], Convert.ToInt32(soobh[0]),map);
                                plaer++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }
        private static void Shot_t()
        {
            while (true)
            {
                while (t.shot.vzriv) //отправка сообщений о положении выстрела
                {
                    byte[] data = Encoding.Unicode.GetBytes(t.unit.ToString() + '\n' + t.shot.coord.X.ToString() + '\n' + t.shot.coord.Y.ToString() + '\n' + "Shot");
                    EndPoint remotePoint = new IPEndPoint(ipAddress, remotePort);
                    listeningSocket.SendTo(data, remotePoint);
                    Thread.Sleep(5);
                }
            }
        }
        private static void Close()
        {
            if (listeningSocket != null)
            {
                listeningSocket.Shutdown(SocketShutdown.Both);
                listeningSocket.Close();
                listeningSocket = null;
            }
        }
    }
}