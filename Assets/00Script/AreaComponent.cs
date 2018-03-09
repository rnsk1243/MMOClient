using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class AreaComponent : MonoBehaviour {

    public int mRightAreaNumber;
    public int mLeftAreaNumber;
    int goalArea;
    //bool mIsMoveAreaRequest; // 요청 했나?

	// Use this for initialization
	void Awake ()
    {
        //mIsMoveAreaRequest = false;
        StartCoroutine(CheckMoveComplete());
	}

    IEnumerator CheckMoveComplete()
    {
        CState state = CState.GetInstance();
        while(true)
        {
            if (state.IsCurConnectState(StateConnect.GameStart))
            {
                CListener listener = CListener.GetInstance();
                int resultVal = listener.GetRequestVal(RequestCollection.SendMoveAreaComplete);
                if(resultVal == goalArea && resultVal != ConstValueInfo.WrongValue)
                {
                    state.mCurAreaNumber = resultVal;
                    //mIsMoveAreaRequest = false;
                    Debug.Log("Area 이동 성공 = " + state.mCurAreaNumber);
                }
                else
                {
                    //Debug.Log("잘 못 된 Area 이동");
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CState state = CState.GetInstance();
        //Debug.Log("OnTriggerEnter = " + state.IsCurConnectState(StateConnect.GameStart));
        //Debug.Log("other.GetComponent<MoveController>() != null = " + other.GetComponent<MoveController>() != null);
        if (state.IsCurConnectState(StateConnect.GameStart) && other.GetComponent<MoveController>() != null)
        {
            int curMyArea = state.mCurAreaNumber;
            //Debug.Log("/////// = " + curMyArea);
            goalArea = (curMyArea == mRightAreaNumber) ? mLeftAreaNumber : mRightAreaNumber;
            //Debug.Log("Area이동 시작 목표 Area = " + goalArea);

            CInitDistinguishCode discodeObj = CInitDistinguishCode.GetInstance();
            PacketMessage moveRequestPacket
                    = new PacketMessage(
                        (int)ProtocolInfo.Request,
                        discodeObj.GetMyDisCode(),
                        goalArea,
                        RequestCollection.SendMoveArea);
            CSender.GetInstance().PushSendData(moveRequestPacket, PacketKindEnum.Message);
            //mIsMoveAreaRequest = true;
        }
    }

}
