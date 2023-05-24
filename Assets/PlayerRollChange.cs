using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class PlayerRollChange : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI Player1Name;
    [SerializeField] private TextMeshProUGUI Player2Name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RollChange();
    }

    public void RollChange()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // 플레이어 1 번이 공격일 경우
        {
/*            Player1Name.text = PhotonNetwork.PlayerList[0].NickName;
            Player2Name.text = PhotonNetwork.PlayerList[0].NickName;*/
            Player1Name.text = PhotonNetwork.PlayerList[0].NickName;
            Player2Name.text = "Player 2";
        }
        if (Input.GetKeyDown(KeyCode.W)) // 플레이어 2 번이 공격일 경우
        {
            /*Player1Name.text = PhotonNetwork.PlayerList[0].NickName;
            Player2Name.text = PhotonNetwork.PlayerList[0].NickName; */
            Player1Name.text = "Player 2";
            Player2Name.text = PhotonNetwork.PlayerList[0].NickName;
        }

    }

}
