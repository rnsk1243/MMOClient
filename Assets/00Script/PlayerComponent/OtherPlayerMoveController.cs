using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class OtherPlayerMoveController : MonoBehaviour {

    Transform mTr;
    Vector3 mNewPosition;
    Vector3 mNewRotation;

    // Use this for initialization
    void Awake() {
        mTr = GetComponent<Transform>();
        mNewPosition = mTr.position;
    }

    public void Move(ref MyVector3 goalPosition)
    {
        mNewPosition = CUtil.ConvertToVector3(ref goalPosition);
    }
	

	// Update is called once per frame
	void FixedUpdate() {

        Vector3 directionVector = mNewPosition - mTr.position;
        float remainDirectionSize = directionVector.magnitude;
        if (remainDirectionSize > 0.1f)
        {
            Vector3 directionNomal = directionVector.normalized;
            mTr.Translate(directionNomal * Time.deltaTime * ConstValueInfo.SpeedMove, Space.Self);
            //Debug.Log("이동중");
        }else
        {
            mTr.position = mNewPosition;
        }

    }
}
