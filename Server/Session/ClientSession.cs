using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;

namespace Server
{
    class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public GameRoom Room { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float DirH { get; set; }
        public float DirV { get; set; }
        public float RotateY { get; set; }

        public float TrainPosX { get; set; }
        public float TrainPosY { get; set; }
        public float TrainPosZ { get; set; }
        public float TrainRotateY { get; set; }

        public float BulletPosX { get; set; }
        public float BulletPosY { get; set; }
        public float BulletPosZ { get; set; }
        public float BulletRotateY { get; set; }
        public int MapSeed { get; set; }

        public int resourceIdx { get; set; }
        public int Health { get; set; }

        public int MobIdx { get; set; }
        public float MobPosX { get; set; }
        public float MobPosY { get; set; }
        public float MobPosZ { get; set; }

        public static int ridx = 0;

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            //Program.RoomList[0].Push(() => Program.RoomList[0].Enter(this));
            //ridx++;
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            SessionManager.Instance.Remove(this);
            if (Room != null)
            {
                GameRoom room = Room;
                room.Push(() => room.Leave(this));
                Room = null;
            }

            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public void OnConnectedRoom(int roomNum)
        {
            Program.RoomList[roomNum].Push(() => Program.RoomList[roomNum].Enter(this));
            Program.FlushRoom(roomNum);
            Console.WriteLine($"OnConnectRoom : {roomNum}");

        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
