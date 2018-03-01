using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class CreatePlayer : MonoBehaviour {

    CState mState;
    CListener mListener;
    private PacketTransform mTakeTransform;
    CPlayerManager mPlayerManager;
    //GameObject mBasicPlayerPrefab; // 기본 플레이어 프레임.
    InitGame mInitGame;

    private void Awake()
    {
        //mBasicPlayerPrefab = Resources.Load("Character/Player") as GameObject;
        mInitGame = GetComponent<InitGame>();
        mState = CState.GetInstance();
        mListener = CListener.GetInstance();
        mPlayerManager = CPlayerManager.GetInstance();
        StartCoroutine(CreatePlayerStart());
        mState.SetConnectState(ConstValue.StateConnect.AddComponent);
    }

    IEnumerator CreatePlayerStart()
    {
        while(true)
        {
            if(mState.GetConnectState() >= ConstValue.StateConnect.CreateCharacter)
            {
                mListener.GetNewClientTransform(ref mTakeTransform);
                int newPlayerDisCode = mTakeTransform.DistinguishCode;
                Debug.Log("도전 = " + newPlayerDisCode + " // 이미 만들어 짐 = " + mPlayerManager.IsMakeAlready(newPlayerDisCode));
                if (newPlayerDisCode != ConstValueInfo.WrongValue && (mPlayerManager.IsMakeAlready(newPlayerDisCode) == false))
                {
                    Debug.Log("만든다. = " + newPlayerDisCode);
                    GameObject gameObj;
                    if (newPlayerDisCode == CInitDistinguishCode.GetInstance().GetMyDisCode())
                    {
                        gameObj = Instantiate(mInitGame.mBasicMyPlayer);
                    }
                    else
                    {
                        gameObj = Instantiate(mInitGame.mOtherPlayer);
                    }
                    gameObj.GetComponent<Transform>().position = Util.ConvertToVector3(ref mTakeTransform.Tr.Position);
                    gameObj.GetComponent<Transform>().rotation = Quaternion.Euler(Util.ConvertToVector3(ref mTakeTransform.Tr.Rotation));
                    //gameObj.GetComponent<Transform>().localScale = Util.ConvertToVector3(ref mTakeTransform.Tr.Scale);
                    mPlayerManager.AddPlayer(newPlayerDisCode, gameObj);
                    mTakeTransform.DistinguishCode = ConstValueInfo.WrongValue;
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
