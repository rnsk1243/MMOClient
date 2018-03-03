using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ConstValue
{
    public enum ProtocolInfo
    {
        None,
        Request,
        Tr,
        Chat,
        NewLink // Area에 들어온 Link
    }

    public enum StateConnect
    {
        Connecting, // 연결 하는 중
        SenderListenReady, // Sender, Listener 생성
        DistinguishCode, // 식별코드 받는 중
        CreateCharacter,
        //SendMyCharacter, // 내가 할 캐릭 정하여 서버에 보내는 중
        //RecvCharacter, // 서버로부터 어떤 캐릭터 생성해야하는지 받는 중
        AddComponent, // 내 캐릭터에 필요한 컴포턴트 붙임.
        GameStart // 준비완료
    }

    public enum PacketKindEnum
    {
        Transform,
        Message
    }

    public enum ComponentEnum
    {
        MoveController,
        OtherMoveController
    }


    static public class ConstValueInfo
    {
        // 접속할 곳의 IP주소.
        public const string IPAddress = "127.0.0.1";
        // 접속할 곳의 포트 번호.
        public const int Port = 9000;
        public const int ListenThreadSleep = 10;
        public const int SendThreadSleep = 10;
        public const int WrongValue = -1; // 잘 못된 값. 혹은 아직 초기화 되지 않은 값.

        public const string ServerIP_TextName = "Text/ServerIPInfo.txt";
        public const string ServerPort_TextName = "Text/ServerPortInfo.txt";

        public const int BufSizeRecv = 1024;
        public const int BufSizeSend = 1024;
        public const int MessageBufSize = 128;
        public const int StartPointPacketKind = 0;
        public const int StartPointProtocol = 0;
        public const int StartPointDistinguishCode = 4; // 전체 받은 바이트중에 인덱스 [4]부터 디시리얼 할 것임.
        public const int StartPointTr = 8;
        public const int StartPointRequestVal = 8;
        public const int StartPointMessage = 12;
        public static readonly int[] PacketSizeArray = { (Marshal.SizeOf(typeof(PacketTransform))-4), (Marshal.SizeOf(typeof(PacketMessage))-4)}; // PacketKindEnum과 순서 맞추어야 함.

        public const float SpeedMove = 10.0f;
        public const float SpeedRot = 100.0f;
    }

    static public class RequestCollection
    {
        public const string SendDistinguishCode = "RequestMyDisCode";
    }
}


