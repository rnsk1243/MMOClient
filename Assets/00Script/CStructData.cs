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
    [MarshalAs(UnmanagedType.Struct)]
    public MyTransform Tr;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ConstValueInfo.ChatBufSize)]
    public string ChatMessage;
    //public DataPacketInfo(int infoProtocol, MyTransform tr)
    //{
    //    InfoProtocol = infoProtocol;
    //    Tr = tr;
    //}

    public void DeserializeData(ref byte[] data)
    {
        this.InfoProtocol = BitConverter.ToInt32(data, 0);
        Debug.Log("받은 프로토콜 : " + this.InfoProtocol);
        switch(this.InfoProtocol)
        {
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

    private void DeserializeTr(ref byte[] data)
    {
       // this.InfoProtocol = BitConverter.ToInt32(data, 0);
        MyVector3 pos = new MyVector3(
            BitConverter.ToSingle(data, 4),
            BitConverter.ToSingle(data, 8),
            BitConverter.ToSingle(data, 12)
            );
        MyVector3 rot = new MyVector3(
            BitConverter.ToSingle(data, 16),
            BitConverter.ToSingle(data, 20),
            BitConverter.ToSingle(data, 24)
            );
        MyVector3 sca = new MyVector3(
            BitConverter.ToSingle(data, 28),
            BitConverter.ToSingle(data, 32),
            BitConverter.ToSingle(data, 36)
            );
        this.Tr = new MyTransform(pos, rot, sca);
    }

    private void DeserializeChat(ref byte[] data)
    {
        string chat = Encoding.Default.GetString(data, 40, ConstValueInfo.ChatBufSize);
        this.ChatMessage = Util.RemoveNullString(ref chat);
    }

}

public class MyTransform
{
   public MyVector3 Position;
   public MyVector3 Rotation;
   public MyVector3 Scale;

    public MyTransform(MyVector3 pos, MyVector3 rot, MyVector3 sca)
    {
        Position = pos;
        Rotation = rot;
        Scale = sca;
    }
}

public struct MyVector3
{
   public float x;
   public float y;
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
