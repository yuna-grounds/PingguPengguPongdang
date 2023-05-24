using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking.Types;
using ExitGames.Client.Photon;


public enum NETWORK_STATE 
{
    Disconnected,               // 연결 안됨
    Connecting,                 // 연결중
    Connected,                  // 연결완료
    Disconnecting,              // 연결이 되었으나 모종의 이유로 연결 끊김
    JoinedLobby,                // 로비에 접속됨
    CreatingRoom,               // 방 생성 중
    CreatedRoom,                // 방 생성 중
    FailedCreatedRoom,          // 방 생성 실패
    JoiningRoom,                // 방에 접속 중
    FailedJoiningRoom,          // 방에 접속 실패
    JoinedRoom,                  // 룸에 접속됨
    GameOn,                      // 게임 씬 접속
    Gaming,                      // 게임중
    GameEnd                     // 게임끝
}

public struct Info
{
    public string roomName;
    public string masterClientId;
    public int maxPlayers;
    public int playerCount;

    public Info(string _roomName, string _masterClientId, int _maxPlayers, int _playerCount)
    {
        this.roomName = _roomName;
        this.masterClientId = _masterClientId;
        this.maxPlayers = _maxPlayers;
        this.playerCount = _playerCount;
    }

    public string ToString()
    {
        return $"{this.roomName}, {this.masterClientId}, {this.maxPlayers}, {this.playerCount}";
    }

}



