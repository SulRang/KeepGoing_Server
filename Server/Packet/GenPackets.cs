using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

public enum PacketID
{
    S_BroadcastEnterGame = 1,
    C_LeaveGame = 2,
    S_BroadcastLeaveGame = 3,
    S_PlayerList = 4,
    C_Move = 5,
    S_BroadcastMove = 6,
    C_TrainMove = 7,
    S_BroadcastTrainMove = 8,
    C_Shot = 9,
    S_BroadcastShot = 10,
    C_MapSeed = 11,
    S_BroadcastMapSeed = 12,
    C_EnterRoom = 13,
    S_BroadcastEnterRoom = 14,
    C_Resource = 15,
    S_BroadcastResource = 16,
    C_Health = 17,
    S_BroadcastHealth = 18,
    C_EnemyMove = 19,
    S_BroadcastEnemyMove = 20,
    C_PlayerStatus = 21,
    S_BroadcastPlayerStatus = 22,
    C_PickUp = 23,
    S_BroadcastPickUp = 24,
}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}



public class S_BroadcastEnterGame : IPacket
{
    public int playerId;
    public float posX;
    public float posY;
    public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnterGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_LeaveGame : IPacket
{

    public ushort Protocol { get { return (ushort)PacketID.C_LeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_BroadcastLeaveGame : IPacket
{
    public int playerId;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastLeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastLeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_PlayerList : IPacket
{
    public class Player
    {
        public bool isSelf;
        public int playerId;
        public float posX;
        public float posY;
        public float posZ;

        public void Read(ArraySegment<byte> segment, ref ushort count)
        {
            isSelf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
            count += sizeof(bool);
            playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);
            posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);
            posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);
        }

        public bool Write(ArraySegment<byte> segment, ref ushort count)
        {
            bool success = true;
            Array.Copy(BitConverter.GetBytes(isSelf), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);
            Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);
            Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);
            Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);
            return success;
        }
    }
    public List<Player> players = new List<Player>();

    public ushort Protocol { get { return (ushort)PacketID.S_PlayerList; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.players.Clear();
        ushort playerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += sizeof(ushort);
        for (int i = 0; i < playerLen; i++)
        {
            Player player = new Player();
            player.Read(segment, ref count);
            players.Add(player);
        }
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_PlayerList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        foreach (Player player in this.players)
            player.Write(segment, ref count);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}



/// <summary>
/// Player Client Packet
/// </summary>
public class C_Move : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float dirH;
    public float dirV;
    public float rotateY;

    public ushort Protocol { get { return (ushort)PacketID.C_Move; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        dirH = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        dirV = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Move), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(dirH), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(dirV), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Player Server Packet
/// </summary>
public class S_BroadcastMove : IPacket
{
    public int playerId;
    public float posX;
    public float posY;
    public float posZ;
    public float dirH;
    public float dirV;
    public float rotateY;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastMove; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        dirH = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        dirV = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastMove), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(dirH), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(dirV), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Train Client Packet
/// </summary>
public class C_TrainMove : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotateY;

    public ushort Protocol { get { return (ushort)PacketID.C_TrainMove; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_TrainMove), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Train Server Packet
/// </summary>
public class S_BroadcastTrainMove : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotateY;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastTrainMove; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastTrainMove), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Train Client Packet
/// </summary>
public class C_Shot : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotateY;

    public ushort Protocol { get { return (ushort)PacketID.C_Shot; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Shot), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Train Server Packet
/// </summary>
public class S_BroadcastShot : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotateY;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastShot; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastShot), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Map Seed Packet
/// </summary>
public class C_MapSeed : IPacket
{
    public int mapSeed;

    public ushort Protocol { get { return (ushort)PacketID.C_MapSeed; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        mapSeed = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Shot), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(mapSeed), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Server Map Seed Packet
/// </summary>
public class S_BroadcastMapSeed : IPacket
{
    public int mapSeed;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastMapSeed; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        mapSeed = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastMapSeed), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(mapSeed), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Enter Room Client
/// </summary>
public class C_EnterRoom : IPacket
{
    public int playerId;
    public int roomNum;

    public ushort Protocol { get { return (ushort)PacketID.C_EnterRoom; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        roomNum = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(roomNum), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}


/// <summary>
/// Enter Room Broadcast Server
/// </summary>
public class S_BroadcastEnterRoom : IPacket
{
    public int playerId;
    public int roomNum;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterRoom; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        roomNum = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(roomNum), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}


/// <summary>
/// Resource Client
/// </summary>
public class C_Resource : IPacket
{
    public int resourceIdx;

    public ushort Protocol { get { return (ushort)PacketID.C_Resource; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        resourceIdx = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(resourceIdx), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}


/// <summary>
/// ResourceBroadcast
/// </summary>
public class S_BroadcastResource : IPacket
{
    public int resourceIdx;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastResource; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        resourceIdx = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(resourceIdx), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// ClientHP
/// </summary>
public class C_Health : IPacket
{
    public int playerId;
    public int health;

    public ushort Protocol { get { return (ushort)PacketID.C_Health; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        health = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(health), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// ResourceBroadcast
/// </summary>
public class S_BroadcastHealth : IPacket
{
    public int playerId;
    public int health;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastHealth; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        health = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnterRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(health), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}


/// <summary>
/// Enemy Client Packet
/// </summary>
public class C_EnemyMove : IPacket
{
    public int enemyIdx;
    public float posX;
    public float posY;
    public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.C_EnemyMove; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        enemyIdx = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_EnemyMove), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(enemyIdx), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// Enemy Move Server Packet
/// </summary>
public class S_BroadcastEnemyMove : IPacket
{
    public int enemyIdx;
    public float posX;
    public float posY;
    public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnemyMove; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        enemyIdx = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnemyMove), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(enemyIdx), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}


/// <summary>
/// Player Client Packet
/// </summary>
public class C_PlayerStatus : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotateY;
    public int status;
    public int itemIdx;

    public ushort Protocol { get { return (ushort)PacketID.C_PlayerStatus; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        status = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        itemIdx = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_PlayerStatus), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(status), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(itemIdx), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// PlayerStatus Server Packet
/// </summary>
public class S_BroadcastPlayerStatus : IPacket
{
    public int playerId;
    public float posX;
    public float posY;
    public float posZ;
    public float rotateY;
    public int status;
    public int itemIdx;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastPlayerStatus; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        status = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        itemIdx = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastPlayerStatus), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(status), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(itemIdx), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}





/// <summary>
/// PickUp Client Packet
/// </summary>
public class C_PickUp : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotateY;
    public bool status;

    public ushort Protocol { get { return (ushort)PacketID.C_PickUp; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        status = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
        count += sizeof(bool);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_PickUp), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(status), 0, segment.Array, segment.Offset + count, sizeof(bool));
        count += sizeof(bool);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

/// <summary>
/// PickUp Server
/// </summary>
public class S_BroadcastPickUp : IPacket
{
    public int playerId;
    public float posX;
    public float posY;
    public float posZ;
    public float rotateY;
    public bool status;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastPickUp; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        rotateY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        status = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
        count += sizeof(bool);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastPickUp), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(rotateY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(status), 0, segment.Array, segment.Offset + count, sizeof(bool));
        count += sizeof(bool);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}