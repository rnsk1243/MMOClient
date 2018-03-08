using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class CreatePlayer : MonoBehaviour {

    private CState mState;
    private CListener mListener;
    private PacketTransform mTakeTransform;
    private InitGame mInitGame;
    private int mInitCreateCharacterAmount;
    //GameObject mBasicPlayerPrefab; // 기본 플레이어 프레임.

    private void Awake()
    {
        mInitGame = GetComponent<InitGame>();
        mState = CState.GetInstance();
        mListener = CListener.GetInstance();
        mInitCreateCharacterAmount = mListener.GetCurNewCreateCharacterAmount();
    }

    public void CreateCharacter(PlayerManager playerManager)
    {
        if (mState.GetConnectState() >= StateConnect.CreateCharacter)
        {
            mTakeTransform.DistinguishCode = ConstValueInfo.WrongValue;
            if(mListener.GetNewClientTransform(ref mTakeTransform))
            {
                int newPlayerDisCode = mTakeTransform.DistinguishCode;
                //Debug.Log("도전 = " + newPlayerDisCode + " // 이미 만들어 짐 = " + mPlayerManager.IsMakeAlready(newPlayerDisCode));
                if ((playerManager.IsMakeAlready(newPlayerDisCode) == false))
                {
                    //Debug.Log("만든다. = " + newPlayerDisCode);
                    GameObject gameObj;
                    if (newPlayerDisCode == CInitDistinguishCode.GetInstance().GetMyDisCode())
                    {
                        gameObj = Instantiate(mInitGame.mBasicMyPlayer);
                        gameObj.AddComponent<MoveController>();
                    }
                    else
                    {
                        gameObj = Instantiate(mInitGame.mOtherPlayer);
                        OtherPlayerMoveController opmc = gameObj.AddComponent<OtherPlayerMoveController>();
                        opmc.MovePositionUpdate(ref mTakeTransform.Tr.Position);
                        opmc.MoveRotateUpdate(ref mTakeTransform.Tr.Rotation);
                    }
                    //Debug.Log("생성 위치 = " + mTakeTransform.Tr.Position.x);
                    gameObj.GetComponent<Transform>().position = CUtil.ConvertToVector3(ref mTakeTransform.Tr.Position);
                    gameObj.GetComponent<Transform>().rotation = Quaternion.Euler(CUtil.ConvertToVector3(ref mTakeTransform.Tr.Rotation));
                    //gameObj.GetComponent<Transform>().localScale = CUtil.ConvertToVector3(ref mTakeTransform.Tr.Scale);
                    playerManager.AddPlayer(newPlayerDisCode, gameObj);
                    mState.SetConnectState(StateConnect.GameStart);
                    //GameStartCountDown();
                }

            }
        }
    }

    private void GameStartCountDown()
    {
        if (mState.GetConnectState() < StateConnect.GameStart)
        {
            if (mInitCreateCharacterAmount <= 0)
            {
                mState.SetConnectState(StateConnect.GameStart);
            }
            else
            {
                --mInitCreateCharacterAmount;
            }
        }
    }

    //IEnumerator CreatePlayerStart()
    //{
    //    while(true)
    //    {
    //        if(mState.GetConnectState() >= StateConnect.CreateCharacter)
    //        {
    //            mTakeTransform.DistinguishCode = ConstValueInfo.WrongValue;
    //            mListener.GetNewClientTransform(ref mTakeTransform);
    //            int newPlayerDisCode = mTakeTransform.DistinguishCode;
    //            //Debug.Log("도전 = " + newPlayerDisCode + " // 이미 만들어 짐 = " + mPlayerManager.IsMakeAlready(newPlayerDisCode));
    //            if (newPlayerDisCode != ConstValueInfo.WrongValue && (mPlayerManager.IsMakeAlready(newPlayerDisCode) == false))
    //            {
    //                Debug.Log("만든다. = " + newPlayerDisCode);
    //                GameObject gameObj;
    //                if (newPlayerDisCode == CInitDistinguishCode.GetInstance().GetMyDisCode())
    //                {
    //                    gameObj = Instantiate(mInitGame.mBasicMyPlayer);
    //                    gameObj.AddComponent<MoveController>();
    //                }
    //                else
    //                {
    //                    gameObj = Instantiate(mInitGame.mOtherPlayer);
    //                    gameObj.AddComponent<OtherPlayerMoveController>();
    //                }
    //                gameObj.GetComponent<Transform>().position = CUtil.ConvertToVector3(ref mTakeTransform.Tr.Position);
    //                gameObj.GetComponent<Transform>().rotation = Quaternion.Euler(CUtil.ConvertToVector3(ref mTakeTransform.Tr.Rotation));
    //                //gameObj.GetComponent<Transform>().localScale = CUtil.ConvertToVector3(ref mTakeTransform.Tr.Scale);
    //                mPlayerManager.AddPlayer(newPlayerDisCode, gameObj);
    //            }
    //        }
    //        yield return new WaitForSeconds(0.3f);
    //    }
    //}
}
