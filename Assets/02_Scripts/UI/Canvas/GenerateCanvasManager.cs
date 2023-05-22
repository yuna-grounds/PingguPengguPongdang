using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject generateCanvas;
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject optionsCanvas;

    [SerializeField] private TextMeshProUGUI DuplicationText;
    [SerializeField] private TMP_InputField PlayerName;


    private bool characterFlag = false;


    private void Start()
    {
        characterFlag = false;
        DuplicationText.text = "캐릭터 이름을 지어주세요.";
    }
    public void OnClickCreateCharacter()
    {
        if(EmptyCheck())
        {
            DuplicationText.text = "닉네임 란이 비어있습니다.";
            return;
        }
        if (PhotonNetworkManager.Instance.DuplicateCheck(PlayerName.text))
        {
            DuplicationText.text = "닉네임이 중복되었습니다";
            return;
        }

        DuplicationText.text = "캐릭터 생성이 완료되었습니다.";
        characterFlag = true;
    }

    public bool EmptyCheck()
    {
        if (PlayerName.text.Equals(""))
        {
            return true;
        }
        return false;
    }

    public void OnClickOptions()
    {
        generateCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        OptionsCanvasManager.prevCanvas = generateCanvas;

    }

    public void OnClickBackBtn()
    {
        PhotonNetworkManager.Instance.DisConnectingServer();
        generateCanvas.SetActive(false);
        titleCanvas.SetActive(true);

    }

    private void Update()
    {
       if (characterFlag && PlayerName.text.Equals(""))
        {
            characterFlag = false;
            DuplicationText.text = "캐릭터 이름을 지어주세요.";
        }

        if (characterFlag && PhotonNetworkManager.network == NETWORK_STATE.JoinedLobby)
        {
            generateCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
            PlayerName.text = "";
        }
    }

}
