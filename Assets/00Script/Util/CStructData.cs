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

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct DataPacketInfo
{
    [MarshalAs(UnmanagedType.I4)]
    public int InfoProtocol;
    [MarshalAs(UnmanagedType.I4)]
    public int RequestVal;
    [MarshalAs(UnmanagedType.Struct)]
    public MyTransform Tr;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ConstValueInfo.ChatBufSize)]
    public string ChatMessage;

    public DataPacketInfo(int infoProtocol, int requestVal, MyTransform tr, string chatMessage)
    {
        InfoProtocol = infoProtocol;
        RequestVal = requestVal;
        Tr = tr;
        ChatMessage = chatMessage;
    }

    public DataPacketInfo(int infoProtocol, int requestVal, string chatMessage)
    {
        InfoProtocol = infoProtocol;
        RequestVal = requestVal;
        Tr = new MyTransform();
        ChatMessage = chatMessage;
    }

    public DataPacketInfo(int infoProtocol)
    {
        InfoProtocol = infoProtocol;
        RequestVal = ConstValueInfo.WrongValue;
        Tr = new MyTransform();
        ChatMessage = "";
    }

    public byte[] Serialize()
    {
        // allocate a byte array for the struct data
        var buffer = new byte[Marshal.SizeOf(typeof(DataPacketInfo))];

        // Allocate a GCHandle and get the array pointer
        var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var pBuffer = gch.AddrOfPinnedObject();

        // copy data from struct to array and unpin the gc pointer
        Marshal.StructureToPtr(this, pBuffer, false);
        
        gch.Free();

        return buffer;
    }

    public void DeserializeData(ref byte[] data)
    {
        this.InfoProtocol = BitConverter.ToInt32(data, 0);
        Debug.Log("받은 프로토콜 : " + this.InfoProtocol);
        switch(this.InfoProtocol)
        {
            case (int)ProtocolInfo.Request:
                this.DeserializeRequest(ref data);
                this.DeserializeChat(ref data);
                break;
            case (int)ProtocolInfo.Tr:
                this.DeserializeTr(ref data);
                break;
            case (int)ProtocolInfo.Chat:
                this.DeserializeChat(ref data);
                break;
            default:
                break;
        }
    }

    private void DeserializeRequest(ref byte[] data)
    {
        this.RequestVal = BitConverter.ToInt32(data, 4);
    }

    private void DeserializeTr(ref byte[] data)
    {
       // this.InfoProtocol = BitConverter.ToInt32(data, 0);
        MyVector3 pos = new MyVector3(
            BitConverter.ToSingle(data, 8),
            BitConverter.ToSingle(data, 12),
            BitConverter.ToSingle(data, 16)
            );
        MyVector3 rot = new MyVector3(
            BitConverter.ToSingle(data, 20),
            BitConverter.ToSingle(data, 24),
            BitConverter.ToSingle(data, 28)
            );
        MyVector3 sca = new MyVector3(
            BitConverter.ToSingle(data, 32),
            BitConverter.ToSingle(data, 36),
            BitConverter.ToSingle(data, 40)
            );
        this.Tr = new MyTransform(pos, rot, sca);
    }

    private void DeserializeChat(ref byte[] data)
    {
        string chat = Encoding.Default.GetString(data, 44, ConstValueInfo.ChatBufSize);
        this.ChatMessage = Util.RemoveNullString(ref chat);
    }

}

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
}
