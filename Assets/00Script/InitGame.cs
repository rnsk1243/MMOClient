using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstValue;

public class InitGame : MonoBehaviour {

    CState mState;    
    CConnect mConnet;
    CListener mListener;
    CSender mSender;
    CInitDistinguishCode mInitDisCode;
    CreatePlayer mCreatePlayer;
    CPlayerManager mPlayerManager;
    PacketTransform ptr;

    public GameObject mBasicMyPlayer;
    public GameObject mOtherPlayer;

    private InitGame()
    { }

    // Use this for initialization
    void Awake()
    {
        mState = CState.GetInstance();
        //InitializeGame();
        //mConnet = CConnect.GetInstance();
        //mListener = CListener.GetInstance();
        //mSender = CSender.GetInstance();
    }

    private void Start()
    {
        StartCoroutine(InitStart());
        //       mState.SetConnectState(StateConnect.DistinguishCode);
        //MyVector3 pos = new MyVector3(1.1f, 1.2f, 1.3f);
        //MyVector3 rot = new MyVector3(2.1f, 2.2f, 2.3f);
        //MyVector3 sca = new MyVector3(3.1f, 3.2f, 3.3f);
        //MyTransform tr = new MyTransform(pos, rot, sca);
        //DataPacketInfo testSendData = new DataPacketInfo((int)ProtocolInfo.Tr, tr, "abc");
        //mSender.Sendn(ref testSendData);

    }

    IEnumerator InitStart()
    {
        Debug.Log("게임 초기화 요청 코루틴 시작");
        while(true)
        {
            InitializeGame();
            yield return new WaitForSeconds(0.3f);
        }
    }

    void InitializeGame()
    {
        switch (mState.GetConnectState())
        {
            case StateConnect.Connecting:
                mConnet = CConnect.GetInstance();
                break;
            case StateConnect.SenderListenReady:
                mSender = CSender.GetInstance();
                mListener = CListener.GetInstance();
                break;
            case StateConnect.DistinguishCode:
                mInitDisCode = CInitDistinguishCode.GetInstance();
                mInitDisCode.RequestMyDisCode();
                break;
            case StateConnect.CreateCharacter:
                mCreatePlayer = gameObject.AddComponent<CreatePlayer>();
                //mInitDisCode = CInitDistinguishCode.GetInstance();
                //mInitDisCode.GetMyDisCode();
                break;
            case StateConnect.AddComponent:
                mPlayerManager = CPlayerManager.GetInstance();
                mPlayerManager.AddPlayerComponent();
                //MyVector3 pos = new MyVector3(1.177f, 1.2f, 1.3f);
                //MyVector3 rot = new MyVector3(2.1f, 2.2f, 2.3f);
                //MyVector3 sca = new MyVector3(3.1f, 3.2f, 3.3f);
                //MyTransform tr = new MyTransform(pos, rot, sca);
                //ptr = new PacketTransform((int)ProtocolInfo.Tr, mInitDisCode.mMyDistinguishCode, tr);
                //Debug.Log("ptr.DistinguishCode = " + ptr.DistinguishCode);
                //mSender.Sendn(ptr, PacketKindEnum.Transform);
                //Debug.Log("AddComponent State");
                break;
            case StateConnect.GameStart:
                Debug.Log("Game Init 완료!!");
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        //DataPacketInfo quitInfo = new DataPacketInfo((int)ProtocolInfo.ExitGameProcess, (int)ProtocolDetail.Message, (int)ProtocolMessageTag.Text, "으악 나 죽네");
        //sender.Sendn(ref quitInfo);
        //Debug.Log("OnApplicationQuit 호출");
        mConnet.CloseStream();
        mConnet.CloseClient();
        //Debug.Log("readyNetWork null로 초기화 시킴");
        mListener.TerminaterThread();
        mSender.TerminaterThread();
        mListener = null;
        mSender = null;
        mConnet = null;
        //sender = null;
    }
}
