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
    // Use this for initialization
    void Awake () {
        mState = CState.GetInstance();
        mSender = CSender.GetInstance();
        mDisCode = CInitDistinguishCode.GetInstance();
        mHorizontal = 0.0f;
        mVertical = 0.0f;
        mMyTransform = GetComponent<Transform>();
        StartCoroutine(SendMyTransform());
    }

    IEnumerator SendMyTransform()
    {
        while(true)
        {
            if(mState.IsCurConnectState(StateConnect.GameStart))
            {
                PacketTransform sendMyTr = new PacketTransform((int)ProtocolInfo.Tr, mDisCode.GetMyDisCode(), CUtil.ConvertGetMyTransform(ref mMyTransform));
                mSender.PushSendData(sendMyTr, PacketKindEnum.Transform);
            }
            yield return new WaitForSeconds(0.3f);
        }
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
