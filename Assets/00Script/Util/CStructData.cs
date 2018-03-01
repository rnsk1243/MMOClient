using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ConstValue;

//[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
//public struct DataPacketInfo
//{
//    [MarshalAs(UnmanagedType.I4)]
//    public int InfoProtocol; //0
//    [MarshalAs(UnmanagedType.I4)]
//    public int RequestVal; //4
//    [MarshalAs(UnmanagedType.Struct)]
//    public MyTransform Tr; //36
//    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ConstValueInfo.ChatBufSize)]
//    public string ChatMessage;

//    public DataPacketInfo(int infoProtocol, int requestVal, MyTransform tr, string chatMessage)
//    {
//        InfoProtocol = infoProtocol;
//        RequestVal = requestVal;
//        Tr = tr;
//        ChatMessage = chatMessage;
//    }

//    public DataPacketInfo(int infoProtocol, int requestVal, string chatMessage)
//    {
//        InfoProtocol = infoProtocol;
//        RequestVal = requestVal;
//        Tr = new MyTransform();
//        ChatMessage = chatMessage;
//    }

//    public DataPacketInfo(int infoProtocol)
//    {
//        InfoProtocol = infoProtocol;
//        RequestVal = ConstValueInfo.WrongValue;
//        Tr = new MyTransform();
//        ChatMessage = "";
//    }

//    public byte[] Serialize()
//    {
//        // allocate a byte array for the struct data
//        var buffer = new byte[Marshal.SizeOf(typeof(DataPacketInfo))];

//        // Allocate a GCHandle and get the array pointer
//        var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
//        var pBuffer = gch.AddrOfPinnedObject();

//        // copy data from struct to array and unpin the gc pointer
//        Marshal.StructureToPtr(this, pBuffer, false);
        
//        gch.Free();

//        return buffer;
//    }

//    public void DeserializeData(ref byte[] data)
//    {
//        this.InfoProtocol = BitConverter.ToInt32(data, 0);
//        Debug.Log("받은 프로토콜 : " + this.InfoProtocol);
//        switch(this.InfoProtocol)
//        {
//            case (int)ProtocolInfo.Request:
//                this.DeserializeRequest(ref data);
//                this.DeserializeChat(ref data);
//                break;
//            case (int)ProtocolInfo.Tr:
//                this.DeserializeTr(ref data);
//                break;
//            case (int)ProtocolInfo.Chat:
//                this.DeserializeChat(ref data);
//                break;
//            default:
//                break;
//        }
//    }

//    private void DeserializeRequest(ref byte[] data)
//    {
//        this.RequestVal = BitConverter.ToInt32(data, 4);
//    }

//    private void DeserializeTr(ref byte[] data)
//    {
//       // this.InfoProtocol = BitConverter.ToInt32(data, 0);
//        MyVector3 pos = new MyVector3(
//            BitConverter.ToSingle(data, 8),
//            BitConverter.ToSingle(data, 12),
//            BitConverter.ToSingle(data, 16)
//            );
//        MyVector3 rot = new MyVector3(
//            BitConverter.ToSingle(data, 20),
//            BitConverter.ToSingle(data, 24),
//            BitConverter.ToSingle(data, 28)
//            );
//        MyVector3 sca = new MyVector3(
//            BitConverter.ToSingle(data, 32),
//            BitConverter.ToSingle(data, 36),
//            BitConverter.ToSingle(data, 40)
//            );
//        this.Tr = new MyTransform(pos, rot, sca);
//    }

//    private void DeserializeChat(ref byte[] data)
//    {
//        string chat = Encoding.Default.GetString(data, 44, ConstValueInfo.ChatBufSize);
//        this.ChatMessage = Util.RemoveNullString(ref chat);
//    }

//}

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

public class Util
{
    // null 문자 제거 함수
    public static string RemoveNullString(ref string str)
    {
        StringBuilder strBuilder = new StringBuilder(); ; // 문자열 더하고 빼는데 이용함
        for (int i = 0; i < str.Length; ++i)
        {
            if ('\0' == str[i])
            {
                break;
            }
            strBuilder.Append(str[i]);
        }
        return strBuilder.ToString();
        //this.InfoValue = strBuilder.ToString();
    }

