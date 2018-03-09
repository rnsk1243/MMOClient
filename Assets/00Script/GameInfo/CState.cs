using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class CState
{
    StateConnect mStateConnect;
    static CState mInstance;
    public int mCurAreaNumber;

    private CState()
    {
        mCurAreaNumber = ConstValueInfo.WrongValue;
        mStateConnect = StateConnect.Connecting;
    }

    public static CState GetInstance()
    {
        if (mInstance == null)
        {
            mInstance = new CState();
        }
        return mInstance;
    }

    public void SetConnectState(StateConnect newState)
    {
        mStateConnect = newState;
    }

    public bool IsCurConnectState(StateConnect state)
    {
        if(mStateConnect == state)
        { 
            return true;
        }else
        {
            return false;
        }
    }

    public StateConnect GetConnectState()
    {
        return mStateConnect;
    }

}
