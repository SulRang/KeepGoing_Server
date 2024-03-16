using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{

    public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Leave(clientSession));
    }

    public static void C_MoveHandler(PacketSession session, IPacket packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        //Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.Move(clientSession, movePacket));
    }

    public static void C_TrainMoveHandler(PacketSession session, IPacket packet)
    {
        C_TrainMove movePacket = packet as C_TrainMove;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        //Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.TrainMove(clientSession, movePacket));

    }
    public static void C_ShotHandler(PacketSession session, IPacket packet)
    {
        C_Shot movePacket = packet as C_Shot;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        //Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.Shot(clientSession, movePacket));

    }
    public static void C_MapSeedHandler(PacketSession session, IPacket packet)
    {
        C_MapSeed movePacket = packet as C_MapSeed;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        //Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.MapSeed(clientSession, movePacket));

    }

    public static void C_EnterRoomHandler(PacketSession session, IPacket packet)
    {
        C_EnterRoom roomPacket = packet as C_EnterRoom;
        ClientSession clientSession = session as ClientSession;
        Program.RoomList[roomPacket.roomNum].Push(() => Program.RoomList[roomPacket.roomNum].Enter(clientSession));
        if (clientSession.Room == null)
            return;

        Program.roomIdx = roomPacket.roomNum;
        GameRoom room = clientSession.Room;
        //room.Push(() => room.Leave(clientSession));
        //clientSession.OnConnectedRoom(roomPacket.roomNum);

        //room = clientSession.Room;
        room.Push(() => room.EnterRoom(clientSession, roomPacket));
    }

    public static void C_ResourceHandler(PacketSession session, IPacket packet)
    {
        C_Resource resourcePacket = packet as C_Resource;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Resource(clientSession, resourcePacket));
    }

    public static void C_HealthHandler(PacketSession session, IPacket packet)
    {
        C_Health healthPacket = packet as C_Health;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.ChangeHealth(clientSession, healthPacket));
    }

    public static void C_EnemyMoveHandler(PacketSession session, IPacket packet)
    {
        C_EnemyMove mobMovePacket = packet as C_EnemyMove;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.MobMove(clientSession, mobMovePacket));
    }

    public static void C_PickUpHandler(PacketSession session, IPacket packet)
    {
        C_PickUp pickPacket = packet as C_PickUp;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        //Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.PickUp(clientSession, pickPacket));
    }

}
