using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textHost;
    [SerializeField] private TextMeshProUGUI _textRoomName;
    [SerializeField] private TextMeshProUGUI _textCount;
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        _textHost.text = "N7zFp";
        _textRoomName.text = roomInfo.Name;
        _textCount.text = "1/2";
    }
}