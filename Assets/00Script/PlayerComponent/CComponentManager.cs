using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class CComponentManager {

    static CComponentManager mInstance;
    List<System.Type> mListComponentType;

    private CComponentManager()
    {
        mListComponentType = new List<System.Type>();
        mListComponentType.Add(typeof(MoveController));
        // 다음줄에 다음 컴포넌트 새로 추가.
    }

    public static CComponentManager GetInstance()
    {
        if(mInstance == null)
        {
            mInstance = new CComponentManager();
        }
        return mInstance;
    }

    public System.Type GetSystemType(ComponentEnum index)
    {
        if((mListComponentType.Count <= (int)index) || (0 > (int)index)) // index out of length 방지
        {
            return null;
        }
        return mListComponentType[(int)index];
    }

}
