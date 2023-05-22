
using TMPro;
using UnityEngine;

public class RoomCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject roomCanvas;
    [SerializeField] private TextMeshProUGUI Player1Name;
    [SerializeField] private TextMeshProUGUI Player2Name;

    private bool join = false;
    private void Update()
    {
        if (!join && roomCanvas.activeSelf)
        {
            string masterName = PhotonNetworkManager.Instance.GetRoomMasterPlayerName();
            if (masterName.Equals(GameData.name))
            {
                print("10");
                Player1Name.text = GameData.name;
                Player2Name.text = "¥Î±‚¡ﬂ";
            }
            else
            {
                Player1Name.text = masterName;
                Player2Name.text = GameData.name;
            }
            join = true;
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
        print("To-do something");
    }
}
