using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class DeletePlayer : MonoBehaviour {

    private CState mState;
    private CListener mListener;

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
            PacketDeleteObj takeDeleteObjPacket = new PacketDeleteObj();
            //mTakeDeleteObjPacket.DistinguishCode = ConstValueInfo.WrongValue;
            if (mListener.GetDeleteObj(ref takeDeleteObjPacket))
            {
                int[] eraseArray = takeDeleteObjPacket.EraseObjDiscodeArray;
                //Debug.Log("!!!! 지울 배열 크기 : " + eraseArray.Length);
                for(int i=0; i<eraseArray.Length; i++)
                {
                    if(eraseArray[i] != CInitDistinguishCode.GetInstance().GetMyDisCode())
                    {
                        playerManager.DeletePlayerObj(eraseArray[i]);
                    }
                }
            }
        }
    }
}
