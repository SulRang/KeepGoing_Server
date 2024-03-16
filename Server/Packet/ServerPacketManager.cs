using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }

    PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {
        _makeFunc.Add((ushort)PacketID.C_LeaveGame, MakePacket<C_LeaveGame>);
        _handler.Add((ushort)PacketID.C_LeaveGame, PacketHandler.C_LeaveGameHandler);
        _makeFunc.Add((ushort)PacketID.C_Move, MakePacket<C_Move>);
        _handler.Add((ushort)PacketID.C_Move, PacketHandler.C_MoveHandler);
        _makeFunc.Add((ushort)PacketID.C_TrainMove, MakePacket<C_TrainMove>);
        _handler.Add((ushort)PacketID.C_TrainMove, PacketHandler.C_TrainMoveHandler);
        _makeFunc.Add((ushort)PacketID.C_Shot, MakePacket<C_Shot>);
        _handler.Add((ushort)PacketID.C_Shot, PacketHandler.C_ShotHandler);
        _makeFunc.Add((ushort)PacketID.C_EnterRoom, MakePacket<C_EnterRoom>);
        _handler.Add((ushort)PacketID.C_EnterRoom, PacketHandler.C_EnterRoomHandler);
        _makeFunc.Add((ushort)PacketID.C_Health, MakePacket<C_Health>);
        _handler.Add((ushort)PacketID.C_Health, PacketHandler.C_HealthHandler);
        _makeFunc.Add((ushort)PacketID.C_EnemyMove, MakePacket<C_EnemyMove>);
        _handler.Add((ushort)PacketID.C_EnemyMove, PacketHandler.C_EnemyMoveHandler);
        _makeFunc.Add((ushort)PacketID.C_PickUp, MakePacket<C_PickUp>);
        _handler.Add((ushort)PacketID.C_PickUp, PacketHandler.C_PickUpHandler);

    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;
        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {
            IPacket packet = func.Invoke(session, buffer);
            if (onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            else
                HandlePacket(session, packet);
        }
    }

    T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.Read(buffer);
        return pkt;
    }

    public void HandlePacket(PacketSession session, IPacket packet)
    {
        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(packet.Protocol, out action))
            action.Invoke(session, packet);
    }
}