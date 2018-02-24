using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System;
using ConstValue;

public class CSender {

    private static CSender mInstance;
    NetworkStream stream;
    private CSender()
    {
        stream = CConnect.GetInstance().GetStream();
    }


    public static CSender GetInstance()
    {
        if (null == mInstance)
        {
            //            Debug.Log("Sender 객체 생성");
            mInstance = new CSender();
        }
        return mInstance;
    }

    public void Sendn(object dataPacket, PacketKindEnum packetKind)
    {
        try
        {
           // PacketMessage m = (PacketMessage)dataPacket;
           // Debug.Log("//// Sendn = " + m.Message);
            byte[] dataBuffer = Util.Serialize(packetKind, dataPacket);
            //Debug.Log("보내는 사이즈 : ");
            stream.Write(dataBuffer, 0, dataBuffer.Length);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
