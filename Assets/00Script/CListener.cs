using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using ConstValue;
using System;
using System.Threading;

public class CListener
{

    private static CListener mInstance;
    private Thread mThreadListen;
    private NetworkStream mStream;
    private CConnect mConnect;
    private Queue<MyTransform> mTrQueue;
    ///

    private CListener()
    {
        mTrQueue = new Queue<MyTransform>();
        mConnect = CConnect.GetInstance();
        mStream = mConnect.GetStream();
        mThreadListen = new Thread(new ThreadStart(Listen));
        //        Debug.Log("listen 시작");
        mThreadListen.Start();
        //mThreadListen.Join();
    }

    ~CListener()
    {
        //      Debug.Log("CListener 소멸자 호출");
        //mThreadListen.Abort();
    }

    public void TerminaterThread()
    {
        //       Debug.Log("TerminaterThread 호출");
        if (mThreadListen.IsAlive)
        {
            //           Debug.Log("mThreadListen 강제 종료 호출");
            mThreadListen.Abort();
        }
    }

    public static CListener GetInstance()
    {
        if (null == mInstance)
        {
            //           Debug.Log("Listener 객체 생성");
            mInstance = new CListener();
        }
        return mInstance;
    }

    //public MyTransform GetTrMessage()
    //{
    //    if (0 != mTrQueue.Count)
    //    {
    //        return mTrQueue.Dequeue();
    //    }
    //    return null;
    //}



    public void Listen()
    {
        while (true)
        {
            try
            {
                if (!mConnect.IsConnected())
                {
                    Debug.Log("연결 끊김 Listen함수 종료..");
                    //mThreadListen.Abort();
                    //TerminaterThread();
                    return;
                }

                byte[] dataBuffer = new byte[ConstValueInfo.BufSizeRecv];
                int goalSize = Marshal.SizeOf(typeof(DataPacketInfo));
                Debug.Log("받을 목표 사이즈 = " + goalSize);
                int curRecvedSize = 0;
                while (true)
                {
                    curRecvedSize += mStream.Read(dataBuffer, 0, goalSize);
                    if (curRecvedSize >= goalSize)
                    {
                        break;
                    }
                }
                DataPacketInfo dataPacket = new DataPacketInfo();
                dataPacket.DeserializeData(ref dataBuffer);
                //System.Text.Encoding utf8 = System.Text.Encoding.UTF8;

                //byte[] utf8Bytes = utf8.GetBytes(mMessage.Message);
                //string decodedStringByUTF8 = utf8.GetString(utf8Bytes);

                //byte[] byteFromStr = System.Text.Encoding.Unicode.GetBytes(mMessage.Message);
                //string result = System.Text.Encoding.Unicode.GetString(byteFromStr);

                DataCategorize(ref dataPacket);
                Thread.Sleep(100);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log("에러 리슨함수 종료..");
                return;
                //mThreadListen.Abort();
            }
        }
    }

    // 여기서는 CheckState.ChangeSceneState만 하고 CheckState.ChangeState(스테이트 변경)은 CheckState에서 할 것.
    void DataCategorize(ref DataPacketInfo dataPacket)
    {
        switch (dataPacket.InfoProtocol)
        {
            case (int)ProtocolInfo.Tr:
                mTrQueue.Enqueue(dataPacket.Tr);
                Debug.Log("받은 Tr : " + dataPacket.Tr.Position.x);
                Debug.Log("받은 Tr : " + dataPacket.Tr.Rotation.x);
                Debug.Log("받은 Tr : " + dataPacket.Tr.Scale.x);
                break;
            case (int)ProtocolInfo.Chat:
                Debug.Log("받은 Message : " + dataPacket.ChatMessage);
                break;
            default:
                Debug.Log("분류 할 수 없는 enum ProtocolInfo에 등록 되어 있지 않음 = " + (int)ProtocolInfo.None);
                break;
        }
    }

}
