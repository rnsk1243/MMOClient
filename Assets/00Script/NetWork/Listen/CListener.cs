﻿using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using ConstValue;
using System;
using System.Threading;

public class CListener
{
    private CState mState;
    private static CListener mInstance;
    private Thread mThreadListen;
    private NetworkStream mStream;
    private CConnect mConnect;
    private Queue<PacketTransform> mNewClientTransform;
    private Queue<PacketTransform> mTrQueue;
    private Dictionary<string, int> mCommandValue;
    private Queue<PacketDeleteObj> mDeletePacketQueue;
    ///
    private PacketTransform mTakeTransform;
    private PacketMessage mTakeMessage;
    private PacketDeleteObj mTakeDeleteObj;

    private CListener()
    {
        mState = CState.GetInstance();
        mConnect = CConnect.GetInstance();
        mStream = mConnect.GetStream();
        mNewClientTransform = new Queue<PacketTransform>();
        mTrQueue = new Queue<PacketTransform>();
        mCommandValue = new Dictionary<string, int>();
        mDeletePacketQueue = new Queue<PacketDeleteObj>();
        mThreadListen = new Thread(new ThreadStart(Listen));
        //        Debug.Log("listen 시작");
        mThreadListen.Start();
        CState.GetInstance().SetConnectState(StateConnect.DistinguishCode);
        //mThreadListen.Join();
    }

    ~CListener()
    {
        //      Debug.Log("CListener 소멸자 호출");
        //mThreadListen.Abort();
    }

    public void TerminaterThread()
    {
               Debug.Log("TerminaterThread 호출");
        if (mThreadListen.IsAlive)
        {
               Debug.Log("mThreadListen 강제 종료 호출");
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

    public int GetRequestVal(string request)
    {
        int returnVal = ConstValueInfo.WrongValue;
        if (mCommandValue.ContainsKey(request) == true)
        {
             returnVal = mCommandValue[request];
            mCommandValue.Remove(request);
        }
        return returnVal;
    }

    public bool GetTrMessage(ref PacketTransform target)
    {
        if (0 != mTrQueue.Count)
        {
            target = mTrQueue.Dequeue();
            return true;
        }else
        {
            return false;
        }
    }

    public bool GetNewClientTransform(ref PacketTransform target)
    {
        if (0 != mNewClientTransform.Count)
        {
            target = mNewClientTransform.Dequeue();
            //Debug.Log("꺼냄 : " + target.DistinguishCode);
            return true;
        }else
        {
            return false;
        }
    }

    public int GetCurNewCreateCharacterAmount()
    {
        return mNewClientTransform.Count;
    }

    public bool GetDeleteObj(ref PacketDeleteObj target)
    {
        //Debug.Log("mDeletePacketQueue.Count = " + mDeletePacketQueue.Count);
        if (0 != mDeletePacketQueue.Count)
        {
           //Debug.Log("GetDeleteObj");
            target = mDeletePacketQueue.Dequeue();
            return true;
        }
        else
        {
            return false;
        }
    }


    private void Listen()
    {
        Debug.Log("Listen Thread Start!");
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
                //Debug.Log("NewLink___0");
                byte[] packetKindBuffer = new byte[4]; // 패킷 종류를 받을 버퍼
                //Debug.Log("NewLink___1");
                byte[] dataBuffer = new byte[ConstValueInfo.BufSizeRecv]; // 데이터를 받을 버퍼
                //Debug.Log("NewLink___2");
                int packetKind = ConstValueInfo.WrongValue;//Marshal.SizeOf(typeof(DataPacketInfo));
                int goalSize = ConstValueInfo.WrongValue;
                mStream.Read(packetKindBuffer, 0, 4); // 일단 어떤 패킷이 왔는지 알기위해 4byte만 잃어서 int packetKind 뽑아냄
                //Debug.Log("NewLink___3");
                packetKind = CUtil.DeserializeInt(ref packetKindBuffer, ConstValueInfo.StartPointPacketKind); // 뽑아낸 byte를 int로 변환
                //Debug.Log("NewLink___4");
                if (packetKind == ConstValueInfo.WrongValue || packetKind >= ConstValueInfo.PacketSizeArray.Length || packetKind < 0)
                {
                    Debug.Log("패킷 받을 종류가 잘못 됨.");
                    CUtil.RecvBufferFlush(mConnect.GetSocket());
                    continue;
                }
                //Debug.Log("NewLink___5");
                //Debug.Log("받을 패킷 종류 : " + packetKind);
                goalSize = ConstValueInfo.PacketSizeArray[packetKind];
                //Debug.Log("NewLink___6");
                //Debug.Log("받을 목표 사이즈 = " + goalSize);
                int curRecvedSize = 0;
                while (true)
                {
                    //Debug.Log("NewLink___7");
                    curRecvedSize += mStream.Read(dataBuffer, 0, goalSize);
                    //Debug.Log("NewLink___8");
                    if (curRecvedSize >= goalSize)
                    {
                        //Debug.Log("NewLink___9");
                        //CUtil.RecvBufferFlush(mConnect.GetSocket());
                        break;
                    }
                    //Debug.Log("NewLink___10");
                }
                //Debug.Log("NewLink___11");
                object obj = CUtil.DeserializeData(ref dataBuffer, packetKind);
                //Debug.Log("NewLink___12");
                //System.Text.Encoding utf8 = System.Text.Encoding.UTF8;

                //byte[] utf8Bytes = utf8.GetBytes(mMessage.Message);
                //string decodedStringByUTF8 = utf8.GetString(utf8Bytes);

                //byte[] byteFromStr = System.Text.Encoding.Unicode.GetBytes(mMessage.Message);
                //string result = System.Text.Encoding.Unicode.GetString(byteFromStr);
                DataCategorize(ref obj, packetKind);
                //Debug.Log("NewLink___13");
                Thread.Sleep(ConstValueInfo.ListenThreadSleep);
                //Debug.Log("NewLink___14");
                //CUtil.RecvBufferFlush(mConnect.GetSocket());
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log("에러 리슨함수..");
                //mThreadListen.Abort();
            }
        }
    }

