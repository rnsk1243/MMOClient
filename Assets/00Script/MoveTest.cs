using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour {

    Transform mTr;
    Vector3 mTargetVector3;

	// Use this for initialization
	void Start () {
        mTr = GetComponent<Transform>();
        mTargetVector3 = new Vector3(-0.0001f, 1.0f, 0.01f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //mTr.position = Vector3.Lerp(mTr.position, mTargetVector3, 0.01f);
        //mTr.position = Vector3.MoveTowards(mTr.position, mTargetVector3, 0.1f);
        //float moved = Mathf.Abs((mTr.position.magnitude - mTargetVector3.magnitude));
        float moved = (mTr.position.magnitude - mTargetVector3.magnitude);
        //Vector3 v3 = mTargetVector3 - mTr.position;
        //Vector3 v3_ = v3.normalized;
        //mTr.Translate(v3_ * Time.deltaTime * ConstValue.ConstValueInfo.SpeedMove, Space.Self);
        //Debug.Log("my = " + mTr.position.magnitude);
        //Debug.Log("mTarget = " + mTargetVector3.magnitude);
        //Debug.Log("my - mTarget = " + (mTr.position.magnitude - mTargetVector3.magnitude));
            Vector3 v3 = mTargetVector3 - mTr.position;
            Vector3 v3_ = v3.normalized;
        moved = v3.magnitude;
        //Debug.Log("my - mTarget = " + (mTr.position.magnitude - mTargetVector3.magnitude));
        Debug.Log("v3.magnitude = " + moved);
        if (moved > 0.1f)
        {
            mTr.Translate(v3_ * Time.deltaTime * ConstValue.ConstValueInfo.SpeedMove, Space.Self);
            Debug.Log("이동중");
        }else
        {
            mTr.position = mTargetVector3;
            Debug.Log("목표 도착");
        }
           // Debug.Log("my = " + mTr.position.magnitude);
           // Debug.Log("mTarget = " + mTargetVector3.magnitude);


        //mTr.Translate(v3 * Time.deltaTime, Space.Self);
    }
}
