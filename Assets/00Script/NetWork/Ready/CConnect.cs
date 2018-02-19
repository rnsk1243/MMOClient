using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class CConnect {

    private static CConnect mInstance;
    private TcpClient mTcpClient;
    private NetworkStream mStream;
    private string mIP;
    private int mPort;
    private string mPortStr;

    private CConnect()
    {
        CReader.GetInstance().LoadTextFile(ref mIP, ConstValueInfo.ServerIP_TextName);
        CReader.GetInstance().LoadTextFile(ref mPortStr, ConstValueInfo.ServerPort_TextName);
        if (mIP == null)
        {
            UnityEngine.Debug.Log("IP정보 파일 못 읽음");
            mIP = ConstValueInfo.IPAddress;
            //mIP = "192.168.200.160";
        }
        if (mPortStr != null)
        {
            mPort = int.Parse(mPortStr);
        }
        else
        {
            UnityEngine.Debug.Log("Port정보 파일 못 읽음");
            mPort = ConstValueInfo.Port;
        }
        mTcpClient = new TcpClient();
        mTcpClient.Connect(mIP, mPort);
        mStream = mTcpClient.GetStream();
        CState.GetInstance().SetConnectState(StateConnect.DistinguishCode);
    }
    ~CConnect()
    {
        UnityEngine.Debug.Log("CReadyNetWork 소멸자 호출");
        mStream.Close();
        mTcpClient.Close();
    }

    public static CConnect GetInstance()
    {
        if(null == mInstance)
        {
            mInstance = new CConnect();
        }
        return mInstance;
    }

    public bool IsConnected()
    {
        return mTcpClient.Connected;
    }

    public NetworkStream GetStream()
    {
        return mStream;
    }

    public void CloseStream()
    {
        UnityEngine.Debug.Log("mStream Close() 호출");
        mStream.Close();
    }

    public void CloseClient()
    {
        UnityEngine.Debug.Log("mTcpClient Close() 호출");
        mTcpClient.Close();
    }

}
