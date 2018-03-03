using System.Runtime.InteropServices;
using UnityEngine;
using System.Text;
using System;
using System.Net.Sockets;
using ConstValue;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;

public class CUtil {

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
        switch (packetKind)
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

    public static MyVector3 ConvertGetMyVector(Vector3 source)
    {
        return new MyVector3(source.x, source.y, source.z);
    }

    public static MyTransform ConvertGetMyTransform(ref Transform source)
    {
        MyVector3 position = ConvertGetMyVector(source.position);
        MyVector3 rotation = ConvertGetMyVector(new Vector3(source.rotation.x, source.rotation.y, source.rotation.z));
        MyVector3 scale = ConvertGetMyVector(source.localScale);
        return new MyTransform(position, rotation, scale);
    }

}
