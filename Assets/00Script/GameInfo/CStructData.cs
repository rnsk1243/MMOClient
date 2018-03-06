using System.Runtime.InteropServices;
using ConstValue;

[StructLayout(LayoutKind.Explicit, Size = 36, Pack = 1)]
public struct MyTransform
{
    [FieldOffset(0)]
    public MyVector3 Position;
    [FieldOffset(12)]
    public MyVector3 Rotation;
    [FieldOffset(24)]
    public MyVector3 Scale;

    public MyTransform(MyVector3 pos, MyVector3 rot, MyVector3 sca)
    {
        Position = pos;
        Rotation = rot;
        Scale = sca;
    }
}

[StructLayout(LayoutKind.Explicit, Size = 12, Pack =1)]
public struct MyVector3
{
    [FieldOffset(0)]
   public float x;
    [FieldOffset(4)]
    public float y;
    [FieldOffset(8)]
    public float z;

    public MyVector3(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
}

/// <summary>

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct PacketTransform
{
    [MarshalAs(UnmanagedType.I4)]
    public int PacketKind;
    [MarshalAs(UnmanagedType.I4)]
    public int InfoProtocol;
    [MarshalAs(UnmanagedType.I4)]
    public int DistinguishCode;
    [MarshalAs(UnmanagedType.Struct)]
    public MyTransform Tr;

    public PacketTransform(int infoProtocol, int distinguishCode, MyTransform tr)
    {
        PacketKind = (int)PacketKindEnum.Transform;
        InfoProtocol = infoProtocol;
        DistinguishCode = distinguishCode;
        Tr = tr;
    }
}

// 144byte
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct PacketMessage
{
    [MarshalAs(UnmanagedType.I4)]
    public int PacketKind;
    [MarshalAs(UnmanagedType.I4)]
    public int InfoProtocol;
    [MarshalAs(UnmanagedType.I4)]
    public int DistinguishCode;
    [MarshalAs(UnmanagedType.I4)]
    public int RequestVal;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ConstValueInfo.MessageBufSize)]
    public string Message;

    public PacketMessage(int infoProtocol, int distinguishCode, int requestVal, string message)
    {
        PacketKind = (int)PacketKindEnum.Message;
        InfoProtocol = infoProtocol;
        DistinguishCode = distinguishCode;
        RequestVal = requestVal;
        Message = message;
    }
}

// 52byte
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct PacketDeleteObj
{
    [MarshalAs(UnmanagedType.I4)]
    public int PacketKind;
    [MarshalAs(UnmanagedType.I4)]
    public int InfoProtocol;
    [MarshalAs(UnmanagedType.I4)]
    public int DistinguishCode;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = ConstValueInfo.SendEraseObjArraySize)]
    public int[] EraseObjDiscodeArray;

    public PacketDeleteObj(int infoProtocol, int distinguishCode)
    {
        PacketKind = (int)PacketKindEnum.DeleteObj;
        InfoProtocol = infoProtocol;
        DistinguishCode = distinguishCode;
        EraseObjDiscodeArray = new int[ConstValueInfo.SendEraseObjArraySize];
        for(int i=0; i<ConstValueInfo.SendEraseObjArraySize; i++)
        {
            EraseObjDiscodeArray[i] = ConstValueInfo.WrongValue;
        }
    }
}

public struct Packet
{
    public object DataPacket;
    public PacketKindEnum PacketKind;

    public Packet(ref object dataPacket, PacketKindEnum packetKind)
    {
        DataPacket = dataPacket;
        PacketKind = packetKind;
    }
}

/// </summary>

