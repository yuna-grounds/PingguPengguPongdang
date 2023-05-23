
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RoomCanvasManager : MonoBehaviourPun
{
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject roomCanvas;
    [SerializeField] private TextMeshProUGUI Player1Name;
    [SerializeField] private TextMeshProUGUI Player2Name;
    
    private void Update()
    {
        if (PhotonNetworkManager.network == NETWORK_STATE.JoinedRoom)
        {
            for (int i = 0; i < 2; i++)
            {
                Player1Name.text = GameData.name;
                string player2 = "";
                if (PhotonNetwork.PlayerList.Where(x => x.NickName != GameData.name).Count() > 0)
                {
                    player2 = PhotonNetwork.PlayerList.Where(x => x.NickName != GameData.name).ToList()[0].NickName;
                }
                if (player2 == null || player2 == "") player2 = "";
                Player2Name.text = player2;
            }
        }
    }
    

    public void OnClickOptions()
    {
        roomCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        OptionsCanvasManager.prevCanvas = roomCanvas;

    }
    public void OnClickBackBtn()
    {
        PhotonNetworkManager.Instance.LeftRoom();
        roomCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);
    }

    public void OnClickReady()
    {
        print("A : " + PhotonNetwork.PlayerList.Count());

        if (PhotonNetwork.PlayerList.Count() >= 2f && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeSceneRPC", RpcTarget.All, "World_HJW");
        }

    }


    [PunRPC]
    private void ChangeSceneRPC(string targetSceneName)
    {
        PhotonNetwork.LoadLevel(targetSceneName);
    }
}
