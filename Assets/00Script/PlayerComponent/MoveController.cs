using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class MoveController : MonoBehaviour {

    private float mHorizontal;
    private float mVertical;
    private Transform mMyTransform;
    private CSender mSender;
    private CInitDistinguishCode mDisCode;
    private CState mState;
    private Vector3 mDirectionNormal;
    // Use this for initialization
    void Awake () {
        mState = CState.GetInstance();
        mSender = CSender.GetInstance();
        mDisCode = CInitDistinguishCode.GetInstance();
        mHorizontal = 0.0f;
        mVertical = 0.0f;
        mMyTransform = GetComponent<Transform>();
        mDirectionNormal = new Vector3();
        StartCoroutine(SendMyTransform());
    }

    IEnumerator SendMyTransform()
    {
        while(true)
        {
            //Debug.Log("mag = " + mDirectionNormal.magnitude);
            if(mState.IsCurConnectState(StateConnect.GameStart) && 0 < mDirectionNormal.magnitude)
            {
                PacketTransform sendMyTr = new PacketTransform((int)ProtocolInfo.Tr, mDisCode.GetMyDisCode(), CUtil.ConvertGetMyTransform(ref mMyTransform));
                //Debug.Log("보내는 rotate 값 = " + sendMyTr.Tr.Rotation.x + "// y = " + sendMyTr.Tr.Rotation.y + "// z = " + sendMyTr.Tr.Rotation.z );
                mSender.PushSendData(sendMyTr, PacketKindEnum.Transform);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update () {
        mHorizontal = Input.GetAxis("Horizontal");
        mVertical = Input.GetAxis("Vertical");

        mDirectionNormal = (Vector3.forward * mVertical) + (Vector3.right * mHorizontal);
        //Debug.Log("moveDir.x = " + moveDir.x + " / moveDir.y = " + moveDir.y + " / moveDir.z = " + moveDir.z);
        mMyTransform.Translate(mDirectionNormal * Time.deltaTime * ConstValueInfo.SpeedMove, Space.Self);
        mMyTransform.Rotate(Vector3.up * Time.deltaTime * ConstValueInfo.SpeedRot * Input.GetAxis("Mouse X"));
    }
}