    public static byte[] Serialize(PacketKindEnum packetKind, object targetStruct)
    {
        int sendSize = 0;
        switch(packetKind)
        {
            case PacketKindEnum.Transform:
                sendSize = Marshal.SizeOf(typeof(PacketTransform));
                break;
            case PacketKindEnum.Message:
                sendSize = Marshal.SizeOf(typeof(PacketMessage));
                break;
            default:
                return null;
        }
        // allocate a byte array for the struct data
        var buffer = new byte[sendSize];

        // Allocate a GCHandle and get the array pointer
        var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var pBuffer = gch.AddrOfPinnedObject();
        //PacketMessage m = (PacketMessage)targetStruct;
        //Debug.Log("//// SeeeSendn = " + m.Message);
        // copy data from struct to array and unpin the gc pointer
        Marshal.StructureToPtr(targetStruct, pBuffer, false);

        gch.Free();
        return buffer;
    }

    public static object DeserializeData(ref byte[] data, int packetKind)
    {
        if (data == null || packetKind == ConstValueInfo.WrongValue)
        {
            Debug.Log("DeserializeData 실패, data가 null이거나 packetKind가 초기화 되지 않음");
            return null;
        }
        //packetKind = DeserializeInt(ref data, ConstValueInfo.StartPointPacketKind);
        int protocolInfo = DeserializeInt(ref data, ConstValueInfo.StartPointProtocol);
        int distinguishCode = DeserializeInt(ref data, ConstValueInfo.StartPointDistinguishCode);
        Debug.Log("-packetKind = " + packetKind);
        Debug.Log("-protocolInfo = " + protocolInfo);
        Debug.Log("-distinguishCode = " + distinguishCode);
        switch (packetKind)
        {
            case (int)PacketKindEnum.Transform:
                MyTransform tr = DeserializeTr(ref data, ConstValueInfo.StartPointTr);
                return new PacketTransform(protocolInfo, distinguishCode, tr);
            case (int)PacketKindEnum.Message:
                int requestVal = DeserializeInt(ref data, ConstValueInfo.StartPointRequestVal);
                string message = DeserializeMessage(ref data, ConstValueInfo.StartPointMessage);
                Debug.Log("requestVal = " + requestVal);
                Debug.Log("message = " + message);
                return new PacketMessage(protocolInfo, distinguishCode, requestVal, message);
            default:
                return null;
        }
    }

    public static int DeserializeInt(ref byte[] data, int startPoint)
    {
        return BitConverter.ToInt32(data, startPoint);
    }

    public static MyTransform DeserializeTr(ref byte[] data, int startPoint)
    {
        MyVector3 pos = new MyVector3(
            BitConverter.ToSingle(data, (startPoint)),
            BitConverter.ToSingle(data, (startPoint + 4)),
            BitConverter.ToSingle(data, (startPoint + 8))
            );
        MyVector3 rot = new MyVector3(
            BitConverter.ToSingle(data, (startPoint + 12)),
            BitConverter.ToSingle(data, (startPoint + 16)),
            BitConverter.ToSingle(data, (startPoint + 20))
            );
        MyVector3 sca = new MyVector3(
            BitConverter.ToSingle(data, (startPoint + 24)),
            BitConverter.ToSingle(data, (startPoint + 28)),
            BitConverter.ToSingle(data, (startPoint + 32))
            );
        return new MyTransform(pos, rot, sca);
    }

    public static string DeserializeMessage(ref byte[] data, int startPoint)
    {
        string chat = Encoding.Default.GetString(data, startPoint, ConstValueInfo.MessageBufSize);
        return RemoveNullString(ref chat);
    }

    public static void RecvBufferFlush(Socket Sock)
    {
        byte[] tempBuf = new byte[ConstValueInfo.BufSizeRecv];
        Sock.Receive(tempBuf);
    }

    public static void ConvertToTransform(ref Transform target, ref MyTransform source)
    {
        Vector3 position = new Vector3(source.Position.x, source.Position.y, source.Position.z);
        Quaternion rotation = new Quaternion(source.Rotation.x, source.Rotation.y, source.Rotation.z, 0);
        Vector3 scale = new Vector3(source.Scale.x, source.Scale.y, source.Scale.z);

        target.position = position;
        target.rotation = rotation;
        target.localScale = scale;
    }

    public static Vector3 ConvertToVector3(ref MyVector3 source)
    {
        return new Vector3(source.x, source.y, source.z);
    }
}
