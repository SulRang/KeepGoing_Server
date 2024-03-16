using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();
        public static List<GameRoom> RoomList = new List<GameRoom>(1000);
        public static GameRoom MainRoom = new GameRoom();
        public static int roomIdx = 0;

        /*
				static void FlushRoom()
				{
					MainRoom.Push(() => MainRoom.Flush());
					JobTimer.Instance.Push(FlushRoom, 250);
				}
		*/
        private static void MakeRoomCapacity()
        {
            for (int i = 1; i < 100; i++)
            {
                RoomList.Add(new GameRoom());
            }
            //Console.WriteLine(RoomList.Capacity);
        }
        /*
		public static void CreateNewRoom(int roomNum = 0)
		{
			GameRoom gameRoom = new GameRoom();
			RoomList.Insert(roomNum, gameRoom);
		}
		*/
        public static void FlushRoom(int roomNum = 0)
        {
            RoomList[roomNum].Push(() => RoomList[roomNum].Flush());
            JobTimer.Instance.Push(FlushRoom, 100, roomNum);
        }

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[1];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");
            MakeRoomCapacity();
            //CreateNewRoom();
            //FlushRoom();
            JobTimer.Instance.Push(FlushRoom);
            for (int i = 1; i <= 50; i++)
                JobTimer.Instance.Push(FlushRoom, 100, i);

            while (true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}
