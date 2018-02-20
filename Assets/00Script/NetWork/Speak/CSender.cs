using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System;

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

    public void Sendn(ref DataPacketInfo dataPacket)
    {
        try
        {
          //  Debug.Log("//Sendn = " + dataPacket.InfoProtocol);
            byte[] dataBuffer = dataPacket.Serialize();
            stream.Write(dataBuffer, 0, dataBuffer.Length);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
