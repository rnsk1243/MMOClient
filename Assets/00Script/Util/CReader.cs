using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CReader
{

    private static CReader mInstance;
    public static CReader GetInstance()
    {
        if (null == mInstance)
        {
            mInstance = new CReader();
        }
        return mInstance;
    }

    public void LoadTextFile(ref string resultStr, string filePathName)
    {
        try
        {
            StreamReader SR = new StreamReader(Application.dataPath + "/Resources/" + filePathName);
            if (SR == null)
            {
                resultStr = "StreamReader Null";
            }
            else
            {
                resultStr = SR.ReadLine();
            }
            SR.Close();
        }
        catch (Exception e)
        {
            resultStr = null;
        }
    }
}