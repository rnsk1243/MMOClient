using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class OtherPlayerMoveController : MonoBehaviour {

    Transform mTr;
    public Vector3 mNewPosition;
    public Vector3 mNewRotate;

    // Use this for initialization
    void Awake() {
        mTr = GetComponent<Transform>();
        mNewPosition = mTr.position;
        mNewRotate = mTr.eulerAngles;
    }

    public void MovePositionUpdate(ref MyVector3 goalPosition)
    {
        //Debug.Log("앙 좋아!");
        mNewPosition = CUtil.ConvertToVector3(ref goalPosition);

        //Debug.Log("/" + discode + "/goal.x = " + goalPosition.x + "goal.z = " + goalPosition.z);
    }

    public void MoveRotateUpdate(ref MyVector3 goalRotation)
    {
        mNewRotate = CUtil.ConvertToVector3(ref goalRotation);
    }

    private void MovePosition()
    {
        Vector3 directionVector = mNewPosition - mTr.position;
        float remainDirectionSize = directionVector.magnitude;
        //Debug.Log("/" + mdiscode + "/directionVector.x = " + directionVector.x + "directionVector.z = " + directionVector.z);
        //Debug.Log("/" + mdiscode + "/mNewPosition.x = " + mNewPosition.x + "mNewPosition.z = " + mNewPosition.z);
        //Debug.Log("/" + mdiscode + "/mTr.x = " + mTr.position.x + "mTr.z = " + mTr.position.z);
        //Debug.Log("/"+ mdiscode + "remainDirectionSize = " + remainDirectionSize);
        if (remainDirectionSize > 0.1f)
        {
            Vector3 directionNormal = directionVector.normalized;
            mTr.Translate(directionNormal * Time.deltaTime * ConstValueInfo.SpeedMove, Space.World); // 必ずSpace.Worldで使うこと。
            //Debug.Log("이동중");
        }
        else
        {
            mTr.position = mNewPosition;
        }
    }

    private void MoveRotate()
    {
        Vector3 directionVector = (mNewRotate - mTr.eulerAngles);

        if (directionVector.y > 180 || directionVector.y < -180)
        {
            directionVector.y = -directionVector.y;
        }

        float remainRotationSize = directionVector.magnitude;
        if (remainRotationSize > 2.0f && remainRotationSize < 360)
        {
            Vector3 dirNormal = directionVector.normalized;
            mTr.transform.Rotate(dirNormal * Time.deltaTime * ConstValueInfo.SpeedRot, Space.Self);
        }
    }
	

	// Update is called once per frame
	void FixedUpdate() {

        MovePosition();
        MoveRotate();
    }
}
