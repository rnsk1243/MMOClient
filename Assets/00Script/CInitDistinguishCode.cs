using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;
public class CInitDistinguishCode {

    static CInitDistinguishCode mInstance;
    static int mMyDistinguishCode;
    CSender mSender;
    CListener mListener;
    

    private CInitDistinguishCode()
    {
        mMyDistinguishCode = ConstValueInfo.WrongValue;
        mListener = CListener.GetInstance();
        mSender = CSender.GetInstance();
    }

    static public CInitDistinguishCode GetInstance()
    {
        if(mInstance == null)
        {
            mInstance = new CInitDistinguishCode();
        }
        return mInstance;
    }

    public void GetMyDisCode()
    {
        if(CState.GetInstance().IsCurConnectState(StateConnect.DistinguishCode) == true)
        {
            int requestVal = mListener.GetRequestVal(RequestCollection.SendDistinguishCode);
            if (requestVal != ConstValueInfo.WrongValue)
            {
                mMyDistinguishCode = requestVal;
                CState.GetInstance().SetConnectState(StateConnect.AddComponent);
                Debug.Log("나의 구분 번호 : " + mMyDistinguishCode);
            }else
            {
                DataPacketInfo requestMyDisCodePacket = new DataPacketInfo((int)ProtocolInfo.Request, ConstValueInfo.WrongValue, RequestCollection.SendDistinguishCode);
                mSender.Sendn(ref requestMyDisCodePacket); // 구분 코드 요청
            }
        }
    }

}
