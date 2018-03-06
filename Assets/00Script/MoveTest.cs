using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;
using System.Runtime.InteropServices;

public class MoveTest : MonoBehaviour {

    Transform mTr;
    //public Vector3 mNewRotate;
    public Vector3 mNewPosition;

	// Use this for initialization
	void Start () {
        mTr = GetComponent<Transform>();
        mNewPosition = new Vector3();
        //mNewRotate = mTr.eulerAngles;
        int siz = (Marshal.SizeOf(typeof(PacketDeleteObj)) - 4);
        Debug.Log("dd = " + siz);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        mTr.Translate(mNewPosition * Time.deltaTime);

    //    Vector3 directionVector = (mNewRotate - mTr.eulerAngles);

    //    if(directionVector.y > 180 || directionVector.y < -180)
    //    {
    //        directionVector.y = -directionVector.y;
    //    }

    //    float remainRotationSize = directionVector.magnitude;
    //    Debug.Log("remainRotationSize = " + remainRotationSize);
    //    if(remainRotationSize > 2.0f && remainRotationSize < 360)
    //    {
    //        Vector3 dirNormal = directionVector.normalized;
    //        mTr.transform.Rotate(dirNormal * Time.deltaTime * ConstValueInfo.SpeedRot, Space.Self);
    //    }
    }
}
