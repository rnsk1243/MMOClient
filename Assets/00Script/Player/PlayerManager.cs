﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;
using System;
using System.Threading;

public class CPlayerManager : MonoBehaviour {

    static private CPlayerManager mInstance;
    private Dictionary<int, GameObject> mPlayerDictionary;
    private CInitDistinguishCode mDisCode;
    private CComponentManager mComponentManager;
    private CListener mListener;
    //private Thread mThreadOtherPlayerMove;

    private PacketTransform mTakePacketTransform;
    private CState mState;
    // private GameObject mTakeGameObj;

    private void Awake()
    {
        Debug.Log("CPlayerManager 만듬");
        mState = CState.GetInstance();
        mPlayerDictionary = new Dictionary<int, GameObject>();
        mDisCode = CInitDistinguishCode.GetInstance();
        mComponentManager = CComponentManager.GetInstance();
        mListener = CListener.GetInstance();
        //mThreadOtherPlayerMove = new Thread(new ThreadStart(OtherPlayerMove));
        //mThreadOtherPlayerMove.Start();
        mTakePacketTransform = new PacketTransform();
        StartCoroutine(OtherPlayerMove());
    }

    //public void TerminaterThread()
    //{
    //    //       Debug.Log("TerminaterThread 호출");
    //    if (mThreadOtherPlayerMove.IsAlive)
    //    {
    //        //           Debug.Log("mThreadListen 강제 종료 호출");
    //        mThreadOtherPlayerMove.Abort();
    //    }
    //}

    //private void GetPlayer(int disCode)
    //{
    //    if(mPlayerDictionary.ContainsKey(disCode) == true)
    //    {
    //        mTakeGameObj = mPlayerDictionary[disCode];
    //    }
    //}

    public void AddPlayer(int disCode, GameObject gameObj)
    {
        if(IsMakeAlready(disCode) == false)
        {
            mPlayerDictionary.Add(disCode, gameObj);
        }
    }

    public bool IsMakeAlready(int disCode)
    {
        return mPlayerDictionary.ContainsKey(disCode);
    }

    public void AddPlayerComponent()
    {
        if(IsMakeAlready(mDisCode.GetMyDisCode()) == true)
        {
            GameObject gameObj = mPlayerDictionary[mDisCode.GetMyDisCode()];
            gameObj.AddComponent<MoveController>();
            CState.GetInstance().SetConnectState(ConstValue.StateConnect.GameStart);
        }
    }

    public void AddGameObjComponent(int disCode, ComponentEnum compIndex)
    {
        Type newComptype = mComponentManager.GetSystemType(compIndex);
        if (IsMakeAlready(disCode) == true && newComptype != null) // gameObj가 존재하는지 && 컴포넌트를 가져왔는지 확인
        {
            GameObject gameObj = mPlayerDictionary[disCode];
            if(gameObj.GetComponent(newComptype) == null) // 이미 추가된 컴포넌트인지 확인
            {
                gameObj.AddComponent(newComptype);
            }
        }
    }

    IEnumerator OtherPlayerMove()
    {
        while (true)
        {
            if (mState.IsCurConnectState(StateConnect.GameStart))
            {
                if (mListener.GetTrMessage(ref mTakePacketTransform))
                {
                    if (IsMakeAlready(mTakePacketTransform.DistinguishCode) == true)
                    {
                        GameObject gameObj = mPlayerDictionary[mTakePacketTransform.DistinguishCode];
                        OtherPlayerMoveController OPMC = gameObj.GetComponent<OtherPlayerMoveController>();
                        if (OPMC != null)
                        {
                            OPMC.MovePositionUpdate(ref mTakePacketTransform.Tr.Position);
                            OPMC.MoveRotateUpdate(ref mTakePacketTransform.Tr.Rotation);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //private void OtherPlayerMove()
    //{
    //    while(true)
    //    {
    //        if(mState.IsCurConnectState(StateConnect.GameStart))
    //        {
    //            if (mListener.GetTrMessage(ref mTakePacketTransform))
    //            {
    //                if (IsMakeAlready(mTakePacketTransform.DistinguishCode) == true)
    //                {
    //                    GameObject gameObj = mPlayerDictionary[mTakePacketTransform.DistinguishCode];
    //                    OtherPlayerMoveController OPMC = gameObj.GetComponent<OtherPlayerMoveController>();
    //                    if (OPMC != null)
    //                    {
    //                        OPMC.Move(ref mTakePacketTransform.Tr.Position);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