    void DataCategorize(ref object dataPacket, int packetKind)
    {
        switch(packetKind)
        {
            case (int)PacketKindEnum.Transform:
                mTakeTransform = (PacketTransform)dataPacket;
                //Debug.Log("받은 tr.position.x = " + mTakeTransform.Tr.Position.x);
                ProtocolCategorize(mTakeTransform.InfoProtocol);
                break;
            case (int)PacketKindEnum.Message:
                mTakeMessage = (PacketMessage)dataPacket;
                //Debug.Log("New message.Message = " + mTakeMessage.Message);
                //Debug.Log("New message.RequestVal = " + mTakeMessage.RequestVal);
                ProtocolCategorize(mTakeMessage.InfoProtocol);
                break;
            case (int)PacketKindEnum.DeleteObj:
                //Debug.Log("PacketKind DeleteObj");
                mTakeDeleteObj = (PacketDeleteObj)dataPacket;
                ProtocolCategorize(mTakeDeleteObj.InfoProtocol);
                break;
            default:
                Debug.Log("분류 할 수 없는 DataPacket");
                break;
        }
        
        //switch (dataPacket.InfoProtocol)
        //{
        //    case (int)ProtocolInfo.Request:
        //        mCommandValue.Add(dataPacket.ChatMessage, dataPacket.RequestVal);
        //        break;
        //    case (int)ProtocolInfo.Tr:
        //        mTrQueue.Enqueue(dataPacket.Tr);
        //        Debug.Log("받은 Tr : " + dataPacket.Tr.Position.x);
        //        Debug.Log("받은 Tr : " + dataPacket.Tr.Rotation.x);
        //        Debug.Log("받은 Tr : " + dataPacket.Tr.Scale.x);
        //        break;
        //    case (int)ProtocolInfo.Chat:
        //        Debug.Log("받은 Message : " + dataPacket.ChatMessage);
        //        break;
        //    default:
        //        Debug.Log("분류 할 수 없는 enum ProtocolInfo에 등록 되어 있지 않음 = " + (int)ProtocolInfo.None);
        //        break;
        //}
    }

    private void ProtocolCategorize(int protocol)
    {
        switch(protocol)
        {
            case (int)ProtocolInfo.Request:
                if(mTakeMessage.Message == RequestCollection.SendDistinguishCode)
                {
                    mState.mCurAreaNumber = mTakeMessage.DistinguishCode;
                    Debug.Log("나의 현재 Area = " + mState.mCurAreaNumber);
                }
                mCommandValue.Add(mTakeMessage.Message, mTakeMessage.RequestVal);
                mTakeMessage.InfoProtocol = ConstValueInfo.WrongValue;
                break;
            case (int)ProtocolInfo.Tr:
                mTrQueue.Enqueue(mTakeTransform);
                mTakeTransform.InfoProtocol = ConstValueInfo.WrongValue;
                break;
            case (int)ProtocolInfo.Chat:
                break;
            case (int)ProtocolInfo.NewLink:
                mNewClientTransform.Enqueue(mTakeTransform);
                mTakeTransform.InfoProtocol = ConstValueInfo.WrongValue;
                break;
            case (int)ProtocolInfo.DeleteObj:
                //Debug.Log("ProtocolInfo DeleteObj");
                mDeletePacketQueue.Enqueue(mTakeDeleteObj);
                mTakeDeleteObj.InfoProtocol = ConstValueInfo.WrongValue;
                break;
            default:
                Debug.Log("분류할 수 없는 Protocol정보 = " + protocol);
                break;
        }
    }

}
