using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class InitNetWork : MonoBehaviour {

    CConnect mConnet;
    CListener mListener;
    CSender mSender;
	// Use this for initialization
	void Awake () {
        mConnet = CConnect.GetInstance();
        mListener = CListener.GetInstance();
        mSender = CSender.GetInstance();
	}

    private void Start()
    {
        MyVector3 pos = new MyVector3(1.1f, 1.2f, 1.3f);
        MyVector3 rot = new MyVector3(2.1f, 2.2f, 2.3f);
        MyVector3 sca = new MyVector3(3.1f, 3.2f, 3.3f);
        MyTransform tr = new MyTransform(pos, rot, sca);
        DataPacketInfo testSendData = new DataPacketInfo((int)ProtocolInfo.Tr, tr, "abc");
        mSender.Sendn(ref testSendData);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnApplicationQuit()
    {
        //DataPacketInfo quitInfo = new DataPacketInfo((int)ProtocolInfo.ExitGameProcess, (int)ProtocolDetail.Message, (int)ProtocolMessageTag.Text, "으악 나 죽네");
        //sender.Sendn(ref quitInfo);
        //Debug.Log("OnApplicationQuit 호출");
        mConnet.CloseStream();
        mConnet.CloseClient();
        //Debug.Log("readyNetWork null로 초기화 시킴");
        mConnet = null;
        mListener.TerminaterThread();
        mListener = null;
        //sender = null;
    }

}
