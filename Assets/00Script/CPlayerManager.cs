using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class CPlayerManager {

    static private CPlayerManager mInstance;
    private Dictionary<int, GameObject> mPlayerDictionary;
    private CInitDistinguishCode mDisCode;
    private CComponentManager mComponentManager;
   // private GameObject mTakeGameObj;

    private CPlayerManager()
    {
        mPlayerDictionary = new Dictionary<int, GameObject>();
        mDisCode = CInitDistinguishCode.GetInstance();
        mComponentManager = CComponentManager.GetInstance();
    }

    static public CPlayerManager GetInstance()
    {
        if (mInstance == null)
        {
            mInstance = new CPlayerManager();
        }
        return mInstance;
    }

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
        System.Type newComptype = mComponentManager.GetSystemType(compIndex);
        if (IsMakeAlready(mDisCode.GetMyDisCode()) == true && newComptype != null)
        {
            GameObject gameObj = mPlayerDictionary[disCode];
            gameObj.AddComponent(newComptype);
        }
    }

}
