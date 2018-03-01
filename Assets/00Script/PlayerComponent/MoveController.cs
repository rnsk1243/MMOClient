using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class MoveController : MonoBehaviour {

    private float mHorizontal;
    private float mVertical;
    private Transform mMyTransform;
    // Use this for initialization
    void Awake () {
        mHorizontal = 0.0f;
        mVertical = 0.0f;
        mMyTransform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        mHorizontal = Input.GetAxis("Horizontal");
        mVertical = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * mVertical) + (Vector3.right * mHorizontal);

        mMyTransform.Translate(moveDir * Time.deltaTime * ConstValueInfo.SpeedMove, Space.Self);
        mMyTransform.Rotate(Vector3.up * Time.deltaTime * ConstValueInfo.SpeedRot * Input.GetAxis("Mouse X"));
    }
}
