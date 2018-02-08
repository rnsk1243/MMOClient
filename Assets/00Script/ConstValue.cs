﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ConstValue
{
    public enum ProtocolInfo
    {
        None,
        Tr,
        Chat
    }

    enum StateConnect
    {
        Connecting, // 연결 하는 중
        DistinguishCode, // 식별코드 받는 중
        SendMyCharacter, // 내가 할 캐릭 정하여 서버에 보내는 중
        RecvCharacter, // 서버로부터 어떤 캐릭터 생성해야하는지 받는 중
        AddComponent, // 내 캐릭터에 필요한 컴포턴트 붙임.
        GameStart // 준비완료
    }

    public class ConstValueInfo
    {
        // 접속할 곳의 IP주소.
        public const string IPAddress = "127.0.0.1";
        // 접속할 곳의 포트 번호.
        public const int Port = 9000;

        public const string ServerIP_TextName = "Text/ServerIPInfo.txt";
        public const string ServerPort_TextName = "Text/ServerPortInfo.txt";

        public const int BufSizeRecv = 1024;
        public const int BufSizeSend = 1024;
        public const int ChatBufSize = 128;
    }


}


