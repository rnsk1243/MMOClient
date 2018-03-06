using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class DeletePlayer : MonoBehaviour {

    private CState mState;
    private CListener mListener;
    private PacketDeleteObj mTakeDeleteObjPacket;

    // Use this for initialization
    private void Awake()
    {
        mState = CState.GetInstance();
        mListener = CListener.GetInstance();
    }

    public void DeleteCharacter(PlayerManager playerManager)
    {
        if(mState.IsCurConnectState(StateConnect.GameStart))
        {
            //mTakeDeleteObjPacket.DistinguishCode = ConstValueInfo.WrongValue;
            if(mListener.GetDeleteObj(ref mTakeDeleteObjPacket))
            {
                int[] eraseArray = mTakeDeleteObjPacket.EraseObjDiscodeArray;
                Debug.Log("!!!! 지울 배열 크기 : " + eraseArray.Length);
                for(int i=0; i<eraseArray.Length; i++)
                {
                    playerManager.DeletePlayerObj(eraseArray[i]);
                }
            }
        }
    }
}