public class PhotonNetworkManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private static PhotonNetworkManager instance;
    public static PhotonNetworkManager Instance
    {
        get
        {
            if (instance == null) return null;
            return instance;
        }
    }

    public static NETWORK_STATE network = NETWORK_STATE.Disconnected;

    private void Awake()
    {
        network = NETWORK_STATE.Disconnected;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 포톤 네트워크 세팅<br/>
    /// 게임 버전 : 0.1
    /// 데이터 송수신 횟수 프레임당 60회
    /// </summary>
    private void NetworkSettings()
    {
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
    }

    // 게임이 시작되면 네트워크 세팅을 먼저 함
    public void Start()
    {
        NetworkSettings();
    }


    // 서버 접속 메소드
    public void ConnectingLobby()
    {
        print("서버 접속 중 . . .");
        network = NETWORK_STATE.Connecting;
        PhotonNetwork.ConnectUsingSettings();
    }

    // 서버 연결 종료 메소드
    public void DisConnectingServer()
    {
        print("서버 접속 해제");
        network = NETWORK_STATE.Disconnecting;
        PhotonNetwork.Disconnect();
    }

    // 닉네임 중복검사 메소드
    //public bool DuplicateCheck(string name)
    //{
    //    foreach (Player player in PhotonNetwork.PlayerList)
    //    {
    //        if (name.Equals(player.NickName))
    //        {
    //            return true;
    //        }
    //    }
    //    PhotonNetwork.NickName = name;
    //    GameData.name = PhotonNetwork.NickName;
    //    return false;
    //}




    // 서버 연결 콜백 메소드 -------------------------------


    // 서버 접속에 대한 콜백 메서드
    // - Title Canvas를 끄고 Lobby Canvas를 보여줌<br />
    // - 캐릭터 생성
    public override void OnConnectedToMaster()
    {
        print("서버 접속 완료");
        network = NETWORK_STATE.Connected;
        PhotonNetwork.JoinLobby();
        print("로비 접속 중 . . .");
    }

    // 서버 접속에 대한 콜백 메서드
    // 서버 연결이 끊기면 모든 캔버스를 끄고 TitleCanvas로 돌아옴
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("서버 접속 끊김");
        network = NETWORK_STATE.Disconnected;
    }

    // 로비 서버 접속 메서드 ----------------------------

    public override void OnJoinedLobby()
    {
        print("로비 접속 완료");
        network = NETWORK_STATE.JoinedLobby;
    }

    // 룸 관련 메서드 -----------------------------------

    private string CreateRoomRandomName()
    {
        RandomStringGenerator generator = new RandomStringGenerator();
        string randomString = generator.GenerateRandomString(5);
        return randomString;
    }

    public void CreateRoom()
    {
        network = NETWORK_STATE.CreatingRoom;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        string name = CreateRoomRandomName();

        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "RoomName", name },
            { "MasterPlayer", PhotonNetwork.NickName }
        };

        PhotonNetwork.CreateRoom(
            name,
            options
        );
    }

    public void SetNickName(string name)
    {
        PhotonNetwork.NickName = name;
    }

    public void FastJoinRoom()
    {
        network = NETWORK_STATE.JoiningRoom;
        PhotonNetwork.JoinRandomRoom();
    }

    public int GetRoomListCount()
    {
        return PhotonNetwork.CountOfRooms;
    }

    public static bool joinedPlayer = false;
    public static string joinedPlayerName = "";
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        joinedPlayer = true;
        joinedPlayerName = newPlayer.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        joinedPlayer = false;
        joinedPlayerName = "";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //print("룸 정보 업데이트");
        //foreach(RoomInfo info in roomList)
        //{
        //    roomInfoList.Add(new Info(
        //        info.Name,
        //        info.masterClientId,
        //        info.MaxPlayers,
        //        info.PlayerCount
        //    ));
        //    print("룸 정보(이름) : " + info.Name);
        //    print("룸 정보(최대인원수) : " + info.MaxPlayers);
        //    print("룸 정보(인원수) : " + info.PlayerCount);
        //    print("룸 정보(마스터아이디) : " + info.masterClientId);
        //}
    }

    public override void OnCreatedRoom()
    {
        network = NETWORK_STATE.CreatedRoom;
        print("방 생성 성공");
        PhotonNetwork.JoinRoom(CreateRoomRandomName());
        network = NETWORK_STATE.JoiningRoom;

    }

    public override void OnJoinedRoom()
    {
        print("방 입장");
        network = NETWORK_STATE.JoinedRoom;
        if (PhotonNetwork.IsMasterClient)
        {
            print($"{GameData.name}은 master Client입니다");
        }
        else
        {
            print($"{GameData.name}은 master Client가 아닙니다");
        }


    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        network = NETWORK_STATE.FailedCreatedRoom;
        print("방 생성 실패" + message);
        network = NETWORK_STATE.JoinedLobby;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        network = NETWORK_STATE.FailedJoiningRoom;

        print("방 입장 실패");
        network = NETWORK_STATE.JoinedLobby;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        network = NETWORK_STATE.FailedJoiningRoom;

        print("빠른 방 입장 실패");
        network = NETWORK_STATE.JoinedLobby;
    }


    public Info GetRoomMasterPlayerName()
    {
        if (PhotonNetwork.InRoom)
        {
            Room currentRoom = PhotonNetwork.CurrentRoom;

            //PhotonNetwork.PlayerList[0].SetCustomProperties(
            //    new ExitGames.Client.Photon.Hashtable {
            //        { "PlayerTag", "" },
            //        {"PlayerTag2", "" }
            //    });

            string roomName = currentRoom.Name;
            int playerCount = currentRoom.PlayerCount;
            int maxPlayers = currentRoom.MaxPlayers;
            string masterPlayer = "";
            if (currentRoom.CustomProperties != null && currentRoom.CustomProperties.Count > 0)
            {
                // CustomProperties에서 원하는 키에 해당하는 값을 가져옴
                if (currentRoom.CustomProperties.ContainsKey("MasterPlayer"))
                {
                    masterPlayer = currentRoom.CustomProperties["MasterPlayer"].ToString();
                }
                if (currentRoom.CustomProperties.ContainsKey("RoomName"))
                {
                    roomName = currentRoom.CustomProperties["RoomName"].ToString();
                }
                print("룸 이름은 : " + roomName);

            }

            Debug.Log("Current Room Name: " + roomName);
            Debug.Log("Current Player Count: " + playerCount);
            Debug.Log("Max Players: " + maxPlayers);
            Debug.Log("Master Player: " + masterPlayer);

            return new Info(
                roomName,
                masterPlayer,
                maxPlayers,
                playerCount
            );

        }
        return new Info("", "", 0,0);
    }

    public void LeftRoom()
    {
        print("방 나오기 시도");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        print("방 나와서 로비 접속함");
        network = NETWORK_STATE.JoinedLobby; 
        
        

    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(network == NETWORK_STATE.GameOn) 
        { 
            


        }
    }
}
