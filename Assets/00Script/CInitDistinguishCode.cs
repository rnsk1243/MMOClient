using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;
public class CInitDistinguishCode {

    static CInitDistinguishCode mInstance;
    private int mMyDistinguishCode;
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

    public void RequestMyDisCode()
    {
        if (CState.GetInstance().IsCurConnectState(StateConnect.DistinguishCode) == true)
        {
            int requestVal = mListener.GetRequestVal(RequestCollection.SendDistinguishCode);
            if (requestVal != ConstValueInfo.WrongValue)
            {
                mMyDistinguishCode = requestVal;
                CState.GetInstance().SetConnectState(StateConnect.CreateCharacter);
                Debug.Log("나의 구분 번호 : " + mMyDistinguishCode);
            }
            else
            {
                //DataPacketInfo requestMyDisCodePacket = new DataPacketInfo((int)ProtocolInfo.Request, ConstValueInfo.WrongValue, RequestCollection.SendDistinguishCode);
                //mSender.Sendn(ref requestMyDisCodePacket); // 구분 코드 요청
                PacketMessage requestMyDisCodePacket
                    = new PacketMessage(
                        (int)ProtocolInfo.Request,
                        ConstValueInfo.WrongValue,
                        ConstValueInfo.WrongValue,
                        RequestCollection.SendDistinguishCode);
                Debug.Log("구분번호 요청 하기 : " + requestMyDisCodePacket.Message);
                mSender.PushSendData(requestMyDisCodePacket, PacketKindEnum.Message); //Sendn(requestMyDisCodePacket, PacketKindEnum.Message);
            }
        }
    }

    public int GetMyDisCode()
    {
        return mMyDistinguishCode;
    }

}
