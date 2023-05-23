using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class UIController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject TitleCanvas;
    [SerializeField] private GameObject GenerateCanvas;
    [SerializeField] private TextMeshProUGUI DuplicationText;
    [SerializeField] private TMP_InputField PlayerName;
    [SerializeField] private GameObject LobbyCanvas;
    [SerializeField] private TextMeshProUGUI LobbyNickName;
    [SerializeField] private GameObject RoomCanvas;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject viewContent;
    [SerializeField] private GameObject optionCanvas;

    private GameObject OnCanvas;

    //----------------------------------------------------------------------//
    // 



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
        OnCanvas = TitleCanvas;
        NetworkSettings();
    }

    //----------------------------------------------------------------------//
    // title

    /// <summary>
    /// Title Canvas의 Start 버튼을 눌렀을 때 작동<br />
    /// - 로비 서버 접속 시도
    /// </summary>
    public void Title_OnClickStart()
    {
        Debug.Log("서버 접속 시작");
        PhotonNetwork.ConnectUsingSettings();
    }

    // 서버 접속에 대한 콜백 메서드
    // - Title Canvas를 끄고 Lobby Canvas를 보여줌<br />
    // - 캐릭터 생성
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");
        TitleCanvas.SetActive(false);
        GenerateCanvas.SetActive(true);
        OnCanvas = GenerateCanvas;

        PhotonNetwork.JoinLobby();
    }

    // 서버 접속에 대한 콜백 메서드
    // 서버 연결이 끊기면 모든 캔버스를 끄고 TitleCanvas로 돌아옴
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("접속 실패");

        TitleCanvas.SetActive(true);
        GenerateCanvas.SetActive(false);
        LobbyCanvas.SetActive(false);

        OnCanvas = TitleCanvas;
    }

    //----------------------------------------------------------------------//
    // Generate

    public void Generate_OnClickCreate()
    {
        Debug.Log("닉네임은 : " + PlayerName.text);
        if (PlayerName.text.Equals(""))
        {
            print("닉네임 란이 비어있습니다.");
            DuplicationText.text = "닉네임 란이 비어있습니다.";
            return;
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (PlayerName.text.Equals(player.NickName))
            {
                print("닉네임이 중복되었습니다");
                DuplicationText.text = "닉네임이 중복되었습니다";
                return;
            }
        }
        DuplicationText.text = "캐릭터 생성이 완료되었습니다.";
        PhotonNetwork.NickName = PlayerName.text;

        GenerateCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
        OnCanvas = LobbyCanvas;
    }

    //----------------------------------------------------------------------//
    // Lobby


    public override void OnJoinedLobby()
    {
        print("로비서버 활성화");
        LobbyNickName.text = PhotonNetwork.NickName;
        RoomUpdate();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("룸 리스트 초기화");
    }



    /// <summary>
    /// 룸 리스트 가져오기
    /// </summary>
    public void GetRoomList()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            return;
        }

        TypedLobby lobby = new TypedLobby("Default", LobbyType.Default);
        PhotonNetwork.GetCustomRoomList(lobby, string.Empty);
    }



    public void Lobby_OnClickRefresh()
    {
        Debug.Log("새로고침");
        RoomUpdate();
    }

    private void RoomUpdate()
    {
        int roomCount = PhotonNetwork.CountOfRooms;
        print("현재 방 개수 : " + roomCount);
        print("현재 viewCotent 자식 개수 : " + roomCount);
        for (int idx = 0; idx < viewContent.transform.childCount; idx++)
        {
            Destroy(viewContent.transform.GetChild(idx).gameObject);
        }

        for (int idx = 0; idx < roomCount; idx++)
        {
            GameObject room = Instantiate(roomPrefab);
            SetRoomListPosition(room, idx);
        }

        // Content 영역 높이 조정
        RectTransform contentRect = viewContent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 10 * 50);
    }

    private void SetRoomListPosition(GameObject room, int idx)
    {
        RectTransform rect = room.GetComponent<RectTransform>();

        room.transform.SetParent(viewContent.transform, false);
        rect.anchoredPosition = new Vector2(0, idx * -21);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 20f);
        rect.localScale = Vector3.one;
    }


    public void CreateRoom()
    {
        print("방 만들기");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        print("방 생성 성공");
        LobbyCanvas.SetActive(false);
        RoomCanvas.SetActive(true);
        OnCanvas = RoomCanvas;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방 생성 실패");
    }

    public void Lobby_OnClickFastJoinBtn()
    {
        print("빠른 방 입장");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        print("방 입장 성공");
        LobbyCanvas.SetActive(false);
        RoomCanvas.SetActive(true);
        OnCanvas = RoomCanvas;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방 입장 실패");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("빠른 방 입장 실패");
    }


    // ---------------------------------------------------------------- //
    public void OnClickBtnOption()
    {
        optionCanvas.SetActive(true);
        OnCanvas = optionCanvas;
    }

    public void OnClickExitBtn()
    {
        OnCanvas.SetActive(false);
    }

}
