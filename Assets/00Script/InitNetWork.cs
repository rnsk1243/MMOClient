using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitNetWork : MonoBehaviour {

    CConnect mConnet;
    CListener mListener;
	// Use this for initialization
	void Awake () {
        mConnet = CConnect.GetInstance();
        mListener = CListener.GetInstance();
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
