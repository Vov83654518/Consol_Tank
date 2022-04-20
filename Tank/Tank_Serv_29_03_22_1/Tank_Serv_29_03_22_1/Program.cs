using Kontra_pros;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Koordinat_pros.Koordinat;

namespace Tank_Serv_29_03_22_1
{
    class Program
    {
        static int localPort; // порт приема сообщений
        static Socket listeningSocket; // Сокет
        static List<IPEndPoint> clients = new List<IPEndPoint>(); // Список "подключенных" клиентов
        static public string[] map; //Карта
        static public int max_plaer = 4;    //максимальное количество игроков
        static public Kontra[] kontri = new Kontra[max_plaer];  //список танков
        static public Point[] nachk = new Point[] { new Point(2, 2), new Point(30, 2), new Point(2, 30), new Point(30, 30) };   //точки начала игроков
        static public int plaer = 0;    //количество играков
        static void Main(string[] args)
        {
            localPort = 1;
            map = File.ReadAllText("map1.txt").Split('\n'); // Считывание карты
            try
            {
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // Создание сокета
                Task listeningTask = new Task(Listen); // Создание потока для получения сообщений
                listeningTask.Start(); // Запуск потока
                listeningTask.Wait(); // Не идем дальше пока поток не будет остановлен
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close(); // Закрываем сокет
            }
        }
        private static void Listen() // поток для приема подключений
        {
            try
            {
                IPEndPoint localIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), localPort);
                listeningSocket.Bind(localIP);
                Console.WriteLine("Redi");  //начало приема сообщений
                while (true)
                {
                    StringBuilder builder = new StringBuilder(); 
                    int bytes = 0; 
                    byte[] data = new byte[256]; 
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0); 

                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (listeningSocket.Available > 0);
                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint; 
                    Console.WriteLine("{0}:{1} - {2}", remoteFullIp.Address.ToString(), remoteFullIp.Port, builder.ToString()); //вывод полученного сообщения

                    bool addClient = true; 
                    for (int i = 0; i < clients.Count; i++) 
                        if (clients[i].ToString() == remoteFullIp.ToString()) 
                            addClient = false; 
                    if (addClient == true) 
                        clients.Add(remoteFullIp);
                    BroadcastMessage(builder.ToString(), remoteFullIp.ToString()); 
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
        private static void BroadcastMessage(string message, string ip) //обработка и отправка сообщений
        {
            if (message == "Map")
            {
                if (max_plaer > plaer)  //отправка карты и координат других игроков
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        int nambPlay = (plaer) % (nachk.Length);
                        if (clients[i].ToString() == ip)
                        {
                            Kontra t_k = new Kontra(nachk[nambPlay].X, nachk[nambPlay].Y, "Doun", plaer, map);
                            kontri[plaer] = t_k;
                            listeningSocket.SendTo(Encoding.Unicode.GetBytes(File.ReadAllText("map1.txt") + '\n' + "Map" + '\n' + 
                                plaer.ToString() + '\n' + nachk[nambPlay].X.ToString() + '\n' + nachk[nambPlay].Y.ToString()), clients[i]);
                            Thread.Sleep(100);
                            for(int o=0;o < plaer; o++) listeningSocket.SendTo(Encoding.Unicode.GetBytes(kontri[o].unit.ToString() + '\n' + kontri[o].coord.X.ToString()
                                + '\n' + kontri[o].coord.Y.ToString() + '\n' + KoordToString(kontri[o].napravlenie) + '\n' + ConsoleKey.S.ToString()), clients[i]);
                            plaer++;
                        }
                        else
                            listeningSocket.SendTo(Encoding.Unicode.GetBytes(plaer.ToString() + '\n' + nachk[nambPlay].X.ToString() + '\n' + 
                                nachk[nambPlay].Y.ToString() + '\n' + "Doun" + '\n' + ConsoleKey.S.ToString()), clients[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < clients.Count; i++) 
                    if (clients[i].ToString() != ip)
                    {
                        var soobh = message.Split('\n');
                        if (soobh[soobh.Length - 1] == "Shot" ) //проверка уничтожения танка 
                        {
                            if (kontri[i].IsPopal(Convert.ToInt32(soobh[1]), Convert.ToInt32(soobh[2])) && Convert.ToInt32(soobh[0]) != i)
                            {
                                listeningSocket.SendTo(Encoding.Unicode.GetBytes(kontri[i].unit.ToString() + '\n' + "Distroid"), clients[i]);
                                clients.Remove(clients[i]);
                            }
                        }
                        else
                            listeningSocket.SendTo(Encoding.Unicode.GetBytes(message), clients[i]); //отпрака положения танка
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
            Console.WriteLine("Сервер остановлен!");
        }
    }
}
