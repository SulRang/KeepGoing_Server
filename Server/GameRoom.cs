using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class GameRoom : IJobQueue
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession s in _sessions)
                s.Send(_pendingList);
            //Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }

        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }

        public void Enter(ClientSession session)
        {
            _sessions.Add(session);
            session.Room = this;

            S_PlayerList players = new S_PlayerList();
            foreach (ClientSession s in _sessions)
            {
                players.players.Add(new S_PlayerList.Player()
                {
                    isSelf = (s == session),
                    playerId = s.SessionId,
                    posX = s.PosX,
                    posY = s.PosY,
                    posZ = s.PosZ,
                });
            }
            session.Send(players.Write());

            S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
            enter.playerId = session.SessionId;
            enter.posX = 0;
            enter.posY = 0;
            enter.posZ = 0;
            Broadcast(enter.Write());
        }

        public void Leave(ClientSession session)
        {
            _sessions.Remove(session);

            S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
            leave.playerId = session.SessionId;
            Broadcast(leave.Write());
        }

        public void Move(ClientSession session, C_Move packet)
        {
            session.PosX = packet.posX;
            session.PosY = packet.posY;
            session.PosZ = packet.posZ;
            session.DirH = packet.dirH;
            session.DirV = packet.dirV;
            session.RotateY = packet.rotateY;

            S_BroadcastMove move = new S_BroadcastMove();
            move.playerId = session.SessionId;
            move.posX = session.PosX;
            move.posY = session.PosY;
            move.posZ = session.PosZ;
            move.dirH = session.DirH;
            move.dirV = session.DirV;
            move.rotateY = session.RotateY;
            Broadcast(move.Write());
        }

        public void TrainMove(ClientSession session, C_TrainMove packet)
        {
            session.TrainPosX = packet.posX;
            session.TrainPosY = packet.posY;
            session.TrainPosZ = packet.posZ;
            session.TrainRotateY = packet.rotateY;

            S_BroadcastTrainMove move = new S_BroadcastTrainMove();
            move.posX = session.TrainPosX;
            move.posY = session.TrainPosY;
            move.posZ = session.TrainPosZ;
            move.rotateY = session.TrainRotateY;
            Broadcast(move.Write());
        }
        public void Shot(ClientSession session, C_Shot packet)
        {
            session.BulletPosX = packet.posX;
            session.BulletPosY = packet.posY;
            session.BulletPosZ = packet.posZ;
            session.BulletRotateY = packet.rotateY;

            S_BroadcastShot move = new S_BroadcastShot();
            move.posX = session.BulletPosX;
            move.posY = session.BulletPosY;
            move.posZ = session.BulletPosZ;
            move.rotateY = session.BulletRotateY;
            Broadcast(move.Write());
        }
        public void MapSeed(ClientSession session, C_MapSeed packet)
        {
            session.MapSeed = packet.mapSeed;

            S_BroadcastMapSeed map = new S_BroadcastMapSeed();
            map.mapSeed = session.MapSeed;
            Broadcast(map.Write());
        }
        public void EnterRoom(ClientSession session, C_EnterRoom packet)
        {
            Console.WriteLine($"Room : {packet.roomNum}");

            /*S_BroadcastEnterRoom enterRoom = new S_BroadcastEnterRoom();
			enterRoom.playerId = session.SessionId;
			Broadcast(enterRoom.Write());
			*/
        }
        public void Resource(ClientSession session, C_Resource packet)
        {
            session.resourceIdx = packet.resourceIdx;

            S_BroadcastResource resource = new S_BroadcastResource();
            resource.resourceIdx = session.resourceIdx;
            Broadcast(resource.Write());
        }
        public void ChangeHealth(ClientSession session, C_Health packet)
        {
            session.Health = packet.health;

            S_BroadcastHealth bHealth = new S_BroadcastHealth();
            bHealth.playerId = session.SessionId;


            bHealth.health = session.Health;
            Broadcast(bHealth.Write());
        }

        public void PickUp(ClientSession session, C_PickUp packet)
        {
            session.PosX = packet.posX;
            session.PosY = packet.posY;
            session.PosZ = packet.posZ;
            session.RotateY = packet.rotateY;


            S_BroadcastPickUp pick = new S_BroadcastPickUp();
            pick.playerId = session.SessionId;
            pick.posX = session.PosX;
            pick.posY = session.PosY;
            pick.posZ = session.PosZ;
            pick.rotateY = session.RotateY;
            Broadcast(pick.Write());
        }

        public void MobMove(ClientSession session, C_EnemyMove packet)
        {
            session.MobIdx = packet.enemyIdx;
            session.MobPosX = packet.posX;
            session.MobPosY = packet.posY;
            session.MobPosZ = packet.posZ;

            S_BroadcastEnemyMove mobMove = new S_BroadcastEnemyMove();
            mobMove.enemyIdx = session.MobIdx;
            mobMove.posX = session.MobPosX;
            mobMove.posY = session.MobPosY;
            mobMove.posZ = session.MobPosZ;
            Broadcast(mobMove.Write());
        }

    }
}
