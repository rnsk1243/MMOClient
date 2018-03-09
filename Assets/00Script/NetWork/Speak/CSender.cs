using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System;
using ConstValue;
using System.Threading;

public class CSender {

    private static CSender mInstance;
    private CConnect mConnect;
    NetworkStream stream;
    private Queue<Packet> mSendQueue;
    private Thread mThreadSender;

    private CSender()
    {
        mConnect = CConnect.GetInstance();
        stream = mConnect.GetStream();
        mSendQueue = new Queue<Packet>();
        mThreadSender = new Thread(new ThreadStart(Sendn));
        mThreadSender.Start();
        CState.GetInstance().SetConnectState(StateConnect.DistinguishCode);
    }

    public void TerminaterThread()
    {
               Debug.Log("TerminaterThread 호출");
        if (mThreadSender.IsAlive)
        {
                       Debug.Log("mThreadListen 강제 종료 호출");
            mThreadSender.Abort();
        }
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

    private void Sendn()
    {
        Debug.Log("Send Thread Start!");
        while(true)
        {
            try
            {
                if (!mConnect.IsConnected())
                {
                    Debug.Log("연결 끊김 Sendn함수 종료..");
                    //mThreadListen.Abort();
                    //TerminaterThread();
                    return;
                }
                if (mSendQueue.Count != 0)
                {
                    Packet targetSendData = mSendQueue.Dequeue();
                    // PacketMessage m = (PacketMessage)dataPacket;
                    // Debug.Log("//// Sendn = " + m.Message);
                    byte[] dataBuffer = CUtil.Serialize(targetSendData.PacketKind, targetSendData.DataPacket);
                    //Debug.Log("보내는 사이즈 : ");
                    stream.Write(dataBuffer, 0, dataBuffer.Length);
                }
                Thread.Sleep(ConstValueInfo.SendThreadSleep);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    public void PushSendData(object dataPacket, PacketKindEnum packetKind)
    {
        Packet packet = new Packet(ref dataPacket, packetKind);
        mSendQueue.Enqueue(packet);
    }



}
