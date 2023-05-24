using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniCamSet : MonoBehaviourPun
{
    public GameObject miniCam;
    void Start()
    {
        if (photonView.IsMine)
        {
            GameObject cam = PhotonNetwork.Instantiate(miniCam.name, transform.position, Quaternion.identity);
            //GameObject cam = Instantiate(miniCam, transform.position, Quaternion.identity);
            cam.AddComponent<OVRManager>();
            cam.transform.SetParent(transform);
            cam.transform.parent.gameObject.AddComponent<OVRPlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
